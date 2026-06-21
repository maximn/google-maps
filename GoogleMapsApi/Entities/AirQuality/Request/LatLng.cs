using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AirQuality.Request
{
    /// <summary>A WGS84 latitude/longitude coordinate sent in the request body (mirrors <c>google.type.LatLng</c>).</summary>
    public sealed class LatLng
    {
        /// <summary>Latitude in degrees, in the range [-90, 90].</summary>
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        /// <summary>Longitude in degrees, in the range [-180, 180].</summary>
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}
