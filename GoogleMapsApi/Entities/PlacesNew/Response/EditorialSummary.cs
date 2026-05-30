using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>A short, editorial summary of a place.</summary>
    public sealed class EditorialSummary
    {
        /// <summary>The summary text.</summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        /// <summary>BCP-47 language code of <see cref="Text"/>.</summary>
        [JsonPropertyName("languageCode")]
        public string? LanguageCode { get; set; }
    }
}
