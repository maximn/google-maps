using System.Collections.Generic;

namespace GoogleMapsApi.StaticMaps.Entities
{
    /// <summary>
    /// Represents a single style rule from Google Styling Wizard JSON
    /// </summary>
    public class MapStyleRule
    {
        /// <summary>
        /// The element type to style (e.g., "geometry", "labels.icon", "labels.text")
        /// </summary>
        public string ElementType { get; set; }

        /// <summary>
        /// The feature type to style (e.g., "water", "road", "landscape")
        /// </summary>
        public string FeatureType { get; set; }

        /// <summary>
        /// List of stylers to apply to the element/feature
        /// </summary>
        public List<MapStyleStyler> Stylers { get; set; } = new List<MapStyleStyler>();
    }
}

