using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Engine
{
	public abstract class MapsAPIGenericEngine<TRequest, TResponse>
		where TRequest : MapsBaseRequest, new()
		where TResponse : class
	{
		/// <summary>
		/// Determines the maximum number of concurrent HTTP connections to open to this engine's host address.
		/// </summary>
		/// <remarks>
		/// This value is determined by the ServicePointManager and is shared across other engines that use the same host address.
		/// </remarks>
		public static int HttpConnectionLimit
		{
			get
			{
				return HttpServicePoint.ConnectionLimit;
			}
			set
			{
				HttpServicePoint.ConnectionLimit = value;
			}
		}
		/// <summary>
		/// Determines the maximum number of concurrent HTTPS connections to open to this engine's host address.
		/// </summary>
		/// <remarks>
		/// This value is determined by the ServicePointManager and is shared across other engines that use the same host address.
		/// </remarks>
		public static int HttpsConnectionLimit
		{
			get
			{
				return HttpsServicePoint.ConnectionLimit;
			}
			set
			{
				HttpsServicePoint.ConnectionLimit = value;
			}
		}

		private static ServicePoint HttpServicePoint { get; set; }
		private static ServicePoint HttpsServicePoint { get; set; }

		static MapsAPIGenericEngine()
		{
			var baseUrl = new TRequest().BaseUrl;
			HttpServicePoint = ServicePointManager.FindServicePoint(new Uri("http://" + baseUrl));
			HttpsServicePoint = ServicePointManager.FindServicePoint(new Uri("https://" + baseUrl));
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
					completionSource.SetException(t.Exception.InnerException);
				else if (t.IsCanceled)
					completionSource.SetCanceled();
				else
					completionSource.SetResult(t.Result);

				asyncCallback(completionSource.Task);
			}, TaskContinuationOptions.ExecuteSynchronously);

			return completionSource.Task;
		}

		protected TResponse EndQueryGoogleAPI(IAsyncResult asyncResult)
		{
			return ((Task<TResponse>)asyncResult).Result;
		}

		protected TResponse QueryGoogleAPI(TRequest request)
		{
			return QueryGoogleAPIAsync(request).Result;
		}

		protected Task<TResponse> QueryGoogleAPIAsync(TRequest request)
		{
			var uri = request.GetUri();

			return new WebClient().DownloadDataTaskAsync(uri)
				.ContinueWith<TResponse>(DownloadDataComplete, TaskContinuationOptions.ExecuteSynchronously);
		}

		private TResponse DownloadDataComplete(Task<byte[]> t)
		{
			if (t.IsFaulted)
			{
				var webException = t.Exception.InnerException as WebException;
				if (webException != null && webException.Status == WebExceptionStatus.ProtocolError && ((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.Forbidden)
					throw new AuthenticationException("The request to Google API failed with HTTP error '(403) Forbidden', which usually indicates that provided client ID or signing key are invalid or expired.", t.Exception.InnerException);
			}

			return Deserialize(t.Result);
		}

		private TResponse Deserialize(byte[] serializedObject)
		{
			var serializer = new DataContractJsonSerializer(typeof(TResponse));
			var stream = new MemoryStream(serializedObject, false);
			return (TResponse)serializer.ReadObject(stream);
		}
	}
}