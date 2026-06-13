using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Engine.JsonConverters;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
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
			try
			{
				// Binary endpoints (e.g. Solar GeoTIFF) carry raw bytes, not JSON.
				if (typeof(IBinaryResponse).IsAssignableFrom(typeof(TResponse)))
				{
					var (bytes, contentType) = await SendAsync(httpClient, uri, body, timeout, token, async response =>
						(await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false),
						 response.Content.Headers.ContentType?.MediaType)).ConfigureAwait(false);

					onRawResponseReceived?.Invoke(bytes);

					var binaryResult = Activator.CreateInstance<TResponse>();
					var binary = (IBinaryResponse)binaryResult!;
					binary.Content = bytes;
					binary.ContentType = contentType;
					return binaryResult;
				}

				var responseContent = await SendAsync(httpClient, uri, body, timeout, token,
					response => response.Content.ReadAsStringAsync()).ConfigureAwait(false);

				onRawResponseReceived?.Invoke(Encoding.UTF8.GetBytes(responseContent));

				return JsonSerializer.Deserialize<TResponse>(responseContent, jsonOptions)!;
			}
			finally
			{
				body?.Dispose();
			}
		}

		private static async Task<T> SendAsync<T>(HttpClient httpClient, Uri uri, HttpContent? body, TimeSpan timeout, CancellationToken cancellationToken, Func<HttpResponseMessage, Task<T>> readContent)
		{
			using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			if (timeout != TimeSpan.FromMilliseconds(-1))
				cts.CancelAfter(timeout);

			try
			{
				using var response = body == null
					? await httpClient.GetAsync(uri, cts.Token).ConfigureAwait(false)
					: await httpClient.PostAsync(uri, body, cts.Token).ConfigureAwait(false);
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