using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>An autocomplete prediction for a search query (rather than a specific place).</summary>
    public sealed class QueryPrediction
    {
        /// <summary>Full text of the predicted query with match highlights.</summary>
        [JsonPropertyName("text")]
        public FormattableText? Text { get; set; }

        /// <summary>Prediction split into main and secondary text.</summary>
        [JsonPropertyName("structuredFormat")]
        public StructuredFormat? StructuredFormat { get; set; }
    }
}
