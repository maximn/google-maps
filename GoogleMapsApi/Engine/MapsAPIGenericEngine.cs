using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Engine
{
	public abstract class MapsAPIGenericEngine<TRequest, TResponse>
		where TRequest : MapsBaseRequest
		where TResponse : class
	{
		protected virtual string BaseUrl
		{
			get
			{
				return "maps.google.com/maps/api/";
			}
		}

		protected IAsyncResult BeginQueryGoogleAPI(TRequest request, AsyncCallback asyncCallback, object state)
		{
			// Must use TaskCompletionSource because in .NET 4.0 there's no overload of ContinueWith that accepts a state object (used in IAsyncResult).
			// Such overloads have been added in .NET 4.5, so this can be removed if/when the project is promoted to that version.
			// An example of such an added overload can be found at: http://msdn.microsoft.com/en-us/library/hh160386.aspx

			var completionSource = new TaskCompletionSource<TResponse>(state);
			QueryGoogleAPIAsync(request).ContinueWith(t =>
			{
				if (t.IsFaulted)
					completionSource.SetException(t.Exception);
				else if (t.IsCanceled)
					completionSource.SetCanceled();
				else
					completionSource.SetResult(t.Result);

				asyncCallback(completionSource.Task);
			});

			return completionSource.Task;
		}

		protected TResponse EndQueryGoogleAPI(IAsyncResult asyncResult)
		{
			return ((Task<TResponse>)asyncResult).Result;
		}

		protected TResponse QueryGoogleAPI(TRequest request)
		{
		    Uri uri;
		    var client = GetWebClient(request, out uri);
            return Deserialize(client.DownloadString(uri));
		}

		protected Task<TResponse> QueryGoogleAPIAsync(TRequest request)
		{
		    Uri uri;
		    var client = GetWebClient(request, out uri);
		    return client.DownloadStringTaskAsync(uri)
				.ContinueWith(t => Deserialize(t.Result), TaskContinuationOptions.ExecuteSynchronously);
		}

	    private WebClient GetWebClient(TRequest request, out Uri uri)
	    {
	        var client = new WebClient();
	        ConfigureUnderlyingWebClient(client, request);
	        uri = GetUri(request);
	        return client;
	    }

	    private Uri GetUri(MapsBaseRequest request)
		{
			string scheme = request.IsSSL ? "https://" : "http://";
			return new Uri(scheme + BaseUrl + "json");
		}
		
		protected abstract void ConfigureUnderlyingWebClient(WebClient wc, MapsBaseRequest request);

		private TResponse Deserialize(string serializedObject)
		{
			var serializer = new DataContractJsonSerializer(typeof(TResponse));
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(serializedObject));
			return (TResponse)serializer.ReadObject(stream);
		}
	}
}