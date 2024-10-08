using GoogleMapsApi.Entities.Common;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi.Engine
{
    public delegate Uri UriCreatedDelegate(Uri uri);
    public delegate void RawResponseReciviedDelegate(byte[] data);

    public abstract class MapsAPIGenericEngine<TRequest, TResponse>
		where TRequest : MapsBaseRequest, new()
		where TResponse : IResponseFor<TRequest>
	{
        internal static event UriCreatedDelegate OnUriCreated;
		internal static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(100);

		protected internal static async Task<TResponse> QueryGoogleAPIAsync(TRequest request, TimeSpan timeout, CancellationToken token = default)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

            var uri = request.GetUri();
            if (OnUriCreated != null)
            {
                uri = OnUriCreated(uri);
            }

		    var client = new HttpClient();

			var response = await client.DownloadDataTaskAsyncAsString(uri, timeout, token).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<TResponse>(response);
		}

    }
}