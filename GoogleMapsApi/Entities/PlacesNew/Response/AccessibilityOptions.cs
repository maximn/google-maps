using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>Accessibility options available at a place.</summary>
    public sealed class AccessibilityOptions
    {
        /// <summary>Whether wheelchair-accessible parking is available.</summary>
        [JsonPropertyName("wheelchairAccessibleParking")]
        public bool? WheelchairAccessibleParking { get; set; }

        /// <summary>Whether the entrance is wheelchair accessible.</summary>
        [JsonPropertyName("wheelchairAccessibleEntrance")]
        public bool? WheelchairAccessibleEntrance { get; set; }

        /// <summary>Whether a wheelchair-accessible restroom is available.</summary>
        [JsonPropertyName("wheelchairAccessibleRestroom")]
        public bool? WheelchairAccessibleRestroom { get; set; }

        /// <summary>Whether wheelchair-accessible seating is available.</summary>
        [JsonPropertyName("wheelchairAccessibleSeating")]
        public bool? WheelchairAccessibleSeating { get; set; }
    }
}
