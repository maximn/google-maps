using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Entities.Elevation.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.Entities.Places.Response;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using GoogleMapsApi.Entities.PlacesText.Request;
using GoogleMapsApi.Entities.PlacesText.Response;
using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi.Entities.PlaceAutocomplete.Response;
using GoogleMapsApi.Entities.TimeZone.Request;
using GoogleMapsApi.Entities.TimeZone.Response;
using GoogleMapsApi.Entities.PlacesRadar.Request;
using GoogleMapsApi.Entities.PlacesRadar.Response;
using GoogleMapsApi.Entities.DistanceMatrix.Request;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using GoogleMapsApi.Entities.PlacesNearBy.Request;
using GoogleMapsApi.Entities.PlacesNearBy.Response;
using GoogleMapsApi.Entities.PlacesFind.Response;
using GoogleMapsApi.Entities.PlacesFind.Request;
using System;
using GoogleMapsApi.Entities.Roads.NearestRoads.Request;
using GoogleMapsApi.Entities.Roads.NearestRoads.Response;
using GoogleMapsApi.Entities.Roads.SpeedLimit.Response;
using GoogleMapsApi.Entities.Roads.SpeedLimit.Request;
using GoogleMapsApi.Entities.Roads.SnapToRoads.Request;
using GoogleMapsApi.Entities.Roads.SnapToRoads.Response;

namespace GoogleMapsApi
{
    public class GoogleMaps
	{
		/// <summary>Perform geocoding operations.</summary>
		public static IEngineFacade<GeocodingRequest, GeocodingResponse> Geocode
		{
			get
			{
				return EngineFacade<GeocodingRequest, GeocodingResponse>.Instance;
			}
		}
		/// <summary>Perform directions operations.</summary>
		public static IEngineFacade<DirectionsRequest, DirectionsResponse> Directions
		{
			get
			{
				return EngineFacade<DirectionsRequest, DirectionsResponse>.Instance;
			}
		}
		/// <summary>Perform elevation operations.</summary>
		public static IEngineFacade<ElevationRequest, ElevationResponse> Elevation
		{
			get
			{
				return EngineFacade<ElevationRequest, ElevationResponse>.Instance;
			}
		}

		/// <summary>Perform places operations.</summary>
		public static IEngineFacade<PlacesRequest, PlacesResponse> Places
		{
			get
			{
				return EngineFacade<PlacesRequest, PlacesResponse>.Instance;
			}
		}

        /// <summary>Perform places text search operations.</summary>
        public static IEngineFacade<PlacesTextRequest, PlacesTextResponse> PlacesText
        {
            get
            {
                return EngineFacade<PlacesTextRequest, PlacesTextResponse>.Instance;
            }
        }

        /// <summary>Perform places radar search operations.</summary>
        [Obsolete("Radar search is deprecated since June 30 2018", true)]
        public static IEngineFacade<PlacesRadarRequest, PlacesRadarResponse> PlacesRadar
        {
            get
            {
                return EngineFacade<PlacesRadarRequest, PlacesRadarResponse>.Instance;
            }
        }

        /// <summary>Perform places text search operations.</summary>
        public static IEngineFacade<TimeZoneRequest, TimeZoneResponse> TimeZone
        {
            get
            {
                return EngineFacade<TimeZoneRequest, TimeZoneResponse>.Instance;
            }
        }

        /// <summary>Perform places details  operations.</summary>
        public static IEngineFacade<PlacesDetailsRequest, PlacesDetailsResponse> PlacesDetails
        {
            get
            {
                return EngineFacade<PlacesDetailsRequest, PlacesDetailsResponse>.Instance;
            }
        }

        /// <summary>Perform place autocomplete operations.</summary>
        public static IEngineFacade<PlaceAutocompleteRequest, PlaceAutocompleteResponse> PlaceAutocomplete
        {
            get
            {
                return EngineFacade<PlaceAutocompleteRequest, PlaceAutocompleteResponse>.Instance;
            }
        }

        /// <summary>Perform near by places operations.</summary>
        public static IEngineFacade<PlacesNearByRequest, PlacesNearByResponse> PlacesNearBy
        {
            get
            {
                return EngineFacade<PlacesNearByRequest, PlacesNearByResponse>.Instance;
            }
        }

        /// <summary>Perform Find Place searches </summary>
        public static IEngineFacade<PlacesFindRequest, PlacesFindResponse> PlacesFind
        {
            get
            {
                return EngineFacade<PlacesFindRequest, PlacesFindResponse>.Instance;
            }
        }

        /// <summary>Retrieve duration and distance values based on the recommended route between start and end points.</summary>
        public static IEngineFacade<DistanceMatrixRequest, DistanceMatrixResponse> DistanceMatrix
        {
            get
            {
                return EngineFacade<DistanceMatrixRequest, DistanceMatrixResponse>.Instance;
            }
        }

        public static IEngineFacade<NearestRoadsRequest, NearestRoadsResponse> NearestRoads
        {
            get
            {
                return EngineFacade<NearestRoadsRequest, NearestRoadsResponse>.Instance;
            }
        }

        public static IEngineFacade<SpeedLimitRequest, SpeedLimitResponse> SpeedLimit
        {
            get
            {
                return EngineFacade<SpeedLimitRequest, SpeedLimitResponse>.Instance;
            }
        }

        public static IEngineFacade<SnapToRoadsRequest, SnapToRoadsResposne> SnapToRoads
        {
            get
            {
                return EngineFacade<SnapToRoadsRequest, SnapToRoadsResposne>.Instance;
            }
        }
    }
}
