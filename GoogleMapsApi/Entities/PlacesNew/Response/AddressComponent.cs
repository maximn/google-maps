using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>A structured component of a place's address.</summary>
    public sealed class AddressComponent
    {
        /// <summary>Full text description of the component.</summary>
        [JsonPropertyName("longText")]
        public string? LongText { get; set; }

        /// <summary>Abbreviated text of the component, if available.</summary>
        [JsonPropertyName("shortText")]
        public string? ShortText { get; set; }

        /// <summary>Types of this address component.</summary>
        [JsonPropertyName("types")]
        public List<string>? Types { get; set; }

        /// <summary>BCP-47 language code of the component text.</summary>
        [JsonPropertyName("languageCode")]
        public string? LanguageCode { get; set; }
    }
}
