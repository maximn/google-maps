using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.PlacesNearBy.Response
{
    /// <summary>
    /// Contains the location
    /// </summary>
    public class Geometry
    {
        [JsonPropertyName("location")]
        public Location Location { get; set; } = null!;
    }
}
