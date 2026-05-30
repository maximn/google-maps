using GoogleMapsApi.Entities.AddressValidation.Request;
using GoogleMapsApi.Entities.AddressValidation.Response;
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
using GoogleMapsApi.Entities.Routes.Request;
using GoogleMapsApi.Entities.Routes.Response;
using GoogleMapsApi.Entities.PlacesNew.Request;
using GoogleMapsApi.Entities.PlacesNew.Response;
using System;

#pragma warning disable CS0618 // legacy Places facade members are intentionally obsolete; their bodies still resolve the engine.

namespace GoogleMapsApi
{
    /// <summary>
    /// Static facade exposing strongly-typed accessors to every supported Google Maps Web Service.
    /// Each property returns a singleton engine that handles request execution for a specific API.
    /// </summary>
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
		[Obsolete("The legacy Places API is frozen. Use the Places API (New) — e.g. GoogleMaps.PlacesSearchText and the GoogleMapsApi.Entities.PlacesNew namespace.")]
		public static IEngineFacade<PlacesRequest, PlacesResponse> Places
		{
			get
			{
				return EngineFacade<PlacesRequest, PlacesResponse>.Instance;
			}
		}

        /// <summary>Perform places text search operations.</summary>
        [Obsolete("The legacy Places API is frozen. Use the Places API (New) — e.g. GoogleMaps.PlacesSearchText and the GoogleMapsApi.Entities.PlacesNew namespace.")]
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
        [Obsolete("The legacy Places API is frozen. Use the Places API (New) — e.g. GoogleMaps.PlacesSearchText and the GoogleMapsApi.Entities.PlacesNew namespace.")]
        public static IEngineFacade<PlacesDetailsRequest, PlacesDetailsResponse> PlacesDetails
        {
            get
            {
                return EngineFacade<PlacesDetailsRequest, PlacesDetailsResponse>.Instance;
            }
        }

        /// <summary>Perform place autocomplete operations.</summary>
        [Obsolete("The legacy Places API is frozen. Use the Places API (New) — e.g. GoogleMaps.PlacesSearchText and the GoogleMapsApi.Entities.PlacesNew namespace.")]
        public static IEngineFacade<PlaceAutocompleteRequest, PlaceAutocompleteResponse> PlaceAutocomplete
        {
            get
            {
                return EngineFacade<PlaceAutocompleteRequest, PlaceAutocompleteResponse>.Instance;
            }
        }

        /// <summary>Perform near by places operations.</summary>
        [Obsolete("The legacy Places API is frozen. Use the Places API (New) — e.g. GoogleMaps.PlacesSearchText and the GoogleMapsApi.Entities.PlacesNew namespace.")]
        public static IEngineFacade<PlacesNearByRequest, PlacesNearByResponse> PlacesNearBy
        {
            get
            {
                return EngineFacade<PlacesNearByRequest, PlacesNearByResponse>.Instance;
            }
        }

        /// <summary>Perform Find Place searches </summary>
        [Obsolete("The legacy Places API is frozen. Use the Places API (New) — e.g. GoogleMaps.PlacesSearchText and the GoogleMapsApi.Entities.PlacesNew namespace.")]
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

        /// <summary>Validate a postal address (Address Validation API). Supports USPS CASS for US/PR addresses.</summary>
        public static IEngineFacade<AddressValidationRequest, AddressValidationResponse> AddressValidation
        {
            get
            {
                return EngineFacade<AddressValidationRequest, AddressValidationResponse>.Instance;
            }
        }

        /// <summary>
        /// Compute routes via the Routes API — the modern replacement for the Directions API.
        /// Supports real-time traffic, eco-routing, toll calculation, two-wheeled vehicles, and
        /// route alternatives.
        /// </summary>
        public static IEngineFacade<RoutesRequest, RoutesResponse> Routes
        {
            get
            {
                return EngineFacade<RoutesRequest, RoutesResponse>.Instance;
            }
        }

        /// <summary>Search for places by free-text query via the Places API (New).</summary>
        public static IEngineFacade<SearchTextRequest, SearchTextResponse> PlacesSearchText
        {
            get
            {
                return EngineFacade<SearchTextRequest, SearchTextResponse>.Instance;
            }
        }

        /// <summary>Search for places near a location via the Places API (New).</summary>
        public static IEngineFacade<SearchNearbyRequest, SearchNearbyResponse> PlacesSearchNearby
        {
            get
            {
                return EngineFacade<SearchNearbyRequest, SearchNearbyResponse>.Instance;
            }
        }

        /// <summary>Fetch rich details about a single place via the Places API (New).</summary>
        public static IEngineFacade<PlaceDetailsRequest, Place> PlaceDetailsNew
        {
            get
            {
                return EngineFacade<PlaceDetailsRequest, Place>.Instance;
            }
        }

        /// <summary>Get place/query predictions for typed input via the Places API (New).</summary>
        public static IEngineFacade<AutocompleteRequest, AutocompleteResponse> PlacesAutocompleteNew
        {
            get
            {
                return EngineFacade<AutocompleteRequest, AutocompleteResponse>.Instance;
            }
        }

        /// <summary>Resolve a place photo reference to an image URI via the Places API (New).</summary>
        public static IEngineFacade<PlacePhotoRequest, PlacePhotoResponse> PlacePhoto
        {
            get
            {
                return EngineFacade<PlacePhotoRequest, PlacePhotoResponse>.Instance;
            }
        }

    }
}

#pragma warning restore CS0618
