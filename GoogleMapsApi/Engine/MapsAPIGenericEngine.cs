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

    public abstract class MapsAPIGenericEngine<TRequest, TResponse>
		where TRequest : MapsBaseRequest, new()
		where TResponse : IResponseFor<TRequest>
	{
        internal static event UriCreatedDelegate? OnUriCreated;
        internal static event RawResponseReceivedDelegate? OnRawResponseReceived;

		private static readonly HttpClient client = new HttpClient();
		private static readonly JsonSerializerOptions jsonOptions = JsonSerializerConfiguration.CreateOptions();

		protected internal static async Task<TResponse> QueryGoogleAPIAsync(TRequest request, TimeSpan timeout, CancellationToken token = default)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

            var requestUri = request.GetUri();
            var uri = OnUriCreated?.Invoke(requestUri) ?? requestUri;
            
		    var responseContent = await GetHttpResponseAsync(uri, timeout, token).ConfigureAwait(false);

            OnRawResponseReceived?.Invoke(Encoding.UTF8.GetBytes(responseContent));

            return JsonSerializer.Deserialize<TResponse>(responseContent, jsonOptions)!;
		}

		private static async Task<string> GetHttpResponseAsync(Uri uri, TimeSpan timeout, CancellationToken cancellationToken)
		{
			using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			if (timeout != TimeSpan.FromMilliseconds(-1))
				cts.CancelAfter(timeout);
			
			try
			{
				using var response = await client.GetAsync(uri, cts.Token).ConfigureAwait(false);
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