using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Routes.Response
{
    /// <summary>
    /// Populated when the Routes API had to fall back to a different routing mode than the one
    /// requested (e.g. live traffic was unavailable).
    /// </summary>
    public sealed class FallbackInfo
    {
        /// <summary>Routing mode actually used.</summary>
        [JsonPropertyName("routingMode")]
        public FallbackRoutingMode? RoutingMode { get; set; }

        /// <summary>Why the fallback occurred.</summary>
        [JsonPropertyName("reason")]
        public FallbackReason? Reason { get; set; }
    }

    /// <summary>Actual routing mode used when a fallback occurred.</summary>
    public enum FallbackRoutingMode
    {
        /// <summary>Unspecified.</summary>
        [EnumMember(Value = "FALLBACK_ROUTING_MODE_UNSPECIFIED")] Unspecified,

        /// <summary>Live traffic was unavailable; a static route was returned.</summary>
        [EnumMember(Value = "FALLBACK_TRAFFIC_UNAWARE")] TrafficUnaware,

        /// <summary>Live traffic was used, but not the higher-fidelity optimal model.</summary>
        [EnumMember(Value = "FALLBACK_TRAFFIC_AWARE")] TrafficAware,
    }

    /// <summary>Reason a fallback routing mode was used.</summary>
    public enum FallbackReason
    {
        /// <summary>Unspecified.</summary>
        [EnumMember(Value = "FALLBACK_REASON_UNSPECIFIED")] Unspecified,

        /// <summary>Server error caused the fallback.</summary>
        [EnumMember(Value = "SERVER_ERROR")] ServerError,

        /// <summary>Latency exceeded the requested budget; a faster mode was used.</summary>
        [EnumMember(Value = "LATENCY_EXCEEDED")] LatencyExceeded,
    }
}
