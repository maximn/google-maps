using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Common
{
    /// <summary>
    /// Hard boundary that restricts results to a region. Set exactly one of <see cref="Circle"/>
    /// or <see cref="Rectangle"/>.
    /// </summary>
    public sealed class LocationRestriction
    {
        /// <summary>Restrict results to this circle.</summary>
        [JsonPropertyName("circle")]
        public Circle? Circle { get; set; }

        /// <summary>Restrict results to this rectangle (viewport).</summary>
        [JsonPropertyName("rectangle")]
        public Viewport? Rectangle { get; set; }
    }
}
