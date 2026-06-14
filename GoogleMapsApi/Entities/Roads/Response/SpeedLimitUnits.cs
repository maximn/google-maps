using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Roads.Response
{
    /// <summary>
    /// Unit of the speed value on a <see cref="SpeedLimit"/>.
    /// </summary>
    public enum SpeedLimitUnits
    {
        /// <summary>Kilometers per hour. The Roads API returns this as <c>KMPH</c>.</summary>
        [EnumMember(Value = "KMPH")]
        Kmph,

        /// <summary>Miles per hour.</summary>
        [EnumMember(Value = "MPH")]
        Mph,
    }
}
