using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Routes.Request
{
    /// <summary>Trade-off between polyline detail and response size.</summary>
    public enum PolylineQuality
    {
        /// <summary>Unspecified; the API uses <see cref="Overview"/>.</summary>
        [EnumMember(Value = "POLYLINE_QUALITY_UNSPECIFIED")] Unspecified,

        /// <summary>High-fidelity polyline (more points, larger response).</summary>
        [EnumMember(Value = "HIGH_QUALITY")] HighQuality,

        /// <summary>Overview polyline suitable for display at city/regional zoom.</summary>
        [EnumMember(Value = "OVERVIEW")] Overview,
    }
}
