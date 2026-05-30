using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Common
{
    /// <summary>
    /// Latitude/longitude pair (Google's <c>google.type.LatLng</c>). The Places API (New) uses
    /// <c>latitude</c>/<c>longitude</c> rather than the legacy <c>lat</c>/<c>lng</c> shape.
    /// Used by both requests and responses.
    /// </summary>
    public sealed class LatLng
    {
        /// <summary>Latitude in decimal degrees.</summary>
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        /// <summary>Longitude in decimal degrees.</summary>
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}
