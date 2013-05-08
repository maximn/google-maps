using System;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Entities.Elevation.Response;

namespace GoogleMapsApi.Engine
{
	[Obsolete("This class is deprecated and will be removed in a future version. Please use the GoogleMaps class instead.")]
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
			return QueryGoogleAPI(request, DefaultTimeout);
		}

		public Task<ElevationResponse> GetElevationAsync(ElevationRequest request)
		{
			return QueryGoogleAPIAsync(request);
		}
	}
}