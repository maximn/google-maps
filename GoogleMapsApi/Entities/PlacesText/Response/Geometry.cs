using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.PlacesText.Response
{
    /// <summary>
    /// Contains the location
    /// </summary>
    public class Geometry
    {
        [JsonPropertyName("location")]
        public Location Location { get; set; }
    }
}