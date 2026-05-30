using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Routes.Request;

namespace GoogleMapsApi.Entities.Routes.Response
{
    /// <summary>Response from the Google Routes API <c>computeRoutes</c> endpoint.</summary>
    /// <remarks>
    /// Which top-level fields are populated depends on what was requested via
    /// <see cref="RoutesRequest.FieldMask"/>. Fields outside the mask are returned as null.
    /// </remarks>
    public sealed class RoutesResponse : IResponseFor<RoutesRequest>
    {
        /// <summary>
        /// Computed routes. If <see cref="RoutesRequest.ComputeAlternativeRoutes"/> was true,
        /// the API may return up to two additional alternatives.
        /// </summary>
        [JsonPropertyName("routes")]
        public List<Route>? Routes { get; set; }

        /// <summary>
        /// Populated when the request asked for traffic-aware routing but the API fell back to
        /// a different mode (e.g. live traffic unavailable).
        /// </summary>
        [JsonPropertyName("fallbackInfo")]
        public FallbackInfo? FallbackInfo { get; set; }

        /// <summary>Diagnostic information about waypoint geocoding.</summary>
        [JsonPropertyName("geocodingResults")]
        public GeocodingResults? GeocodingResults { get; set; }
    }
}
