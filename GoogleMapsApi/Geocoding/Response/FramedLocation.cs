using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using GoogleMapsApi.Common;

namespace GoogleMapsApi.Geocoding.Response
{
    [DataContract]
    public class FramedLocation
    {
        [DataMember(Name="southwest")]
        public Location SouthWest { get; set; }

        [DataMember(Name = "northeast")]
        public Location NorthEast { get; set; }
    }
}
