using System;
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
	/// <summary>
	/// This class is a facade to all Maps engines.
	/// </summary>
	[Obsolete("This class is deprecated and will be removed in a future version. Please use the GoogleMaps class instead.")]
	public class MapsAPIEngine : IMapsAPIEngine
	{
		public GeocodingResponse GetGeocode(GeocodingRequest request)
		{
			return new GeocodingEngine().GetGeocode(request);
		}

		public DirectionsResponse GetDirections(DirectionsRequest request)
		{
			return new DirectionsEngine().GetDirections(request);
		}

		public ElevationResponse GetElevation(ElevationRequest request)
		{
			return new ElevationEngine().GetElevation(request);
		}

		public PlacesResponse GetPlace(PlacesRequest request)
		{
			return new PlacesEngine().GetPlaces(request);
		}
	}
}