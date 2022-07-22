using GoogleMapsApi.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.SpeedLimit.Request
{
    public class SpeedLimitRequest : SignableRequest
    {
        protected internal override string BaseUrl
        {
			get => "https://roads.googleapis.com/v1/speedlimits";
        }

		public IEnumerable<Location> Locations { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<Location> Path { get; set; }

		protected override QueryStringParametersList GetQueryStringParameters()
		{
			if ((Locations == null) == (Path == null))
				throw new ArgumentException("Either Locations or Path must be specified, and both cannot be specified.");

			var parameters = base.GetQueryStringParameters();
			parameters.Add(Locations != null ? "locations" : "path", string.Join("|", Locations ?? Path));

			return parameters;
		}
    }
}
