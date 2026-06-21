using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Pollen.Response
{
    /// <summary>A pollen index value (the Universal Pollen Index) for a pollen type or plant.</summary>
    public sealed class IndexInfo
    {
        /// <summary>The index's code (e.g. <c>"UPI"</c>).</summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>Human-readable index name (e.g. <c>"Universal Pollen Index"</c>).</summary>
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        /// <summary>The index value, on a 0-5 scale.</summary>
        [JsonPropertyName("value")]
        public int Value { get; set; }

        /// <summary>Textual classification of the value (e.g. <c>"Low"</c>, <c>"High"</c>).</summary>
        [JsonPropertyName("category")]
        public string? Category { get; set; }

        /// <summary>A human-readable explanation of the index value.</summary>
        [JsonPropertyName("indexDescription")]
        public string? IndexDescription { get; set; }

        /// <summary>The colour associated with the value, for rendering.</summary>
        [JsonPropertyName("color")]
        public Color? Color { get; set; }
    }
}
