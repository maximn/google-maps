using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.PlacesNew.Common;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>A user review of a place.</summary>
    public sealed class Review
    {
        /// <summary>Resource name of the review.</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Human-readable, localized "time ago" description of the publish time.</summary>
        [JsonPropertyName("relativePublishTimeDescription")]
        public string? RelativePublishTimeDescription { get; set; }

        /// <summary>Star rating of the review, 1.0–5.0.</summary>
        [JsonPropertyName("rating")]
        public double? Rating { get; set; }

        /// <summary>Localized review text.</summary>
        [JsonPropertyName("text")]
        public LocalizedText? Text { get; set; }

        /// <summary>Original (untranslated) review text.</summary>
        [JsonPropertyName("originalText")]
        public LocalizedText? OriginalText { get; set; }

        /// <summary>Attribution for the review's author.</summary>
        [JsonPropertyName("authorAttribution")]
        public AuthorAttribution? AuthorAttribution { get; set; }

        /// <summary>RFC-3339 timestamp the review was published.</summary>
        [JsonPropertyName("publishTime")]
        public string? PublishTime { get; set; }
    }
}
