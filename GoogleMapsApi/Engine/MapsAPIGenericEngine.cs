using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Engine.JsonConverters;
using GoogleMapsApi.Diagnostics;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace GoogleMapsApi.Engine
{
    public delegate Uri UriCreatedDelegate(Uri uri);
    public delegate void RawResponseReceivedDelegate(byte[] data);

    internal abstract class MapsAPIGenericEngine<TRequest, TResponse>
		where TRequest : MapsBaseRequest, new()
		where TResponse : IResponseFor<TRequest>
	{
		private static readonly JsonSerializerOptions jsonOptions = JsonSerializerConfiguration.CreateOptions();

		private static readonly ConcurrentDictionary<Type, PropertyInfo?> statusProperties = new();

		private static readonly Regex secretQueryParameter =
			new Regex(@"([?&](?:key|signature)=)[^&]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		internal static async Task<TResponse> QueryGoogleAPIAsync(
			HttpClient httpClient,
			TRequest request,
			TimeSpan timeout,
			CancellationToken token,
			UriCreatedDelegate? onUriCreated,
			RawResponseReceivedDelegate? onRawResponseReceived)
		{
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			var requestUri = request.GetUri();
			var uri = onUriCreated?.Invoke(requestUri) ?? requestUri;

			var body = request.GetRequestBody();

			var apiName = GetApiName();
			var method = body is null ? "GET" : "POST";
			using var activity = GoogleMapsActivity.Source.StartActivity($"GoogleMapsApi {apiName}", ActivityKind.Client);
			if (activity is not null)
			{
				activity.SetTag("gmaps.api", apiName);
				activity.SetTag("http.request.method", method);
				activity.SetTag("server.address", uri.Host);
				activity.SetTag("url.full", RedactUrl(uri));
			}

			var startTimestamp = Stopwatch.GetTimestamp();
			int? statusCode = null;
			string? responseStatus = null;
			string? errorType = null;
			Action<int> onResponseStatus = code =>
			{
				statusCode = code;
				activity?.SetTag("http.response.status_code", code);
			};

			try
			{
				// Binary endpoints (e.g. Solar GeoTIFF) carry raw bytes, not JSON.
				if (typeof(IBinaryResponse).IsAssignableFrom(typeof(TResponse)))
				{
					var (bytes, contentType) = await SendAsync(httpClient, uri, body, timeout, token, onResponseStatus, async response =>
						(await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false),
						 response.Content.Headers.ContentType?.MediaType)).ConfigureAwait(false);

					onRawResponseReceived?.Invoke(bytes);

					var binaryResult = Activator.CreateInstance<TResponse>();
					var binary = (IBinaryResponse)binaryResult!;
					binary.Content = bytes;
					binary.ContentType = contentType;
					return binaryResult;
				}

				var responseContent = await SendAsync(httpClient, uri, body, timeout, token, onResponseStatus,
					response => response.Content.ReadAsStringAsync()).ConfigureAwait(false);

				onRawResponseReceived?.Invoke(Encoding.UTF8.GetBytes(responseContent));

				var response = JsonSerializer.Deserialize<TResponse>(responseContent, jsonOptions)!;

				if (activity is not null || GoogleMapsMetrics.RequestDuration.Enabled)
				{
					responseStatus = GetResponseStatus(response);
					if (activity is not null && responseStatus is not null)
						activity.SetTag("gmaps.response_status", responseStatus);
				}

				return response;
			}
			catch (Exception ex)
			{
				errorType = ex.GetType().FullName;
				activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
				activity?.SetTag("error.type", errorType);
				throw;
			}
			finally
			{
				body?.Dispose();
				GoogleMapsMetrics.Record(apiName, method, statusCode, responseStatus, errorType, startTimestamp);
			}
		}

		private static string GetApiName()
		{
			var name = typeof(TRequest).Name;
			return name.EndsWith("Request", StringComparison.Ordinal)
				? name.Substring(0, name.Length - "Request".Length)
				: name;
		}

		private static string RedactUrl(Uri uri)
			=> secretQueryParameter.Replace(uri.AbsoluteUri, "$1REDACTED");

		private const int MaxErrorBodyLength = 2000;

		/// <summary>
		/// Prepares an error response body for inclusion in an exception message: secrets are redacted in
		/// case the service echoed the request URL back, and oversized bodies (HTML error pages) are cut off.
		/// </summary>
		private static string SanitizeErrorBody(string body)
		{
			var redacted = secretQueryParameter.Replace(body, "$1REDACTED");
			return redacted.Length > MaxErrorBodyLength
				? redacted.Substring(0, MaxErrorBodyLength) + "..."
				: redacted;
		}

		private static string? GetResponseStatus(TResponse response)
		{
			var property = statusProperties.GetOrAdd(typeof(TResponse), static t => t.GetProperty("Status"));
			return property?.GetValue(response)?.ToString();
		}

		private static async Task<T> SendAsync<T>(HttpClient httpClient, Uri uri, HttpContent? body, TimeSpan timeout, CancellationToken cancellationToken, Action<int>? onResponseStatus, Func<HttpResponseMessage, Task<T>> readContent)
		{
			using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			if (timeout != TimeSpan.FromMilliseconds(-1))
				cts.CancelAfter(timeout);

			try
			{
				using var response = body == null
					? await httpClient.GetAsync(uri, cts.Token).ConfigureAwait(false)
					: await httpClient.PostAsync(uri, body, cts.Token).ConfigureAwait(false);
				onResponseStatus?.Invoke((int)response.StatusCode);
				await HandleHttpResponse(response, timeout).ConfigureAwait(false);
				return await readContent(response).ConfigureAwait(false);
			}
			catch (OperationCanceledException) when (cts.Token.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
			{
				throw new TimeoutException($"The request has exceeded the timeout limit of {timeout} and has been aborted.");
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				// Re-throw OperationCanceledException when cancellation token was cancelled
				throw;
			}
			catch (TaskCanceledException ex)
			{
				// Check if this was due to our cancellation token being cancelled
				if (cancellationToken.IsCancellationRequested)
				{
					throw new OperationCanceledException("The operation was cancelled.", ex, cancellationToken);
				}
				// If not due to our cancellation token, it might be due to timeout, re-throw as is
				throw;
			}
		}

		private static async Task HandleHttpResponse(HttpResponseMessage response, TimeSpan timeout)
		{
			if (!response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
				var errorBody = string.IsNullOrWhiteSpace(responseContent) ? null : SanitizeErrorBody(responseContent);

				if (response.StatusCode == HttpStatusCode.Forbidden ||
					response.StatusCode == HttpStatusCode.ProxyAuthenticationRequired ||
					response.StatusCode == HttpStatusCode.Unauthorized)
					throw new System.Security.Authentication.AuthenticationException(errorBody ?? response.StatusCode.ToString());

				if (response.StatusCode == HttpStatusCode.GatewayTimeout ||
					response.StatusCode == HttpStatusCode.RequestTimeout)
					throw new TimeoutException($"The request has exceeded the timeout limit of {timeout} and has been aborted.");

				var message = $"Failed with HttpResponse: {response.StatusCode} and message: {response.ReasonPhrase}";
				throw new HttpRequestException(errorBody == null ? message : $"{message}. Response: {errorBody}");
			}
		}

    }
}
