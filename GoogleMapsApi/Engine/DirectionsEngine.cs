using System;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;

namespace GoogleMapsApi.Engine
{
	[Obsolete("This class is deprecated and will be removed in a future version. Please use the GoogleMaps class instead.")]
	public class DirectionsEngine : MapsAPIGenericEngine<DirectionsRequest, DirectionsResponse>
	{
		public IAsyncResult BeginGetDirections(DirectionsRequest request, AsyncCallback asyncCallback, object state)
		{
			return BeginQueryGoogleAPI(request, asyncCallback, state);
		}

		public DirectionsResponse EndGetDirections(IAsyncResult asyncResult)
		{
			return EndQueryGoogleAPI(asyncResult);
		}

		public DirectionsResponse GetDirections(DirectionsRequest request)
		{
			return QueryGoogleAPI(request, DefaultTimeout);
		}

		public Task<DirectionsResponse> GetDirectionsAsync(DirectionsRequest request)
		{
			return QueryGoogleAPIAsync(request);
		}
	}
}