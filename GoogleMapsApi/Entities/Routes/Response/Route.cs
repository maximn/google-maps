using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Routes.Response
{
    /// <summary>A computed route — usually one per response, more when alternatives are requested.</summary>
    public sealed class Route
    {
        /// <summary>Labels describing this route (e.g. <c>DEFAULT_ROUTE</c>, <c>FUEL_EFFICIENT</c>).</summary>
        [JsonPropertyName("routeLabels")]
        public List<RouteLabel>? RouteLabels { get; set; }

        /// <summary>Legs of the route (one per origin/intermediate/destination pair).</summary>
        [JsonPropertyName("legs")]
        public List<RouteLeg>? Legs { get; set; }

        /// <summary>Total distance covered by the route in meters.</summary>
        [JsonPropertyName("distanceMeters")]
        public int? DistanceMeters { get; set; }

        /// <summary>
        /// Total travel time including traffic. Returned as a protobuf-duration string
        /// (e.g. <c>"123s"</c>); see <see cref="DurationSeconds"/> for a parsed numeric value.
        /// </summary>
        [JsonPropertyName("duration")]
        public string? Duration { get; set; }

        /// <summary><see cref="Duration"/> parsed to a number of seconds.</summary>
        [JsonIgnore]
        public double? DurationSeconds => DurationParser.ToSeconds(Duration);

        /// <summary>
        /// Travel time ignoring current traffic. Also a protobuf-duration string;
        /// see <see cref="StaticDurationSeconds"/>.
        /// </summary>
        [JsonPropertyName("staticDuration")]
        public string? StaticDuration { get; set; }

        /// <summary><see cref="StaticDuration"/> parsed to a number of seconds.</summary>
        [JsonIgnore]
        public double? StaticDurationSeconds => DurationParser.ToSeconds(StaticDuration);

        /// <summary>Encoded polyline of the route geometry.</summary>
        [JsonPropertyName("polyline")]
        public Polyline? Polyline { get; set; }

        /// <summary>Free-text descriptions of conditions affecting the route.</summary>
        [JsonPropertyName("warnings")]
        public List<string>? Warnings { get; set; }

        /// <summary>Bounding viewport that contains the route.</summary>
        [JsonPropertyName("viewport")]
        public Viewport? Viewport { get; set; }

        /// <summary>Travel-advisory metadata (toll info, fuel consumption, speed-reading intervals).</summary>
        [JsonPropertyName("travelAdvisory")]
        public RouteTravelAdvisory? TravelAdvisory { get; set; }

        /// <summary>
        /// Optimized intermediate-waypoint order when
        /// <see cref="Request.RoutesRequest.OptimizeWaypointOrder"/> is true. The values are
        /// zero-based indices into the request's <c>Intermediates</c> array.
        /// </summary>
        [JsonPropertyName("optimizedIntermediateWaypointIndex")]
        public List<int>? OptimizedIntermediateWaypointIndex { get; set; }

        /// <summary>Token usable with the Navigation SDK to start turn-by-turn for this route.</summary>
        [JsonPropertyName("routeToken")]
        public string? RouteToken { get; set; }
    }

    /// <summary>Labels describing a returned route's category.</summary>
    public enum RouteLabel
    {
        /// <summary>Unspecified.</summary>
        [EnumMember(Value = "ROUTE_LABEL_UNSPECIFIED")] Unspecified,

        /// <summary>The default best route.</summary>
        [EnumMember(Value = "DEFAULT_ROUTE")] DefaultRoute,

        /// <summary>An alternative to the default route.</summary>
        [EnumMember(Value = "DEFAULT_ROUTE_ALTERNATE")] DefaultRouteAlternate,

        /// <summary>The fuel-efficient route reference (when requested).</summary>
        [EnumMember(Value = "FUEL_EFFICIENT")] FuelEfficient,
    }
}
