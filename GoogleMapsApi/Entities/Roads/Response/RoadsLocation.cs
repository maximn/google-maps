using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Roads.Response
{
    /// <summary>
    /// A latitude/longitude coordinate returned by the Roads API. Unlike
    /// <see cref="GoogleMapsApi.Entities.Common.Location"/> (which serializes as <c>lat</c>/<c>lng</c>),
    /// the Roads API uses the <c>latitude</c>/<c>longitude</c> JSON keys.
    /// </summary>
    public sealed class RoadsLocation
    {
        /// <summary>Latitude in decimal degrees.</summary>
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        /// <summary>Longitude in decimal degrees.</summary>
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}
