using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AirQuality.Response
{
    /// <summary>A single air-quality index value (e.g. the Universal AQI or a local/national index).</summary>
    public sealed class AirQualityIndex
    {
        /// <summary>The index's code (e.g. <c>"uaqi"</c>, <c>"usa_epa"</c>).</summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>Human-readable index name (e.g. <c>"Universal AQI"</c>).</summary>
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        /// <summary>The numeric index score.</summary>
        [JsonPropertyName("aqi")]
        public int Aqi { get; set; }

        /// <summary>The index score as it should be displayed.</summary>
        [JsonPropertyName("aqiDisplay")]
        public string? AqiDisplay { get; set; }

        /// <summary>The colour associated with the score, for rendering.</summary>
        [JsonPropertyName("color")]
        public Color? Color { get; set; }

        /// <summary>Textual classification of the score (e.g. <c>"Good air quality"</c>).</summary>
        [JsonPropertyName("category")]
        public string? Category { get; set; }

        /// <summary>Code of the pollutant dominating this index (e.g. <c>"pm25"</c>).</summary>
        [JsonPropertyName("dominantPollutant")]
        public string? DominantPollutant { get; set; }
    }
}
