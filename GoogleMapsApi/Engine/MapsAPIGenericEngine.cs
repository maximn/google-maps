using GoogleMapsApi.Entities.Common;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
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
		internal static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(100);

		protected internal static async Task<TResponse> QueryGoogleAPIAsync(TRequest request, TimeSpan timeout, CancellationToken token = default)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

            var requstUri = request.GetUri();
            var uri = OnUriCreated?.Invoke(requstUri) ?? requstUri;
            
		    var client = new HttpClient();

			var response = await client.DownloadDataTaskAsyncAsString(uri, timeout, token).ConfigureAwait(false);

            OnRawResponseReceived?.Invoke(Encoding.UTF8.GetBytes(response));

            return JsonConvert.DeserializeObject<TResponse>(response);
		}

    }
}