using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AirQuality.Response
{
    /// <summary>Supplementary information about a pollutant (returned with <c>POLLUTANT_ADDITIONAL_INFO</c>).</summary>
    public sealed class AdditionalInfo
    {
        /// <summary>Text describing the pollutant's main emission sources.</summary>
        [JsonPropertyName("sources")]
        public string? Sources { get; set; }

        /// <summary>Text describing the pollutant's main health effects.</summary>
        [JsonPropertyName("effects")]
        public string? Effects { get; set; }
    }
}
