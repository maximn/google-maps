using GoogleMapsApi.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.SnapToRoads.Response
{
    public class SnappedPoint
    {
        [DataMember(Name = "location")]
        public RoadsLocation Location { get; set; }

        [DataMember(Name = "placeId")]
        public string PlaceId { get; set; }

        [DataMember(Name = "originalIndex")]
        public long OriginalIndex { get; set; }
    }
}
