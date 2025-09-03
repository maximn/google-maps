using GoogleMapsApi.Entities.Common;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesRadar.Response
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
