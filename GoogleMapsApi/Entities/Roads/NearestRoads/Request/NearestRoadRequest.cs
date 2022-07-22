using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.StaticMaps.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.SnapToRoad.Request
{
	public class NearestRoadsRequest:MapsBaseRequest
	{
		
		protected internal override string BaseUrl
		{
			get
			{
				return "roads.googleapis.com/v1/nearestRoads/";
			}
		}
		public IList<ILocationString> Points { get; set; }
		
		protected override QueryStringParametersList GetQueryStringParameters()
		{
			if (Points == null)
				throw new ArgumentException("Points must be provided.");
			if (string.IsNullOrWhiteSpace(ApiKey))
				throw new ArgumentException("ApiKey must be provided");

			var parameters = base.GetQueryStringParameters();
			parameters.Add("points", string.Join("|", Points));


			return parameters;
		}
		public override Uri GetUri()
		{
			string scheme = IsSSL ? "https://" : "http://";

			var queryString = GetQueryStringParameters().GetQueryStringPostfix();
			return new Uri(scheme + BaseUrl + "?" + queryString);
		}
		
	}
}

