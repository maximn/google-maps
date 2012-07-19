using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Places.Response
{
    [DataContract]
    public class Event
    {
        [DataMember(Name = "event_id")]
        public string EventId { get; set; }

        [DataMember(Name = "summary")]
        public string Summary { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}