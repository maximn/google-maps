using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Common
{
    /// <summary>A circle defined by a center point and a radius in meters.</summary>
    public sealed class Circle
    {
        /// <summary>Center of the circle.</summary>
        [JsonPropertyName("center")]
        public LatLng? Center { get; set; }

        /// <summary>Radius of the circle in meters.</summary>
        [JsonPropertyName("radius")]
        public double Radius { get; set; }
    }
}
