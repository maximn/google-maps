using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesDetails.Response
{
    /// <summary>
    /// Contains a summary of the place.
    /// </summary>
    public class PlaceEditorialSummary
    {
        /// <summary>
        /// The language of the previous fields. May not always be present.
        /// </summary>
        [JsonPropertyName("language")]
        public string Language { get; set; }

        /// <summary>
        /// A medium-length textual summary of the place.
        /// </summary>
        [JsonPropertyName("overview")]
        public string Overview { get; set; }
    }
}