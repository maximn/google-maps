using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>A latitude/longitude bounding box defined by its southwest and northeast corners.</summary>
    public sealed class LatLngBox
    {
        /// <summary>The southwest corner of the box.</summary>
        [JsonPropertyName("sw")]
        public LatLng? Sw { get; set; }

        /// <summary>The northeast corner of the box.</summary>
        [JsonPropertyName("ne")]
        public LatLng? Ne { get; set; }
    }
}
