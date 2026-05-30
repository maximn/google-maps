using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Routes.Response
{
    /// <summary>
    /// Diagnostic information about how the request's waypoints were geocoded. Returned only when
    /// the request supplies waypoints by free-text address (waypoints supplied by place ID or
    /// lat/lng do not require geocoding).
    /// </summary>
    public sealed class GeocodingResults
    {
        /// <summary>Geocoded origin waypoint.</summary>
        [JsonPropertyName("origin")]
        public GeocodedWaypoint? Origin { get; set; }

        /// <summary>Geocoded destination waypoint.</summary>
        [JsonPropertyName("destination")]
        public GeocodedWaypoint? Destination { get; set; }

        /// <summary>Geocoded intermediate waypoints, in input order.</summary>
        [JsonPropertyName("intermediates")]
        public List<GeocodedIntermediate>? Intermediates { get; set; }
    }

    /// <summary>Geocoded information for a single waypoint.</summary>
    public class GeocodedWaypoint
    {
        /// <summary>
        /// Status of the geocode. <c>0</c> indicates success; non-zero values indicate an error.
        /// </summary>
        [JsonPropertyName("geocoderStatus")]
        public GeocoderStatus? GeocoderStatus { get; set; }

        /// <summary>Resulting place ID for the geocoded location.</summary>
        [JsonPropertyName("placeId")]
        public string? PlaceId { get; set; }

        /// <summary>Place type tags (e.g. <c>"street_address"</c>, <c>"premise"</c>).</summary>
        [JsonPropertyName("type")]
        public List<string>? Type { get; set; }

        /// <summary>True if the geocode was a partial match (the input was not exact).</summary>
        [JsonPropertyName("partialMatch")]
        public bool? PartialMatch { get; set; }
    }

    /// <summary>Geocoded intermediate waypoint with its input position.</summary>
    public sealed class GeocodedIntermediate : GeocodedWaypoint
    {
        /// <summary>Zero-based index of this waypoint in the request's <c>Intermediates</c> list.</summary>
        [JsonPropertyName("intermediateWaypointRequestIndex")]
        public int? IntermediateWaypointRequestIndex { get; set; }
    }

    /// <summary>RPC-style status returned by the geocoder.</summary>
    public sealed class GeocoderStatus
    {
        /// <summary>Status code (0 means OK).</summary>
        [JsonPropertyName("code")]
        public int? Code { get; set; }

        /// <summary>Human-readable status message.</summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
