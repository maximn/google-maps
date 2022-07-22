using GoogleMapsApi.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.SnapToRoads.Request
{
    public class SnapToRoadsRequest : BaseRoadsRequest
    {
        protected internal override string BaseUrl
        {
             get => "roads.googleapis.com/v1/snapToRoads";
        }

        public IEnumerable<Location> Path { get; set; }

        public bool Interpolate { get; set; }

        protected override QueryStringParametersList GetQueryStringParameters()
        {
            if ((Path == null))
                throw new ArgumentException("Path must be specified, and both cannot be specified.");

            var parameters = base.GetQueryStringParameters();
            parameters.Add("path", string.Join("|", Path));
            parameters.Add("interpolate", Interpolate.ToString().ToLower());
            return parameters;
        }
    }
}
