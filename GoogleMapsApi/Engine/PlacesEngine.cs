using System;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.Entities.Places.Response;

namespace GoogleMapsApi.Engine
{
	[Obsolete("This class is deprecated and will be removed in a future version. Please use the GoogleMaps class instead.")]
	public class PlacesEngine : MapsAPIGenericEngine<PlacesRequest, PlacesResponse>
	{
		public IAsyncResult BeginGetPlaces(PlacesRequest request, AsyncCallback asyncCallback, object state)
		{
			return BeginQueryGoogleAPI(request, asyncCallback, state);
		}

		public PlacesResponse EndGetPlaces(IAsyncResult asyncResult)
		{
			return EndQueryGoogleAPI(asyncResult);
		}

		public PlacesResponse GetPlaces(PlacesRequest request)
		{
			return QueryGoogleAPI(request, DefaultTimeout);
		}

		public Task<PlacesResponse> GetPlacesAsync(PlacesRequest request)
		{
			return QueryGoogleAPIAsync(request);
		}
	}
}