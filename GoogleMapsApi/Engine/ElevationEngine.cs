using System;
using System.Linq;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Entities.Elevation.Response;

namespace GoogleMapsApi.Engine
{
	public class ElevationEngine : MapsAPIGenericEngine<ElevationRequest, ElevationResponse>
	{
		public IAsyncResult BeginGetElevation(ElevationRequest request, AsyncCallback asyncCallback, object state)
		{
			return BeginQueryGoogleAPI(request, asyncCallback, state);
		}

		public ElevationResponse EndGetElevation(IAsyncResult asyncResult)
		{
			return EndQueryGoogleAPI(asyncResult);
		}

		public ElevationResponse GetElevation(ElevationRequest request)
		{
			return QueryGoogleAPI(request);
		}
		
		public Task<ElevationResponse> GetElevationAsync(ElevationRequest request)
		{
			return QueryGoogleAPIAsync(request);
		}
	}
}