using GoogleMapsApi.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.NearestRoads.Request
{
    public class NearestRoadsRequest : SignableRequest
    {
        protected internal override string BaseUrl
        {
            get => "https://roads.googleapis.com/v1/nearestRoads"; 
        }

        public IEnumerable<Location> Points { get; set; }

        protected override QueryStringParametersList GetQueryStringParameters()
        {
            if (Points == null)
                throw new ArgumentException("Points must be specified, and both cannot be specified.");

            var parameters = base.GetQueryStringParameters();
            parameters.Add("points", string.Join("|", Points));
            return parameters;
        }
    }
}
