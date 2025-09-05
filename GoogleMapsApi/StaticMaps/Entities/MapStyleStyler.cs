namespace GoogleMapsApi.StaticMaps.Entities
{
    /// <summary>
    /// Represents a single styler from Google Styling Wizard JSON
    /// </summary>
    public class MapStyleStyler
    {
        /// <summary>
        /// Color in hex format (e.g., "#f5f5f5")
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// Visibility setting ("on", "off", "simplified")
        /// </summary>
        public string Visibility { get; set; }

        /// <summary>
        /// Lightness value (-100 to 100)
        /// </summary>
        public float? Lightness { get; set; }

        /// <summary>
        /// Saturation value (-100 to 100)
        /// </summary>
        public float? Saturation { get; set; }

        /// <summary>
        /// Gamma value (0.01 to 10.0)
        /// </summary>
        public float? Gamma { get; set; }

        /// <summary>
        /// Hue value (hex color)
        /// </summary>
        public string? Hue { get; set; }

        /// <summary>
        /// Weight for lines/paths
        /// </summary>
        public int? Weight { get; set; }
    }
}

