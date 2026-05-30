using GoogleMapsApi.Entities.AddressValidation.Request;
using GoogleMapsApi.Entities.AddressValidation.Response;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.DistanceMatrix.Request;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Entities.Elevation.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi.Entities.PlaceAutocomplete.Response;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.Entities.Places.Response;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using GoogleMapsApi.Entities.PlacesFind.Request;
using GoogleMapsApi.Entities.PlacesFind.Response;
using GoogleMapsApi.Entities.PlacesNearBy.Request;
using GoogleMapsApi.Entities.PlacesNearBy.Response;
using GoogleMapsApi.Entities.PlacesText.Request;
using GoogleMapsApi.Entities.PlacesText.Response;
using GoogleMapsApi.Entities.Routes.Request;
using GoogleMapsApi.Entities.Routes.Response;
using GoogleMapsApi.Entities.TimeZone.Request;
using GoogleMapsApi.Entities.TimeZone.Response;

namespace GoogleMapsApi
{
    /// <summary>
    /// Instance-based entry point exposing every supported Google Maps Web Service.
    /// Designed for dependency injection scenarios where an <see cref="System.Net.Http.HttpClient"/>
    /// is supplied (typically via <c>IHttpClientFactory</c>) and a single API key is shared across calls.
    /// </summary>
    public interface IGoogleMapsClient
    {
        /// <summary>Perform geocoding operations.</summary>
        IEngineFacade<GeocodingRequest, GeocodingResponse> Geocode { get; }

        /// <summary>Perform directions operations.</summary>
        IEngineFacade<DirectionsRequest, DirectionsResponse> Directions { get; }

        /// <summary>Perform elevation operations.</summary>
        IEngineFacade<ElevationRequest, ElevationResponse> Elevation { get; }

        /// <summary>Perform places operations.</summary>
        IEngineFacade<PlacesRequest, PlacesResponse> Places { get; }

        /// <summary>Perform places text search operations.</summary>
        IEngineFacade<PlacesTextRequest, PlacesTextResponse> PlacesText { get; }

        /// <summary>Retrieve time zone data for a coordinate.</summary>
        IEngineFacade<TimeZoneRequest, TimeZoneResponse> TimeZone { get; }

        /// <summary>Perform places details operations.</summary>
        IEngineFacade<PlacesDetailsRequest, PlacesDetailsResponse> PlacesDetails { get; }

        /// <summary>Perform place autocomplete operations.</summary>
        IEngineFacade<PlaceAutocompleteRequest, PlaceAutocompleteResponse> PlaceAutocomplete { get; }

        /// <summary>Perform near-by places operations.</summary>
        IEngineFacade<PlacesNearByRequest, PlacesNearByResponse> PlacesNearBy { get; }

        /// <summary>Perform find-place searches.</summary>
        IEngineFacade<PlacesFindRequest, PlacesFindResponse> PlacesFind { get; }

        /// <summary>Retrieve duration and distance values based on the recommended route between start and end points.</summary>
        IEngineFacade<DistanceMatrixRequest, DistanceMatrixResponse> DistanceMatrix { get; }

        /// <summary>Validate a postal address (Address Validation API). Supports USPS CASS for US/PR addresses.</summary>
        IEngineFacade<AddressValidationRequest, AddressValidationResponse> AddressValidation { get; }

        /// <summary>
        /// Compute routes via the Routes API — the modern replacement for the Directions API.
        /// </summary>
        IEngineFacade<RoutesRequest, RoutesResponse> Routes { get; }
    }
}
