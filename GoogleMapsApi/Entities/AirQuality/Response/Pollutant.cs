using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AirQuality.Response
{
    /// <summary>Data for a single pollutant (returned with the pollutant-concentration extra computations).</summary>
    public sealed class Pollutant
    {
        /// <summary>The pollutant's code (e.g. <c>"pm25"</c>, <c>"no2"</c>).</summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>The pollutant's short display name (e.g. <c>"PM2.5"</c>).</summary>
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        /// <summary>The pollutant's full name (e.g. <c>"Fine particulate matter (&lt;2.5µm)"</c>).</summary>
        [JsonPropertyName("fullName")]
        public string? FullName { get; set; }

        /// <summary>The measured concentration.</summary>
        [JsonPropertyName("concentration")]
        public Concentration? Concentration { get; set; }

        /// <summary>Supplementary sources/effects information for this pollutant.</summary>
        [JsonPropertyName("additionalInfo")]
        public AdditionalInfo? AdditionalInfo { get; set; }
    }
}
