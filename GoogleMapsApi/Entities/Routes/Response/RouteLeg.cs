using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Routes.Response
{
    /// <summary>A single leg of a <see cref="Route"/> (between two waypoints).</summary>
    public sealed class RouteLeg
    {
        /// <summary>Distance covered by this leg in meters.</summary>
        [JsonPropertyName("distanceMeters")]
        public int? DistanceMeters { get; set; }

        /// <summary>Travel time for this leg as a protobuf-duration string (e.g. <c>"123s"</c>).</summary>
        [JsonPropertyName("duration")]
        public string? Duration { get; set; }

        /// <summary><see cref="Duration"/> parsed to seconds.</summary>
        [JsonIgnore]
        public double? DurationSeconds => DurationParser.ToSeconds(Duration);

        /// <summary>Static (traffic-free) travel time for this leg as a protobuf-duration string.</summary>
        [JsonPropertyName("staticDuration")]
        public string? StaticDuration { get; set; }

        /// <summary><see cref="StaticDuration"/> parsed to seconds.</summary>
        [JsonIgnore]
        public double? StaticDurationSeconds => DurationParser.ToSeconds(StaticDuration);

        /// <summary>Polyline of this leg.</summary>
        [JsonPropertyName("polyline")]
        public Polyline? Polyline { get; set; }

        /// <summary>Start location of the leg (waypoint snapped to road).</summary>
        [JsonPropertyName("startLocation")]
        public LegLocation? StartLocation { get; set; }

        /// <summary>End location of the leg (waypoint snapped to road).</summary>
        [JsonPropertyName("endLocation")]
        public LegLocation? EndLocation { get; set; }

        /// <summary>Steps that make up this leg.</summary>
        [JsonPropertyName("steps")]
        public List<RouteLegStep>? Steps { get; set; }

        /// <summary>Travel-advisory metadata for the leg.</summary>
        [JsonPropertyName("travelAdvisory")]
        public RouteLegTravelAdvisory? TravelAdvisory { get; set; }
    }

    /// <summary>Geographic location of a leg endpoint.</summary>
    public sealed class LegLocation
    {
        /// <summary>Latitude / longitude in WGS84.</summary>
        [JsonPropertyName("latLng")]
        public Request.LatLng? LatLng { get; set; }
    }
}
