using System;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesDetails.Response
{
    public class Aspect
    {
        /// <summary>
        /// Event id.
        /// </summary>
        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

    }
}
