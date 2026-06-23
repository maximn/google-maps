using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Pollen.Response
{
    /// <summary>Descriptive information about a plant that produces pollen.</summary>
    public sealed class PlantDescription
    {
        /// <summary>The pollen type this plant belongs to (<c>"GRASS"</c>, <c>"TREE"</c> or <c>"WEED"</c>).</summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>The plant's botanical family (e.g. <c>"Betulaceae"</c>).</summary>
        [JsonPropertyName("family")]
        public string? Family { get; set; }

        /// <summary>The seasons in which the plant typically pollinates.</summary>
        [JsonPropertyName("season")]
        public string? Season { get; set; }

        /// <summary>Colours that help identify the plant.</summary>
        [JsonPropertyName("specialColors")]
        public string? SpecialColors { get; set; }

        /// <summary>Shapes that help identify the plant.</summary>
        [JsonPropertyName("specialShapes")]
        public string? SpecialShapes { get; set; }

        /// <summary>Information about cross-reaction with other allergens.</summary>
        [JsonPropertyName("crossReaction")]
        public string? CrossReaction { get; set; }

        /// <summary>URL of a representative picture of the plant.</summary>
        [JsonPropertyName("picture")]
        public string? Picture { get; set; }

        /// <summary>URL of a close-up picture of the plant.</summary>
        [JsonPropertyName("pictureCloseup")]
        public string? PictureCloseup { get; set; }
    }
}
