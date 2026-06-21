using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.AddressValidation.Request;
using GoogleMapsApi.Entities.AddressValidation.Response;
using GoogleMapsApi.Entities.AerialView.Request;
using GoogleMapsApi.Entities.AerialView.Response;
using GoogleMapsApi.Entities.AirQuality.Request;
using GoogleMapsApi.Entities.AirQuality.Response;
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
using GoogleMapsApi.Entities.Pollen.Request;
using GoogleMapsApi.Entities.Pollen.Response;
using GoogleMapsApi.Entities.Solar.Request;
using GoogleMapsApi.Entities.Solar.Response;
using GoogleMapsApi.Entities.TimeZone.Request;
using GoogleMapsApi.Entities.TimeZone.Response;
using System;
using System.Net.Http;

namespace GoogleMapsApi
{
    /// <summary>
    /// Instance-based Google Maps client driven by an injected <see cref="HttpClient"/>.
    /// It is friendly to <c>IHttpClientFactory</c>, supports per-instance event handlers, and
    /// avoids the testability problems of static state.
    /// </summary>
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
            TimeZone = new HttpClientEngineFacade<TimeZoneRequest, TimeZoneResponse>(httpClient, options);
            DistanceMatrix = new HttpClientEngineFacade<DistanceMatrixRequest, DistanceMatrixResponse>(httpClient, options);
            AddressValidation = new HttpClientEngineFacade<AddressValidationRequest, AddressValidationResponse>(httpClient, options);
            Routes = new HttpClientEngineFacade<RoutesRequest, RoutesResponse>(httpClient, options);
            SnapToRoads = new HttpClientEngineFacade<SnapToRoadsRequest, SnapToRoadsResponse>(httpClient, options);
            NearestRoads = new HttpClientEngineFacade<NearestRoadsRequest, NearestRoadsResponse>(httpClient, options);
            SpeedLimits = new HttpClientEngineFacade<SpeedLimitsRequest, SpeedLimitsResponse>(httpClient, options);
            PlacesSearchText = new HttpClientEngineFacade<SearchTextRequest, SearchTextResponse>(httpClient, options);
            PlacesSearchNearby = new HttpClientEngineFacade<SearchNearbyRequest, SearchNearbyResponse>(httpClient, options);
            PlaceDetailsNew = new HttpClientEngineFacade<PlaceDetailsRequest, Place>(httpClient, options);
            PlacesAutocompleteNew = new HttpClientEngineFacade<AutocompleteRequest, AutocompleteResponse>(httpClient, options);
            PlacePhoto = new HttpClientEngineFacade<PlacePhotoRequest, PlacePhotoResponse>(httpClient, options);
            SolarBuildingInsights = new HttpClientEngineFacade<BuildingInsightsRequest, BuildingInsightsResponse>(httpClient, options);
            SolarDataLayers = new HttpClientEngineFacade<DataLayersRequest, DataLayersResponse>(httpClient, options);
            SolarGeoTiff = new HttpClientEngineFacade<GeoTiffRequest, GeoTiffResponse>(httpClient, options);
            AirQualityCurrentConditions = new HttpClientEngineFacade<CurrentConditionsRequest, CurrentConditionsResponse>(httpClient, options);
            AirQualityForecast = new HttpClientEngineFacade<ForecastRequest, ForecastResponse>(httpClient, options);
            AirQualityHistory = new HttpClientEngineFacade<HistoryRequest, HistoryResponse>(httpClient, options);
            AirQualityHeatmapTile = new HttpClientEngineFacade<HeatmapTileRequest, HeatmapTileResponse>(httpClient, options);
            PollenForecast = new HttpClientEngineFacade<PollenForecastRequest, PollenForecastResponse>(httpClient, options);
            PollenHeatmapTile = new HttpClientEngineFacade<PollenHeatmapTileRequest, PollenHeatmapTileResponse>(httpClient, options);
            AerialView = new AerialViewApi(httpClient, options);
        }

        /// <inheritdoc/>
        public IEngineFacade<GeocodingRequest, GeocodingResponse> Geocode { get; }

        /// <inheritdoc/>
        public IEngineFacade<DirectionsRequest, DirectionsResponse> Directions { get; }

        /// <inheritdoc/>
        public IEngineFacade<ElevationRequest, ElevationResponse> Elevation { get; }

        /// <inheritdoc/>
        public IEngineFacade<TimeZoneRequest, TimeZoneResponse> TimeZone { get; }

        /// <inheritdoc/>
        public IEngineFacade<DistanceMatrixRequest, DistanceMatrixResponse> DistanceMatrix { get; }

        /// <inheritdoc/>
        public IEngineFacade<AddressValidationRequest, AddressValidationResponse> AddressValidation { get; }

        /// <inheritdoc/>
        public IEngineFacade<RoutesRequest, RoutesResponse> Routes { get; }

        /// <inheritdoc/>
        public IEngineFacade<SnapToRoadsRequest, SnapToRoadsResponse> SnapToRoads { get; }

        /// <inheritdoc/>
        public IEngineFacade<NearestRoadsRequest, NearestRoadsResponse> NearestRoads { get; }

        /// <inheritdoc/>
        public IEngineFacade<SpeedLimitsRequest, SpeedLimitsResponse> SpeedLimits { get; }

        /// <inheritdoc/>
        public IEngineFacade<SearchTextRequest, SearchTextResponse> PlacesSearchText { get; }

        /// <inheritdoc/>
        public IEngineFacade<SearchNearbyRequest, SearchNearbyResponse> PlacesSearchNearby { get; }

        /// <inheritdoc/>
        public IEngineFacade<PlaceDetailsRequest, Place> PlaceDetailsNew { get; }

        /// <inheritdoc/>
        public IEngineFacade<AutocompleteRequest, AutocompleteResponse> PlacesAutocompleteNew { get; }

        /// <inheritdoc/>
        public IEngineFacade<PlacePhotoRequest, PlacePhotoResponse> PlacePhoto { get; }

        /// <inheritdoc/>
        public IEngineFacade<BuildingInsightsRequest, BuildingInsightsResponse> SolarBuildingInsights { get; }

        /// <inheritdoc/>
        public IEngineFacade<DataLayersRequest, DataLayersResponse> SolarDataLayers { get; }

        /// <inheritdoc/>
        public IEngineFacade<GeoTiffRequest, GeoTiffResponse> SolarGeoTiff { get; }

        /// <inheritdoc/>
        public IEngineFacade<CurrentConditionsRequest, CurrentConditionsResponse> AirQualityCurrentConditions { get; }

        /// <inheritdoc/>
        public IEngineFacade<ForecastRequest, ForecastResponse> AirQualityForecast { get; }

        /// <inheritdoc/>
        public IEngineFacade<HistoryRequest, HistoryResponse> AirQualityHistory { get; }

        /// <inheritdoc/>
        public IEngineFacade<HeatmapTileRequest, HeatmapTileResponse> AirQualityHeatmapTile { get; }

        /// <inheritdoc/>
        public IEngineFacade<PollenForecastRequest, PollenForecastResponse> PollenForecast { get; }

        /// <inheritdoc/>
        public IEngineFacade<PollenHeatmapTileRequest, PollenHeatmapTileResponse> PollenHeatmapTile { get; }

        /// <inheritdoc/>
        public IAerialViewApi AerialView { get; }
    }
}
