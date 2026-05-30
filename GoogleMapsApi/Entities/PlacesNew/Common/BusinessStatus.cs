using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Common
{
    /// <summary>Operational status of a place (business).</summary>
    public enum BusinessStatus
    {
        /// <summary>Status unspecified or unknown.</summary>
        [EnumMember(Value = "BUSINESS_STATUS_UNSPECIFIED")] Unspecified,

        /// <summary>The establishment is operational, not closed either temporarily or permanently.</summary>
        [EnumMember(Value = "OPERATIONAL")] Operational,

        /// <summary>The establishment is temporarily closed.</summary>
        [EnumMember(Value = "CLOSED_TEMPORARILY")] ClosedTemporarily,

        /// <summary>The establishment is permanently closed.</summary>
        [EnumMember(Value = "CLOSED_PERMANENTLY")] ClosedPermanently,
    }
}
