using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Routes.Response
{
    /// <summary>A single navigation step within a <see cref="RouteLeg"/>.</summary>
    public sealed class RouteLegStep
    {
        /// <summary>Distance covered by this step in meters.</summary>
        [JsonPropertyName("distanceMeters")]
        public int? DistanceMeters { get; set; }

        /// <summary>Static (traffic-free) duration of the step as a protobuf-duration string.</summary>
        [JsonPropertyName("staticDuration")]
        public string? StaticDuration { get; set; }

        /// <summary><see cref="StaticDuration"/> parsed to seconds.</summary>
        [JsonIgnore]
        public double? StaticDurationSeconds => DurationParser.ToSeconds(StaticDuration);

        /// <summary>Polyline of this step.</summary>
        [JsonPropertyName("polyline")]
        public Polyline? Polyline { get; set; }

        /// <summary>Start location of the step.</summary>
        [JsonPropertyName("startLocation")]
        public LegLocation? StartLocation { get; set; }

        /// <summary>End location of the step.</summary>
        [JsonPropertyName("endLocation")]
        public LegLocation? EndLocation { get; set; }

        /// <summary>Navigation instruction for the step.</summary>
        [JsonPropertyName("navigationInstruction")]
        public NavigationInstruction? NavigationInstruction { get; set; }

        /// <summary>Travel mode used for this step (relevant on transit/multi-modal routes).</summary>
        [JsonPropertyName("travelMode")]
        public Request.RoutesTravelMode? TravelMode { get; set; }
    }

    /// <summary>Turn-by-turn navigation instruction for a step.</summary>
    public sealed class NavigationInstruction
    {
        /// <summary>High-level maneuver type (e.g. <c>TURN_LEFT</c>).</summary>
        [JsonPropertyName("maneuver")]
        public Maneuver? Maneuver { get; set; }

        /// <summary>Human-readable instruction text.</summary>
        [JsonPropertyName("instructions")]
        public string? Instructions { get; set; }
    }

    /// <summary>Maneuver categories returned by the Routes API.</summary>
    public enum Maneuver
    {
        /// <summary>Unspecified.</summary>
        [EnumMember(Value = "MANEUVER_UNSPECIFIED")] Unspecified,

        /// <summary>Turn slightly to the left.</summary>
        [EnumMember(Value = "TURN_SLIGHT_LEFT")] TurnSlightLeft,

        /// <summary>Turn sharply to the left.</summary>
        [EnumMember(Value = "TURN_SHARP_LEFT")] TurnSharpLeft,

        /// <summary>Make a left U-turn.</summary>
        [EnumMember(Value = "UTURN_LEFT")] UTurnLeft,

        /// <summary>Turn left.</summary>
        [EnumMember(Value = "TURN_LEFT")] TurnLeft,

        /// <summary>Turn slightly to the right.</summary>
        [EnumMember(Value = "TURN_SLIGHT_RIGHT")] TurnSlightRight,

        /// <summary>Turn sharply to the right.</summary>
        [EnumMember(Value = "TURN_SHARP_RIGHT")] TurnSharpRight,

        /// <summary>Make a right U-turn.</summary>
        [EnumMember(Value = "UTURN_RIGHT")] UTurnRight,

        /// <summary>Turn right.</summary>
        [EnumMember(Value = "TURN_RIGHT")] TurnRight,

        /// <summary>Continue straight.</summary>
        [EnumMember(Value = "STRAIGHT")] Straight,

        /// <summary>Take the ramp to the left.</summary>
        [EnumMember(Value = "RAMP_LEFT")] RampLeft,

        /// <summary>Take the ramp to the right.</summary>
        [EnumMember(Value = "RAMP_RIGHT")] RampRight,

        /// <summary>Merge.</summary>
        [EnumMember(Value = "MERGE")] Merge,

        /// <summary>Take the fork on the left.</summary>
        [EnumMember(Value = "FORK_LEFT")] ForkLeft,

        /// <summary>Take the fork on the right.</summary>
        [EnumMember(Value = "FORK_RIGHT")] ForkRight,

        /// <summary>Take the ferry.</summary>
        [EnumMember(Value = "FERRY")] Ferry,

        /// <summary>Take a train onto a ferry.</summary>
        [EnumMember(Value = "FERRY_TRAIN")] FerryTrain,

        /// <summary>Turn left at a roundabout.</summary>
        [EnumMember(Value = "ROUNDABOUT_LEFT")] RoundaboutLeft,

        /// <summary>Turn right at a roundabout.</summary>
        [EnumMember(Value = "ROUNDABOUT_RIGHT")] RoundaboutRight,

        /// <summary>Initial maneuver — depart.</summary>
        [EnumMember(Value = "DEPART")] Depart,

        /// <summary>Used to indicate a street name change.</summary>
        [EnumMember(Value = "NAME_CHANGE")] NameChange,
    }
}
