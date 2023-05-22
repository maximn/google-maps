using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.PlacesDetails.Response
{
    [DataContract]
    public enum BusinessStatus
    {
        [EnumMember(Value = "OPERATIONAL")]
        Operational,
        [EnumMember(Value = "CLOSED_TEMPORARILY")]
        ClosedTemporarily,
        [EnumMember(Value = "CLOSED_PERMANENTLY")]
        ClosedPermanently,
    }
}
