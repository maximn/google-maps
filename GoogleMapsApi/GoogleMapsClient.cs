using GoogleMapsApi.Engine;
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
using System;
using System.Net.Http;

namespace GoogleMapsApi
{
    /// <summary>
    /// Instance-based Google Maps client driven by an injected <see cref="HttpClient"/>.
    /// Prefer this over the static <see cref="GoogleMaps"/> facade in modern .NET projects —
    /// it is friendly to <c>IHttpClientFactory</c>, supports per-instance event handlers, and
    /// avoids the testability problems of static state.
    /// </summary>
    /// <remarks>
    /// The static <see cref="GoogleMaps"/> facade continues to work unchanged for backward compatibility.
    /// </remarks>
    public sealed class GoogleMapsClient : IGoogleMapsClient
    {
        /// <summary>
        /// Creates a client that uses the supplied <see cref="HttpClient"/> with default options
        /// (no ambient API key — callers must set <c>ApiKey</c> on each request).
        /// </summary>
        public GoogleMapsClient(HttpClient httpClient)
            : this(httpClient, new GoogleMapsClientOptions())
        {
        }

        /// <summary>
        /// Creates a client that uses the supplied <see cref="HttpClient"/> and options.
        /// </summary>
        /// <param name="httpClient">The HTTP client used for every API call. Typically obtained from <c>IHttpClientFactory</c>.</param>
        /// <param name="options">Ambient options applied to every request (e.g. default API key).</param>
        public GoogleMapsClient(HttpClient httpClient, GoogleMapsClientOptions options)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (options == null) throw new ArgumentNullException(nameof(options));

            Geocode = new HttpClientEngineFacade<GeocodingRequest, GeocodingResponse>(httpClient, options);
            Directions = new HttpClientEngineFacade<DirectionsRequest, DirectionsResponse>(httpClient, options);
            Elevation = new HttpClientEngineFacade<ElevationRequest, ElevationResponse>(httpClient, options);
            Places = new HttpClientEngineFacade<PlacesRequest, PlacesResponse>(httpClient, options);
            PlacesText = new HttpClientEngineFacade<PlacesTextRequest, PlacesTextResponse>(httpClient, options);
            TimeZone = new HttpClientEngineFacade<TimeZoneRequest, TimeZoneResponse>(httpClient, options);
            PlacesDetails = new HttpClientEngineFacade<PlacesDetailsRequest, PlacesDetailsResponse>(httpClient, options);
            PlaceAutocomplete = new HttpClientEngineFacade<PlaceAutocompleteRequest, PlaceAutocompleteResponse>(httpClient, options);
            PlacesNearBy = new HttpClientEngineFacade<PlacesNearByRequest, PlacesNearByResponse>(httpClient, options);
            PlacesFind = new HttpClientEngineFacade<PlacesFindRequest, PlacesFindResponse>(httpClient, options);
            DistanceMatrix = new HttpClientEngineFacade<DistanceMatrixRequest, DistanceMatrixResponse>(httpClient, options);
            AddressValidation = new HttpClientEngineFacade<AddressValidationRequest, AddressValidationResponse>(httpClient, options);
            Routes = new HttpClientEngineFacade<RoutesRequest, RoutesResponse>(httpClient, options);
        }

        /// <inheritdoc/>
        public IEngineFacade<GeocodingRequest, GeocodingResponse> Geocode { get; }

        /// <inheritdoc/>
        public IEngineFacade<DirectionsRequest, DirectionsResponse> Directions { get; }

        /// <inheritdoc/>
        public IEngineFacade<ElevationRequest, ElevationResponse> Elevation { get; }

        /// <inheritdoc/>
        public IEngineFacade<PlacesRequest, PlacesResponse> Places { get; }

        /// <inheritdoc/>
        public IEngineFacade<PlacesTextRequest, PlacesTextResponse> PlacesText { get; }

        /// <inheritdoc/>
        public IEngineFacade<TimeZoneRequest, TimeZoneResponse> TimeZone { get; }

        /// <inheritdoc/>
        public IEngineFacade<PlacesDetailsRequest, PlacesDetailsResponse> PlacesDetails { get; }

        /// <inheritdoc/>
        public IEngineFacade<PlaceAutocompleteRequest, PlaceAutocompleteResponse> PlaceAutocomplete { get; }

        /// <inheritdoc/>
        public IEngineFacade<PlacesNearByRequest, PlacesNearByResponse> PlacesNearBy { get; }

        /// <inheritdoc/>
        public IEngineFacade<PlacesFindRequest, PlacesFindResponse> PlacesFind { get; }

        /// <inheritdoc/>
        public IEngineFacade<DistanceMatrixRequest, DistanceMatrixResponse> DistanceMatrix { get; }

        /// <inheritdoc/>
        public IEngineFacade<AddressValidationRequest, AddressValidationResponse> AddressValidation { get; }

        /// <inheritdoc/>
        public IEngineFacade<RoutesRequest, RoutesResponse> Routes { get; }
    }
}
