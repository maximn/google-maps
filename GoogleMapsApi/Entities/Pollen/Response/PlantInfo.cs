using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Pollen.Response
{
    /// <summary>Pollen information for one specific plant species on a given day.</summary>
    public sealed class PlantInfo
    {
        /// <summary>The plant's code (e.g. <c>"BIRCH"</c>, <c>"OAK"</c>, <c>"RAGWEED"</c>).</summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>Human-readable plant name (e.g. <c>"Birch"</c>).</summary>
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        /// <summary>Whether the plant is in season on this day.</summary>
        [JsonPropertyName("inSeason")]
        public bool? InSeason { get; set; }

        /// <summary>The pollen index for this plant. Absent when no data is available.</summary>
        [JsonPropertyName("indexInfo")]
        public IndexInfo? IndexInfo { get; set; }

        /// <summary>Descriptive detail about the plant. Absent when <c>PlantsDescription</c> was disabled.</summary>
        [JsonPropertyName("plantDescription")]
        public PlantDescription? PlantDescription { get; set; }
    }
}
