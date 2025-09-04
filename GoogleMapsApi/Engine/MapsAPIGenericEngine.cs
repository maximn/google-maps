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
        internal static event UriCreatedDelegate OnUriCreated;
        internal static event RawResponseReceivedDelegate OnRawResponseReceived;

		private static readonly HttpClient client = new HttpClient();
		private static readonly JsonSerializerOptions jsonOptions = CreateJsonOptions();

		private static JsonSerializerOptions CreateJsonOptions()
		{
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};

			// Add JsonStringEnumConverter for all enums
			options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
			
			// Add custom converters
			options.Converters.Add(new PriceLevelJsonConverter());
			options.Converters.Add(new OverviewPolylineJsonConverter());
			
			// Add Duration converters for specific types
			options.Converters.Add(new DurationJsonConverter<GoogleMapsApi.Entities.DistanceMatrix.Response.Duration>());
			options.Converters.Add(new DurationJsonConverter<GoogleMapsApi.Entities.Directions.Response.Duration>());

			return options;
		}

		protected internal static async Task<TResponse> QueryGoogleAPIAsync(TRequest request, TimeSpan timeout, CancellationToken token = default)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

            var requestUri = request.GetUri();
            var uri = OnUriCreated?.Invoke(requestUri) ?? requestUri;
            
		    var responseContent = await GetHttpResponseAsync(uri, timeout, token).ConfigureAwait(false);

            OnRawResponseReceived?.Invoke(Encoding.UTF8.GetBytes(responseContent));

            return JsonSerializer.Deserialize<TResponse>(responseContent, jsonOptions);
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