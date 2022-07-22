using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Roads.SnapToRoads.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.SnapToRoads.Response
{
    public class SnapToRoadsResposne : IResponseFor<SnapToRoadsRequest>
    {
        [DataMember(Name = "snappedPoints")]
        public List<SnappedPoint> SnappedPoints { get; set; }

        [DataMember(Name = "warningMessage")]
        public string WarningMessage { get; set; }
    }
}
