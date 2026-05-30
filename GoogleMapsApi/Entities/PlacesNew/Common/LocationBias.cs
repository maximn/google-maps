using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Common
{
    /// <summary>
    /// Soft boundary that biases results toward a region. Set exactly one of <see cref="Circle"/>
    /// or <see cref="Rectangle"/>.
    /// </summary>
    public sealed class LocationBias
    {
        /// <summary>Bias results toward this circle.</summary>
        [JsonPropertyName("circle")]
        public Circle? Circle { get; set; }

        /// <summary>Bias results toward this rectangle (viewport).</summary>
        [JsonPropertyName("rectangle")]
        public Viewport? Rectangle { get; set; }
    }
}
