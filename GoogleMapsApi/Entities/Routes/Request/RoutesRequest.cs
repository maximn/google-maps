using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Routes.Request
{
    /// <summary>
    /// Request for the Google Routes API
    /// (<c>POST https://routes.googleapis.com/directions/v2:computeRoutes</c>).
    /// Modern replacement for the legacy Directions API: real-time traffic, eco-routing,
    /// toll calculation, two-wheeled vehicle support, and route alternatives.
    /// </summary>
    /// <remarks>
    /// The Routes API <strong>requires a field mask</strong>. <see cref="FieldMask"/> is
    /// pre-populated with a reasonable default that mirrors what the legacy Directions API
    /// returned; tighten it to reduce response size and cost. See
    /// <see href="https://developers.google.com/maps/documentation/routes/choose_fields"/>.
    /// </remarks>
    public sealed class RoutesRequest : MapsBaseRequest
    {
        /// <summary>Default <see cref="FieldMask"/> — sized to roughly match what Directions returned.</summary>
        public const string DefaultFieldMask =
            "routes.duration,routes.distanceMeters,routes.polyline.encodedPolyline," +
            "routes.legs.steps,routes.legs.distanceMeters,routes.legs.duration,routes.warnings";

        /// <summary>
        /// Comma-separated response field mask. <strong>Required by the Routes API</strong> —
        /// requests with an empty mask are rejected. Passed in the URL as <c>$fields</c>.
        /// </summary>
        public string FieldMask { get; set; } = DefaultFieldMask;

        /// <summary>Origin waypoint. Required.</summary>
        public Waypoint Origin { get; set; } = new Waypoint();

        /// <summary>Destination waypoint. Required.</summary>
        public Waypoint Destination { get; set; } = new Waypoint();

        /// <summary>Intermediate waypoints, in order.</summary>
        public List<Waypoint>? Intermediates { get; set; }

        /// <summary>Travel mode. Defaults to <see cref="RoutesTravelMode.Drive"/>.</summary>
        public RoutesTravelMode TravelMode { get; set; } = RoutesTravelMode.Drive;

        /// <summary>
        /// How traffic data should be applied. Only valid for <see cref="RoutesTravelMode.Drive"/>
        /// and <see cref="RoutesTravelMode.TwoWheeler"/>.
        /// </summary>
        public RoutingPreference? RoutingPreference { get; set; }

        /// <summary>Quality of the polyline returned for each route.</summary>
        public PolylineQuality? PolylineQuality { get; set; }

        /// <summary>Wire format of the polyline returned for each route.</summary>
        public PolylineEncoding? PolylineEncoding { get; set; }

        /// <summary>
        /// Desired departure time in absolute time. Mutually exclusive with <see cref="ArrivalTime"/>.
        /// Required for <see cref="RoutesTravelMode.Transit"/> unless <see cref="ArrivalTime"/> is set.
        /// </summary>
        public DateTimeOffset? DepartureTime { get; set; }

        /// <summary>
        /// Desired arrival time in absolute time. Transit only; mutually exclusive with <see cref="DepartureTime"/>.
        /// </summary>
        public DateTimeOffset? ArrivalTime { get; set; }

        /// <summary>If true, the API may return up to two additional alternative routes.</summary>
        public bool ComputeAlternativeRoutes { get; set; }

        /// <summary>Conditions affecting the route (avoidances, vehicle info, toll passes).</summary>
        public RouteModifiers? RouteModifiers { get; set; }

        /// <summary>BCP-47 language code for the response (e.g. <c>"en-US"</c>).</summary>
        public string? LanguageCode { get; set; }

        /// <summary>Two-letter CLDR region code used for region-specific routing (e.g. <c>"US"</c>).</summary>
        public string? RegionCode { get; set; }

        /// <summary>Distance units for the response.</summary>
        public Units? Units { get; set; }

        /// <summary>If true and intermediates are supplied, reorder them to minimize total cost.</summary>
        public bool OptimizeWaypointOrder { get; set; }

        /// <summary>Optional reference routes to compute alongside the default route (e.g. fuel-efficient).</summary>
        public List<ReferenceRoute>? RequestedReferenceRoutes { get; set; }

        /// <summary>Optional extra computations (tolls, fuel consumption, traffic polyline).</summary>
        public List<ExtraComputation>? ExtraComputations { get; set; }

        /// <summary>Traffic model used when traffic-aware routing is enabled.</summary>
        public TrafficModel? TrafficModel { get; set; }

        /// <summary>Transit-specific preferences. <see cref="RoutesTravelMode.Transit"/> only.</summary>
        public TransitPreferences? TransitPreferences { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException("ApiKey is required for the Routes API.");
            if (string.IsNullOrWhiteSpace(FieldMask))
                throw new InvalidOperationException("FieldMask is required for the Routes API. See RoutesRequest.DefaultFieldMask for a starting point.");

            return new Uri(
                "https://routes.googleapis.com/directions/v2:computeRoutes" +
                $"?key={Uri.EscapeDataString(ApiKey!)}" +
                $"&$fields={Uri.EscapeDataString(FieldMask)}");
        }

        /// <inheritdoc/>
        protected internal override HttpContent? GetRequestBody()
        {
            if (DepartureTime.HasValue && ArrivalTime.HasValue)
                throw new InvalidOperationException("DepartureTime and ArrivalTime are mutually exclusive.");

            var payload = new Payload
            {
                Origin = Origin,
                Destination = Destination,
                Intermediates = Intermediates is { Count: > 0 } ? Intermediates : null,
                TravelMode = TravelMode != RoutesTravelMode.Unspecified ? TravelMode : (RoutesTravelMode?)null,
                RoutingPreference = RoutingPreference,
                PolylineQuality = PolylineQuality,
                PolylineEncoding = PolylineEncoding,
                DepartureTime = DepartureTime?.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"),
                ArrivalTime = ArrivalTime?.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"),
                ComputeAlternativeRoutes = ComputeAlternativeRoutes ? true : (bool?)null,
                RouteModifiers = RouteModifiers,
                LanguageCode = string.IsNullOrWhiteSpace(LanguageCode) ? null : LanguageCode,
                RegionCode = string.IsNullOrWhiteSpace(RegionCode) ? null : RegionCode,
                Units = Units,
                OptimizeWaypointOrder = OptimizeWaypointOrder ? true : (bool?)null,
                RequestedReferenceRoutes = RequestedReferenceRoutes is { Count: > 0 } ? RequestedReferenceRoutes : null,
                ExtraComputations = ExtraComputations is { Count: > 0 } ? ExtraComputations : null,
                TrafficModel = TrafficModel,
                TransitPreferences = TransitPreferences,
            };
            var json = JsonSerializer.Serialize(payload, BodyOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static readonly JsonSerializerOptions BodyOptions = JsonSerializerConfiguration.CreateRequestBodyOptions();

        private sealed class Payload
        {
            [JsonPropertyName("origin")] public Waypoint? Origin { get; set; }
            [JsonPropertyName("destination")] public Waypoint? Destination { get; set; }
            [JsonPropertyName("intermediates")] public List<Waypoint>? Intermediates { get; set; }
            [JsonPropertyName("travelMode")] public RoutesTravelMode? TravelMode { get; set; }
            [JsonPropertyName("routingPreference")] public RoutingPreference? RoutingPreference { get; set; }
            [JsonPropertyName("polylineQuality")] public PolylineQuality? PolylineQuality { get; set; }
            [JsonPropertyName("polylineEncoding")] public PolylineEncoding? PolylineEncoding { get; set; }
            [JsonPropertyName("departureTime")] public string? DepartureTime { get; set; }
            [JsonPropertyName("arrivalTime")] public string? ArrivalTime { get; set; }
            [JsonPropertyName("computeAlternativeRoutes")] public bool? ComputeAlternativeRoutes { get; set; }
            [JsonPropertyName("routeModifiers")] public RouteModifiers? RouteModifiers { get; set; }
            [JsonPropertyName("languageCode")] public string? LanguageCode { get; set; }
            [JsonPropertyName("regionCode")] public string? RegionCode { get; set; }
            [JsonPropertyName("units")] public Units? Units { get; set; }
            [JsonPropertyName("optimizeWaypointOrder")] public bool? OptimizeWaypointOrder { get; set; }
            [JsonPropertyName("requestedReferenceRoutes")] public List<ReferenceRoute>? RequestedReferenceRoutes { get; set; }
            [JsonPropertyName("extraComputations")] public List<ExtraComputation>? ExtraComputations { get; set; }
            [JsonPropertyName("trafficModel")] public TrafficModel? TrafficModel { get; set; }
            [JsonPropertyName("transitPreferences")] public TransitPreferences? TransitPreferences { get; set; }
        }
    }
}
