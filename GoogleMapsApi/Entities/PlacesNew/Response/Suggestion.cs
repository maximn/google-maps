using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>
    /// A single autocomplete suggestion. Exactly one of <see cref="PlacePrediction"/> or
    /// <see cref="QueryPrediction"/> is populated.
    /// </summary>
    public sealed class Suggestion
    {
        /// <summary>Prediction for a specific place.</summary>
        [JsonPropertyName("placePrediction")]
        public PlacePrediction? PlacePrediction { get; set; }

        /// <summary>Prediction for a search query.</summary>
        [JsonPropertyName("queryPrediction")]
        public QueryPrediction? QueryPrediction { get; set; }
    }
}
