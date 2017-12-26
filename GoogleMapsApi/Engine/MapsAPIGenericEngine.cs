using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using Newtonsoft.Json;

namespace GoogleMapsApi.Engine
{
    public delegate Uri UriCreatedDelegate(Uri uri);
    public delegate void RawResponseReciviedDelegate(byte[] data);

    public abstract class MapsAPIGenericEngine<TRequest, TResponse>
		where TRequest : MapsBaseRequest, new()
		where TResponse : IResponseFor<TRequest>
	{
        internal static event UriCreatedDelegate OnUriCreated;
        internal static event RawResponseReciviedDelegate OnRawResponseRecivied;

#if NET45

        /// <summary>
        /// Determines the maximum number of concurrent HTTP connections to open to this engine's host address. The default value is 2 connections.
        /// </summary>
        /// <remarks>
        /// This value is determined by the ServicePointManager and is shared across other engines that use the same host address.
        /// </remarks>
        public static int HttpConnectionLimit
		{
			get { return HttpServicePoint.ConnectionLimit; }
			set { HttpServicePoint.ConnectionLimit = value; }
		}

		/// <summary>
		/// Determines the maximum number of concurrent HTTPS connections to open to this engine's host address. The default value is 2 connections.
		/// </summary>
		/// <remarks>
		/// This value is determined by the ServicePointManager and is shared across other engines that use the same host address.
		/// </remarks>
		public static int HttpsConnectionLimit
		{
			get { return HttpsServicePoint.ConnectionLimit; }
			set { HttpsServicePoint.ConnectionLimit = value; }
		}



        private static ServicePoint HttpServicePoint { get; set; }
		private static ServicePoint HttpsServicePoint { get; set; }

#endif

		internal static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(100);

		private const string AuthenticationFailedMessage =
			"The request to Google API failed with HTTP error '(403) Forbidden', which usually indicates that the provided client ID or signing key is invalid or expired.";

		static MapsAPIGenericEngine()
		{
			var baseUrl = new TRequest().BaseUrl;

#if NET45
            HttpServicePoint = ServicePointManager.FindServicePoint(new Uri("http://" + baseUrl));
			HttpsServicePoint = ServicePointManager.FindServicePoint(new Uri("https://" + baseUrl));
#endif
        }

        /// <summary>
        /// A method that wraps responses into easily understood exceptions
        /// 
        /// Obsolete: We should be moving to returning raw http resonses, as opposed to wrapping them
        /// </summary>
        /// <param name="request"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        [Obsolete]
		protected internal static async Task<TResponse> QueryGoogleAPI(TRequest request, TimeSpan timeout)
		{
			if (request == null)
				throw new ArgumentNullException("request");

			var uri = request.GetUri();
			if (OnUriCreated != null)
			{
			    uri = OnUriCreated(uri);
			}

            var data = await new HttpClient().DownloadDataTaskAsync(uri, timeout).ConfigureAwait(false);
            OnRawResponseRecivied?.Invoke(data);
            return Deserialize(data);
		}

		protected internal static async Task<TResponse> QueryGoogleAPIAsync(TRequest request, TimeSpan timeout, CancellationToken token = default(CancellationToken))
		{
			if (request == null)
				throw new ArgumentNullException("request");

            var uri = request.GetUri();
            if (OnUriCreated != null)
            {
                uri = OnUriCreated(uri);
            }

		    var response = await new HttpClient().DownloadDataTaskAsyncAsString(uri, timeout, token);

            return JsonConvert.DeserializeObject<TResponse>(response);
		}

		private static TResponse Deserialize(byte[] serializedObject)
		{
			var serializer = new DataContractJsonSerializer(typeof(TResponse));
			var stream = new MemoryStream(serializedObject, false);
			return (TResponse)serializer.ReadObject(stream);
		}

    }
}