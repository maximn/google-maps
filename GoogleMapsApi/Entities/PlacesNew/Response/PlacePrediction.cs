using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>An autocomplete prediction for a specific place.</summary>
    public sealed class PlacePrediction
    {
        /// <summary>Resource name of the predicted place (e.g. <c>places/{placeId}</c>).</summary>
        [JsonPropertyName("place")]
        public string? Place { get; set; }

        /// <summary>Place ID of the predicted place.</summary>
        [JsonPropertyName("placeId")]
        public string? PlaceId { get; set; }

        /// <summary>Full text of the prediction with match highlights.</summary>
        [JsonPropertyName("text")]
        public FormattableText? Text { get; set; }

        /// <summary>Prediction split into main and secondary text.</summary>
        [JsonPropertyName("structuredFormat")]
        public StructuredFormat? StructuredFormat { get; set; }

        /// <summary>Place types associated with the predicted place.</summary>
        [JsonPropertyName("types")]
        public List<string>? Types { get; set; }
    }
}
