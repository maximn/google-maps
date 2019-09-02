using GoogleMapsApi.Entities.Common;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi.Engine
{
    public delegate Uri UriCreatedDelegate(Uri uri);
    public delegate void RawResponseReceivedDelegate(HttpResponseMessage data);

    public abstract class MapsApiGenericEngine<TRequest, TResponse>
		where TRequest : MapsBaseRequest, new()
		where TResponse : IResponseFor<TRequest>
	{
        internal static event UriCreatedDelegate OnUriCreated;
        internal static event RawResponseReceivedDelegate OnRawResponseReceived;

        /// <summary>
        /// Determines the maximum number of concurrent HTTP connections to open to this engine's host address. The default value is 2 connections.
        /// </summary>
        /// <remarks>
        /// This value is determined by the ServicePointManager and is shared across other engines that use the same host address.
        /// </remarks>
        public static int HttpConnectionLimit
		{
			get => HttpServicePoint.ConnectionLimit;
            set => HttpServicePoint.ConnectionLimit = value;
        }

		/// <summary>
		/// Determines the maximum number of concurrent HTTPS connections to open to this engine's host address. The default value is 2 connections.
		/// </summary>
		/// <remarks>
		/// This value is determined by the ServicePointManager and is shared across other engines that use the same host address.
		/// </remarks>
		public static int HttpsConnectionLimit
		{
			get => HttpsServicePoint.ConnectionLimit;
            set => HttpsServicePoint.ConnectionLimit = value;
        }

		private static ServicePoint HttpServicePoint { get; set; }
		private static ServicePoint HttpsServicePoint { get; set; }
		internal static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(100);

		private const string AuthenticationFailedMessage =
			"The request to Google API failed with HTTP error '(403) Forbidden', which usually indicates that the provided client ID or signing key is invalid or expired.";

		static MapsApiGenericEngine()
		{
			var baseUrl = new TRequest().BaseUrl;
			HttpServicePoint = ServicePointManager.FindServicePoint(new Uri("http://" + baseUrl));
			HttpsServicePoint = ServicePointManager.FindServicePoint(new Uri("https://" + baseUrl));
		}

        /// <summary>
        /// A method that wraps responses into easily understood exceptions
        /// 
        /// Obsolete: We should be moving to returning raw http responses, as opposed to wrapping them
        /// </summary>
        /// <param name="request"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        [Obsolete("We should be moving to returning raw http responses, as opposed to wrapping them")]
		protected internal static async Task<TResponse> QueryGoogleApi(TRequest request, TimeSpan timeout)
        {
            return await QueryGoogleApiAsync(request, timeout);
        }

        protected internal static async Task<TResponse> QueryGoogleApiAsync(TRequest request, TimeSpan timeout, CancellationToken token = default)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

            var uri = request.GetUri();
            if (OnUriCreated != null)
            {
                uri = OnUriCreated(uri);
            }

            var client = new HttpClient {Timeout = timeout};

            var httpResponse = await client.GetAsync(uri, token);

            if (!httpResponse.IsSuccessStatusCode)
            {
                switch (httpResponse.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.ProxyAuthenticationRequired:
                    case HttpStatusCode.Unauthorized:
                        throw new AuthenticationException(await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
                    case HttpStatusCode.GatewayTimeout:
                    case HttpStatusCode.RequestTimeout:
                        throw new TimeoutException($"The request has exceeded the timeout limit of {timeout} and has been aborted.");
                    default:
                        throw new HttpRequestException($"Failed with HttpResponse: {httpResponse.StatusCode} and message: {httpResponse.ReasonPhrase}");
                }
            }

            OnRawResponseReceived?.Invoke(httpResponse);

            return JsonConvert.DeserializeObject<TResponse>(await httpResponse.Content.ReadAsStringAsync());
		}
    }
}