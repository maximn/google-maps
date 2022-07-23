using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.StaticMaps.Entities;
using GoogleMapsApi.StaticMaps.Enums;

namespace MapsWebApi.Models
{
    public class CustomImageRequest
    {
        public string City { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public MapType MapType { get; set; } = MapType.Terrain;
    }
}