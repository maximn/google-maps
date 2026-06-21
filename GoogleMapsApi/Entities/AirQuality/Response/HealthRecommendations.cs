using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AirQuality.Response
{
    /// <summary>
    /// Health advice tailored to the current conditions for the general population and at-risk groups
    /// (returned with the <c>HEALTH_RECOMMENDATIONS</c> extra computation).
    /// </summary>
    public sealed class HealthRecommendations
    {
        /// <summary>Advice for the general population.</summary>
        [JsonPropertyName("generalPopulation")]
        public string? GeneralPopulation { get; set; }

        /// <summary>Advice for elderly people.</summary>
        [JsonPropertyName("elderly")]
        public string? Elderly { get; set; }

        /// <summary>Advice for people with lung disease.</summary>
        [JsonPropertyName("lungDiseasePopulation")]
        public string? LungDiseasePopulation { get; set; }

        /// <summary>Advice for people with heart disease.</summary>
        [JsonPropertyName("heartDiseasePopulation")]
        public string? HeartDiseasePopulation { get; set; }

        /// <summary>Advice for athletes / people exercising outdoors.</summary>
        [JsonPropertyName("athletes")]
        public string? Athletes { get; set; }

        /// <summary>Advice for pregnant women.</summary>
        [JsonPropertyName("pregnantWomen")]
        public string? PregnantWomen { get; set; }

        /// <summary>Advice for children.</summary>
        [JsonPropertyName("children")]
        public string? Children { get; set; }
    }
}
