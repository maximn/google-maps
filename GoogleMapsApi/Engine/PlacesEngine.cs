using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.Entities.Places.Response;

namespace GoogleMapsApi.Engine
{
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
			return QueryGoogleAPI(request);
		}

		public Task<PlacesResponse> GetPlacesAsync(PlacesRequest request)
		{
			return QueryGoogleAPIAsync(request);
		}
	}
}