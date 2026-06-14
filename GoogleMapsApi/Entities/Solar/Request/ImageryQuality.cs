using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Solar.Request
{
    /// <summary>
    /// Quality of the imagery the Solar API used (or is required to use). Higher quality means
    /// finer-resolution data. Used both as a request constraint (<c>requiredQuality</c>) and a
    /// response field (<c>imageryQuality</c>).
    /// </summary>
    public enum ImageryQuality
    {
        /// <summary>No quality specified.</summary>
        [EnumMember(Value = "IMAGERY_QUALITY_UNSPECIFIED")] Unspecified,

        /// <summary>Imagery at 0.1 m/pixel.</summary>
        [EnumMember(Value = "HIGH")] High,

        /// <summary>Imagery at 0.25 m/pixel.</summary>
        [EnumMember(Value = "MEDIUM")] Medium,

        /// <summary>Imagery at 0.5 m/pixel.</summary>
        [EnumMember(Value = "LOW")] Low,

        /// <summary>Aerial or enhanced-aerial imagery, lower resolution than <see cref="Low"/>.</summary>
        [EnumMember(Value = "BASE")] Base,
    }
}
