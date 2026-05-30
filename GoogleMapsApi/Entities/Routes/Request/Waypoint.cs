using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Routes.Request
{
    /// <summary>
    /// Origin, destination, or intermediate stop on a route. Set exactly one of
    /// <see cref="Location"/>, <see cref="PlaceId"/>, or <see cref="Address"/>.
    /// </summary>
    public sealed class Waypoint
    {
        /// <summary>Latitude / longitude (with optional heading) of this waypoint.</summary>
        [JsonPropertyName("location")]
        public WaypointLocation? Location { get; set; }

        /// <summary>
        /// Google Places place ID — the preferred way to specify a waypoint when known, since
        /// it bypasses geocoding ambiguity.
        /// </summary>
        [JsonPropertyName("placeId")]
        public string? PlaceId { get; set; }

        /// <summary>Free-text address; geocoded by the API at request time.</summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// True if this waypoint is a pass-through point (no stop). Only meaningful for
        /// intermediate waypoints; ignored for origin/destination.
        /// </summary>
        [JsonPropertyName("via")]
        public bool? Via { get; set; }

        /// <summary>
        /// True if the waypoint is on a particular side of the road (so the route prefers
        /// approaches from that side). Driving routes only.
        /// </summary>
        [JsonPropertyName("sideOfRoad")]
        public bool? SideOfRoad { get; set; }

        /// <summary>
        /// True if the waypoint is intended as a stop where the vehicle parks. Affects
        /// guidance for the leg and U-turn preferences.
        /// </summary>
        [JsonPropertyName("vehicleStopover")]
        public bool? VehicleStopover { get; set; }

        /// <summary>
        /// Creates a waypoint from a free-text address.
        /// </summary>
        public static Waypoint FromAddress(string address) => new Waypoint { Address = address };

        /// <summary>
        /// Creates a waypoint from a Places place ID.
        /// </summary>
        public static Waypoint FromPlaceId(string placeId) => new Waypoint { PlaceId = placeId };

        /// <summary>
        /// Creates a waypoint from a latitude/longitude pair.
        /// </summary>
        public static Waypoint FromCoordinates(double latitude, double longitude)
            => new Waypoint { Location = new WaypointLocation { LatLng = new LatLng { Latitude = latitude, Longitude = longitude } } };
    }

    /// <summary>Geographic location of a <see cref="Waypoint"/>.</summary>
    public sealed class WaypointLocation
    {
        /// <summary>Latitude / longitude in WGS84.</summary>
        [JsonPropertyName("latLng")]
        public LatLng? LatLng { get; set; }

        /// <summary>
        /// Compass heading in degrees [0, 360) the vehicle is moving in. Only meaningful for
        /// driving routes and only respected when <see cref="Waypoint.SideOfRoad"/> is true.
        /// </summary>
        [JsonPropertyName("heading")]
        public int? Heading { get; set; }
    }

    /// <summary>Latitude/longitude pair in WGS84 (Google's <c>google.type.LatLng</c>).</summary>
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
