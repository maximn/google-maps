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
	public class SnapToRoadRequest:MapsBaseRequest
	{
		
		protected internal override string BaseUrl
		{
			get
			{
				return "roads.googleapis.com/v1/snapToRoads/";
			}
		}
		public Path Path { get; set; }
		public bool Interpolate { get; set; }
		protected override QueryStringParametersList GetQueryStringParameters()
		{
			if (Path == null)
				throw new ArgumentException("Path must be provided.");
			if (string.IsNullOrWhiteSpace(ApiKey))
				throw new ArgumentException("ApiKey must be provided");

			var parameters = base.GetQueryStringParameters();
			parameters.Add("path", string.Join("|", Path.Locations));
			parameters.Add("interpolate", Interpolate.ToString());


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

