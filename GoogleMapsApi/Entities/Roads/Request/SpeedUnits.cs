using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Roads.Request
{
    /// <summary>
    /// Unit system requested for Speed Limits results. Sent as the <c>units</c> query parameter.
    /// </summary>
    public enum SpeedUnits
    {
        /// <summary>Kilometers per hour (the Roads API default). Sent as <c>KPH</c>.</summary>
        [EnumMember(Value = "KPH")]
        Kph,

        /// <summary>Miles per hour. Sent as <c>MPH</c>.</summary>
        [EnumMember(Value = "MPH")]
        Mph,
    }
}
