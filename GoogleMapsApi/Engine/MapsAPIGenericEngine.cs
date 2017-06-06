using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;

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

		internal static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(100);

		private const string AuthenticationFailedMessage = "The request to Google API failed with HTTP error '(403) Forbidden', which usually indicates that the provided client ID or signing key is invalid or expired.";

		protected internal static async Task<TResponse> QueryGoogleAPI(TRequest request, TimeSpan timeout)
		{
			if (request == null)
				throw new ArgumentNullException("request");

			var uri = request.GetUri();
			if (OnUriCreated != null)
			{
			    uri = OnUriCreated(uri);
			}

			using (var httpClient = new HttpClient())
			{
			    httpClient.Timeout = timeout;
                var result = await httpClient.GetAsync(uri);

			    if (result.IsSuccessStatusCode)
			    {
			        var data = await result.Content.ReadAsByteArrayAsync();
			        OnRawResponseRecivied?.Invoke(data);
			        return Deserialize(data); //Http 200: Everything is good!
			    }

			    if (result.StatusCode == HttpStatusCode.Forbidden)
			    {
			        throw new UnauthorizedAccessException(AuthenticationFailedMessage);
                }
                if (result.StatusCode == HttpStatusCode.RequestTimeout)
			    {
			        throw new TimeoutException($"The request has exceeded the timeout limit of {timeout} and has been aborted.");
                }

			    throw new HttpRequestException($"Received http code: {result.StatusCode} but was expecting a success");
            }		    
		}

		private static TResponse Deserialize(byte[] serializedObject)
		{
			var serializer = new DataContractJsonSerializer(typeof(TResponse));
			var stream = new MemoryStream(serializedObject, false);
			return (TResponse)serializer.ReadObject(stream);
		}
	}
}