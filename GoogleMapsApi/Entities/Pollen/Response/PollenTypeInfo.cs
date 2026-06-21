using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Pollen.Response
{
    /// <summary>Pollen information for one pollen type (grass, tree or weed) on a given day.</summary>
    public sealed class PollenTypeInfo
    {
        /// <summary>The pollen type code (<c>"GRASS"</c>, <c>"TREE"</c> or <c>"WEED"</c>).</summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>Human-readable pollen type name (e.g. <c>"Grass"</c>).</summary>
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        /// <summary>Whether the pollen type is in season on this day.</summary>
        [JsonPropertyName("inSeason")]
        public bool? InSeason { get; set; }

        /// <summary>The pollen index for this type. Absent when no data is available.</summary>
        [JsonPropertyName("indexInfo")]
        public IndexInfo? IndexInfo { get; set; }

        /// <summary>Health recommendations related to this pollen type.</summary>
        [JsonPropertyName("healthRecommendations")]
        public List<string>? HealthRecommendations { get; set; }
    }
}
