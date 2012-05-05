using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Entities.Elevation.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.Entities.Places.Response;

namespace GoogleMapsApi.Engine
{
	public interface IMapsAPIEngine
	{
		GeocodingResponse GetGeocode(GeocodingRequest request);
		DirectionsResponse GetDirections(DirectionsRequest request);
		ElevationResponse GetElevation(ElevationRequest request);
		PlacesResponse GetPlace(PlacesRequest request);
	}
}