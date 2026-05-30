using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Routes.Response
{
    /// <summary>Route-level advisory metadata returned when the corresponding extra computations are requested.</summary>
    public sealed class RouteTravelAdvisory
    {
        /// <summary>Toll information for the route. Populated when <see cref="Request.ExtraComputation.Tolls"/> is requested.</summary>
        [JsonPropertyName("tollInfo")]
        public TollInfo? TollInfo { get; set; }

        /// <summary>
        /// Estimated fuel consumption in microliters. Populated when
        /// <see cref="Request.ExtraComputation.FuelConsumption"/> is requested.
        /// </summary>
        [JsonPropertyName("fuelConsumptionMicroliters")]
        public string? FuelConsumptionMicroliters { get; set; }

        /// <summary>
        /// Speed-reading intervals over the route polyline. Populated when
        /// <see cref="Request.ExtraComputation.TrafficOnPolyline"/> is requested.
        /// </summary>
        [JsonPropertyName("speedReadingIntervals")]
        public List<SpeedReadingInterval>? SpeedReadingIntervals { get; set; }
    }

    /// <summary>Leg-level advisory metadata.</summary>
    public sealed class RouteLegTravelAdvisory
    {
        /// <summary>Toll information for the leg.</summary>
        [JsonPropertyName("tollInfo")]
        public TollInfo? TollInfo { get; set; }

        /// <summary>Speed-reading intervals over the leg polyline.</summary>
        [JsonPropertyName("speedReadingIntervals")]
        public List<SpeedReadingInterval>? SpeedReadingIntervals { get; set; }
    }

    /// <summary>Toll cost summary.</summary>
    public sealed class TollInfo
    {
        /// <summary>Estimated toll prices, one per currency the route crosses.</summary>
        [JsonPropertyName("estimatedPrice")]
        public List<Money>? EstimatedPrice { get; set; }
    }

    /// <summary>
    /// Monetary amount in a given currency (Google's <c>google.type.Money</c>).
    /// </summary>
    public sealed class Money
    {
        /// <summary>Three-letter ISO 4217 currency code (e.g. <c>"USD"</c>).</summary>
        [JsonPropertyName("currencyCode")]
        public string? CurrencyCode { get; set; }

        /// <summary>Whole units of the amount (e.g. 42 USD).</summary>
        [JsonPropertyName("units")]
        public string? Units { get; set; }

        /// <summary>
        /// Fractional units, in nanos (10^-9). 100,000,000 nanos = $0.10. Sign matches <see cref="Units"/>.
        /// </summary>
        [JsonPropertyName("nanos")]
        public int? Nanos { get; set; }
    }

    /// <summary>One segment of speed information over the polyline.</summary>
    public sealed class SpeedReadingInterval
    {
        /// <summary>Starting polyline-point index (inclusive).</summary>
        [JsonPropertyName("startPolylinePointIndex")]
        public int? StartPolylinePointIndex { get; set; }

        /// <summary>Ending polyline-point index (exclusive).</summary>
        [JsonPropertyName("endPolylinePointIndex")]
        public int? EndPolylinePointIndex { get; set; }

        /// <summary>Traffic speed bucket for the interval.</summary>
        [JsonPropertyName("speed")]
        public Speed? Speed { get; set; }
    }

    /// <summary>Traffic speed bucket.</summary>
    public enum Speed
    {
        /// <summary>Unspecified.</summary>
        [System.Runtime.Serialization.EnumMember(Value = "SPEED_UNSPECIFIED")] Unspecified,

        /// <summary>Normal speed; no congestion.</summary>
        [System.Runtime.Serialization.EnumMember(Value = "NORMAL")] Normal,

        /// <summary>Slow speed; light congestion.</summary>
        [System.Runtime.Serialization.EnumMember(Value = "SLOW")] Slow,

        /// <summary>Traffic jam.</summary>
        [System.Runtime.Serialization.EnumMember(Value = "TRAFFIC_JAM")] TrafficJam,
    }
}
