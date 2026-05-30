using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Common
{
    /// <summary>
    /// A latitude/longitude viewport, represented as two diagonally opposite <see cref="LatLng"/>
    /// points (<see cref="Low"/> = south-west, <see cref="High"/> = north-east). Used both as a
    /// rectangular location bias/restriction in requests and as a place's viewport in responses.
    /// </summary>
    public sealed class Viewport
    {
        /// <summary>Low (south-west) corner of the viewport.</summary>
        [JsonPropertyName("low")]
        public LatLng? Low { get; set; }

        /// <summary>High (north-east) corner of the viewport.</summary>
        [JsonPropertyName("high")]
        public LatLng? High { get; set; }
    }
}
