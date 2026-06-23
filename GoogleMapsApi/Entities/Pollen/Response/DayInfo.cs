using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Pollen.Response
{
    /// <summary>Pollen forecast for a single day.</summary>
    public sealed class DayInfo
    {
        /// <summary>The date this forecast applies to.</summary>
        [JsonPropertyName("date")]
        public Date? Date { get; set; }

        /// <summary>Per-pollen-type information (up to three: grass, tree, weed).</summary>
        [JsonPropertyName("pollenTypeInfo")]
        public List<PollenTypeInfo>? PollenTypeInfo { get; set; }

        /// <summary>Per-plant information (up to fifteen species).</summary>
        [JsonPropertyName("plantInfo")]
        public List<PlantInfo>? PlantInfo { get; set; }
    }
}
