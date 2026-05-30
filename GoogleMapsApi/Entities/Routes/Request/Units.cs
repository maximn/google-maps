using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Routes.Request
{
    /// <summary>Display units for distance values in the response.</summary>
    public enum Units
    {
        /// <summary>Unspecified; the API picks based on the request's region.</summary>
        [EnumMember(Value = "UNITS_UNSPECIFIED")] Unspecified,

        /// <summary>Metric units (meters, kilometers).</summary>
        [EnumMember(Value = "METRIC")] Metric,

        /// <summary>Imperial units (feet, miles).</summary>
        [EnumMember(Value = "IMPERIAL")] Imperial,
    }
}
