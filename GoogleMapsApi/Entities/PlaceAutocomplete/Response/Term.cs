using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.PlaceAutocomplete.Response
{
    /// <summary>
    /// Identifies a section of description in a PlaceAutocomplete search result
    /// </summary>
    public class Term
    {
        /// <summary>
        /// The text of the term
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }

        /// <summary>
        /// The start position of this term in the description, measured in Unicode characters
        /// </summary>
        [JsonPropertyName("offset")]
        public int Offset { get; set; }
    }
}
