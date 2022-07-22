using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Roads.SnapToRoads.Response;
using GoogleMapsApi.Entities.Roads.SpeedLimit.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.SpeedLimit.Response
{
    public class SpeedLimitResponse : IResponseFor<SpeedLimitRequest>
    {
        [DataMember(Name = "speedLimits")]
        public List<SpeedLimitModel> SpeedLimits { get; set; }

        [DataMember(Name = "snappedPoints")]
        public List<SnappedPoint> SnappedPoints { get; set; }

        [DataMember(Name = "warningMessage")]
        public string WarningMessage { get; set; }
    }
}
