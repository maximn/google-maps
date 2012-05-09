using System;
using System.Linq;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;

namespace GoogleMapsApi.Engine
{
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
			return QueryGoogleAPI(request);
		}

		public Task<GeocodingResponse> GetGeocodeAsync(GeocodingRequest request)
		{
			return QueryGoogleAPIAsync(request);
		}
	}
}