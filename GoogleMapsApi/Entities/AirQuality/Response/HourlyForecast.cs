using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AirQuality.Response
{
    /// <summary>Air-quality data for a single hour, used by the forecast and history responses.</summary>
    public sealed class HourlyForecast
    {
        /// <summary>The hour (rounded down) these values apply to.</summary>
        [JsonPropertyName("dateTime")]
        public DateTimeOffset? DateTime { get; set; }

        /// <summary>The air-quality indexes for this hour.</summary>
        [JsonPropertyName("indexes")]
        public List<AirQualityIndex>? Indexes { get; set; }

        /// <summary>Per-pollutant data for this hour (when requested via extra computations).</summary>
        [JsonPropertyName("pollutants")]
        public List<Pollutant>? Pollutants { get; set; }

        /// <summary>Health recommendations for this hour (when requested via extra computations).</summary>
        [JsonPropertyName("healthRecommendations")]
        public HealthRecommendations? HealthRecommendations { get; set; }
    }
}
