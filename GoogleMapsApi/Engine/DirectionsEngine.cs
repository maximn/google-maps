using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;

namespace GoogleMapsApi.Engine
{
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
			return QueryGoogleAPI(request);
		}
		
		public Task<DirectionsResponse> GetDirectionsAsync(DirectionsRequest request)
		{
			return QueryGoogleAPIAsync(request);
		}
	}
}