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
using GoogleMapsApi.Entities.Roads.Request;
using GoogleMapsApi.Entities.Roads.Response;
using GoogleMapsApi.Entities.Routes.Request;
using GoogleMapsApi.Entities.Routes.Response;
using GoogleMapsApi.Entities.PlacesNew.Request;
using GoogleMapsApi.Entities.PlacesNew.Response;
using GoogleMapsApi.Entities.TimeZone.Request;
using GoogleMapsApi.Entities.TimeZone.Response;
using System;

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

        /// <summary>Retrieve time zone data for a coordinate.</summary>
        IEngineFacade<TimeZoneRequest, TimeZoneResponse> TimeZone { get; }

        /// <summary>Retrieve duration and distance values based on the recommended route between start and end points.</summary>
        IEngineFacade<DistanceMatrixRequest, DistanceMatrixResponse> DistanceMatrix { get; }

        /// <summary>Validate a postal address (Address Validation API). Supports USPS CASS for US/PR addresses.</summary>
        IEngineFacade<AddressValidationRequest, AddressValidationResponse> AddressValidation { get; }

        /// <summary>
        /// Compute routes via the Routes API — the modern replacement for the Directions API.
        /// </summary>
        IEngineFacade<RoutesRequest, RoutesResponse> Routes { get; }

        /// <summary>Snap GPS points to the most likely road segments traveled (Roads API).</summary>
        IEngineFacade<SnapToRoadsRequest, SnapToRoadsResponse> SnapToRoads { get; }

        /// <summary>Find the nearest road segment for each of a set of points (Roads API).</summary>
        IEngineFacade<NearestRoadsRequest, NearestRoadsResponse> NearestRoads { get; }

        /// <summary>
        /// Look up posted speed limits along a path or for place IDs (Roads API).
        /// Requires a Google Asset Tracking license; other keys receive an HTTP 403.
        /// </summary>
        IEngineFacade<SpeedLimitsRequest, SpeedLimitsResponse> SpeedLimits { get; }

        /// <summary>Search for places by free-text query via the Places API (New).</summary>
        IEngineFacade<SearchTextRequest, SearchTextResponse> PlacesSearchText { get; }

        /// <summary>Search for places near a location via the Places API (New).</summary>
        IEngineFacade<SearchNearbyRequest, SearchNearbyResponse> PlacesSearchNearby { get; }

        /// <summary>Fetch rich details about a single place via the Places API (New).</summary>
        IEngineFacade<PlaceDetailsRequest, Place> PlaceDetailsNew { get; }

        /// <summary>Get place/query predictions for typed input via the Places API (New).</summary>
        IEngineFacade<AutocompleteRequest, AutocompleteResponse> PlacesAutocompleteNew { get; }

        /// <summary>Resolve a place photo reference to an image URI via the Places API (New).</summary>
        IEngineFacade<PlacePhotoRequest, PlacePhotoResponse> PlacePhoto { get; }

        /// <summary>Render and look up cinematic flyover videos via the Aerial View API.</summary>
        IAerialViewApi AerialView { get; }
    }
}
