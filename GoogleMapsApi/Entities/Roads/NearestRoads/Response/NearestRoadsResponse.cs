using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Roads.NearestRoads.Request;
using GoogleMapsApi.Entities.Roads.SnapToRoads.Response;
using GoogleMapsApi.Entities.Roads.SpeedLimit.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.NearestRoads.Response
{
    public class NearestRoadsResponse : IResponseFor<NearestRoadsRequest>
    {
        [DataMember(Name = "snappedPoints")]
        public List<SnappedPoint> SnappedPoints { get; set; }
    }
}
