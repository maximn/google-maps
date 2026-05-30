using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Routes.Response
{
    /// <summary>Latitude/longitude viewport that bounds a route or feature.</summary>
    public sealed class Viewport
    {
        /// <summary>Low (southwest) corner.</summary>
        [JsonPropertyName("low")]
        public Request.LatLng? Low { get; set; }

        /// <summary>High (northeast) corner.</summary>
        [JsonPropertyName("high")]
        public Request.LatLng? High { get; set; }
    }
}
