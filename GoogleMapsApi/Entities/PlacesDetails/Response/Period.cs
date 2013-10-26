using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.PlacesDetails.Response
{
    [DataContract]
    public class Period
    {
        [DataMember(Name = "open")]
        public TimeOfWeek OpenTime { get; set; }

        [DataMember(Name = "close")]
        public TimeOfWeek CloseTime { get; set; }
    }
}
