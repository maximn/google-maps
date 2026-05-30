using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Routes.Request
{
    /// <summary>
    /// Mode of transportation for a <see cref="RoutesRequest"/>. Note that Routes API adds
    /// <see cref="TwoWheeler"/> over the legacy Directions API.
    /// </summary>
    public enum RoutesTravelMode
    {
        /// <summary>Travel mode unspecified. The API treats this as <see cref="Drive"/>.</summary>
        [EnumMember(Value = "TRAVEL_MODE_UNSPECIFIED")] Unspecified,

        /// <summary>Travel by passenger car.</summary>
        [EnumMember(Value = "DRIVE")] Drive,

        /// <summary>Travel by bicycle.</summary>
        [EnumMember(Value = "BICYCLE")] Bicycle,

        /// <summary>Travel by walking.</summary>
        [EnumMember(Value = "WALK")] Walk,

        /// <summary>
        /// Two-wheeled, motorized vehicle (e.g. motorcycle). Note this differs from
        /// <see cref="Bicycle"/> which is human-powered. Available only in a subset of regions.
        /// </summary>
        [EnumMember(Value = "TWO_WHEELER")] TwoWheeler,

        /// <summary>Public transit travel; only available for selected regions.</summary>
        [EnumMember(Value = "TRANSIT")] Transit,
    }
}
