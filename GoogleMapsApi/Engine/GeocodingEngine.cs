using System;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;

namespace GoogleMapsApi.Engine
{
	[Obsolete("This class is deprecated and will be removed in a future version. Please use the GoogleMaps class instead.")]
	public class GeocodingEngine : MapsAPIGenericEngine<GeocodingRequest, GeocodingResponse>
	{
		public IAsyncResult BeginGetGeocode(GeocodingRequest request, AsyncCallback asyncCallback, object state)
		{
			return BeginQueryGoogleAPI(request, asyncCallback, state);
		}

		public GeocodingResponse EndGetGeocode(IAsyncResult asyncResult)
		{
			return EndQueryGoogleAPI(asyncResult);
		}

		public GeocodingResponse GetGeocode(GeocodingRequest request)
		{
			return QueryGoogleAPI(request, DefaultTimeout);
		}

		public Task<GeocodingResponse> GetGeocodeAsync(GeocodingRequest request)
		{
			return QueryGoogleAPIAsync(request);
		}
	}
}