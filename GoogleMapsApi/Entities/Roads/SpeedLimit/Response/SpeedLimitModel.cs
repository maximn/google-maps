using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Roads.SpeedLimit.Response
{
    public class SpeedLimitModel
    {
        [DataMember(Name = "placeId")]
        public string PlaceId { get; set; }

        [DataMember(Name = "speedLimit")]
        public double SpeedLimit { get; set; }

        [DataMember(Name = "units")]
        public string Units { get; set; }
    }
}
