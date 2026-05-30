using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Common
{
    /// <summary>Localized variant of text in a particular language (Google's <c>google.type.LocalizedText</c>).</summary>
    public sealed class LocalizedText
    {
        /// <summary>Localized string.</summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        /// <summary>BCP-47 language code of <see cref="Text"/>.</summary>
        [JsonPropertyName("languageCode")]
        public string? LanguageCode { get; set; }
    }
}
