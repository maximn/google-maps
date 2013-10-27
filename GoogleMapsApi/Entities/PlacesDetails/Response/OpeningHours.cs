using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.PlacesDetails.Response
{
    [DataContract]
    public class OpeningHours
    {
        [DataMember(Name = "open_now")]
        public bool OpenNow { get; set; }

        [DataMember(Name = "periods")]
        public IEnumerable<Period> Periods { get; set; }
    }
}
