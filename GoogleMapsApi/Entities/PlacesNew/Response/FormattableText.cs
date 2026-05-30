using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>Text plus the substrings that matched the autocomplete input.</summary>
    public sealed class FormattableText
    {
        /// <summary>The text to display.</summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        /// <summary>Ranges within <see cref="Text"/> that matched the user's input.</summary>
        [JsonPropertyName("matches")]
        public List<StringRange>? Matches { get; set; }
    }

    /// <summary>An offset range within a string.</summary>
    public sealed class StringRange
    {
        /// <summary>Zero-based start offset of the range, inclusive.</summary>
        [JsonPropertyName("startOffset")]
        public int? StartOffset { get; set; }

        /// <summary>Zero-based end offset of the range, exclusive.</summary>
        [JsonPropertyName("endOffset")]
        public int? EndOffset { get; set; }
    }
}
