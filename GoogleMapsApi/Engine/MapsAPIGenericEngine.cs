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
			using var activity = GoogleMapsActivity.Source.StartActivity($"GoogleMapsApi {apiName}", ActivityKind.Client);
			if (activity is not null)
			{
				activity.SetTag("gmaps.api", apiName);
				activity.SetTag("http.request.method", body is null ? "GET" : "POST");
				activity.SetTag("server.address", uri.Host);
				activity.SetTag("url.full", RedactUrl(uri));
			}

			try
			{
				string responseContent;
				try
				{
					responseContent = await GetHttpResponseAsync(httpClient, uri, body, timeout, token).ConfigureAwait(false);
				}
				finally
				{
					body?.Dispose();
				}

				onRawResponseReceived?.Invoke(Encoding.UTF8.GetBytes(responseContent));

				var response = JsonSerializer.Deserialize<TResponse>(responseContent, jsonOptions)!;

				if (activity is not null)
				{
					var responseStatus = GetResponseStatus(response);
					if (responseStatus is not null)
						activity.SetTag("gmaps.response_status", responseStatus);
				}

				return response;
			}
			catch (Exception ex)
			{
				activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
				activity?.SetTag("error.type", ex.GetType().FullName);
				throw;
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

		private static string? GetResponseStatus(TResponse response)
		{
			var property = statusProperties.GetOrAdd(typeof(TResponse), static t => t.GetProperty("Status"));
			return property?.GetValue(response)?.ToString();
		}

		private static async Task<string> GetHttpResponseAsync(HttpClient httpClient, Uri uri, HttpContent? body, TimeSpan timeout, CancellationToken cancellationToken)
		{
			using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			if (timeout != TimeSpan.FromMilliseconds(-1))
				cts.CancelAfter(timeout);

			try
			{
				using var response = body == null
					? await httpClient.GetAsync(uri, cts.Token).ConfigureAwait(false)
					: await httpClient.PostAsync(uri, body, cts.Token).ConfigureAwait(false);
				Activity.Current?.SetTag("http.response.status_code", (int)response.StatusCode);
				await HandleHttpResponse(response, timeout).ConfigureAwait(false);
				return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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

				if (response.StatusCode == HttpStatusCode.Forbidden ||
					response.StatusCode == HttpStatusCode.ProxyAuthenticationRequired ||
					response.StatusCode == HttpStatusCode.Unauthorized)
					throw new System.Security.Authentication.AuthenticationException(responseContent);

				if (response.StatusCode == HttpStatusCode.GatewayTimeout ||
					response.StatusCode == HttpStatusCode.RequestTimeout)
					throw new TimeoutException($"The request has exceeded the timeout limit of {timeout} and has been aborted.");

				throw new HttpRequestException($"Failed with HttpResponse: {response.StatusCode} and message: {response.ReasonPhrase}");
			}
		}

    }
}