using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.AirQuality.Request;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.AirQuality.Response
{
    /// <summary>Response from the Air Quality API <c>currentConditions:lookup</c> endpoint.</summary>
    public sealed class CurrentConditionsResponse : IResponseFor<CurrentConditionsRequest>
    {
        /// <summary>The hour (rounded down) the conditions apply to.</summary>
        [JsonPropertyName("dateTime")]
        public DateTimeOffset? DateTime { get; set; }

        /// <summary>Region code (ISO 3166-1 alpha-2) of the location.</summary>
        [JsonPropertyName("regionCode")]
        public string? RegionCode { get; set; }

        /// <summary>The air-quality indexes for the location.</summary>
        [JsonPropertyName("indexes")]
        public List<AirQualityIndex>? Indexes { get; set; }

        /// <summary>Per-pollutant data (when requested via extra computations).</summary>
        [JsonPropertyName("pollutants")]
        public List<Pollutant>? Pollutants { get; set; }

        /// <summary>Health recommendations (when requested via the <c>HEALTH_RECOMMENDATIONS</c> computation).</summary>
        [JsonPropertyName("healthRecommendations")]
        public HealthRecommendations? HealthRecommendations { get; set; }
    }
}
