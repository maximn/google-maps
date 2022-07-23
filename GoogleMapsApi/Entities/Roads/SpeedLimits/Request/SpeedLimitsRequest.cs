using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.StaticMaps.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.SpeedLimits.Request
{
	public class SpeedLimitsRequest : MapsBaseRequest
	{
		public enum SpeedUnit
		{
			KPH,
			MPH
		}
		
		protected internal override string BaseUrl
		{
			get
			{
				return "roads.googleapis.com/v1/speedLimits/";
			}
		}
		public IList<ILocationString> Path { get; set; }
		public string PlaceId { get; set; }
		SpeedUnit Unit { get; set; }

		protected override QueryStringParametersList GetQueryStringParameters()
		{
			if (Path == null && PlaceId==null)
				throw new ArgumentException("Either a path of PlaceId must be provided.");
			if (string.IsNullOrWhiteSpace(ApiKey))
				throw new ArgumentException("ApiKey must be provided");
			if(Path!=null & PlaceId!=null)
			{
				throw new ArgumentException("Only one of path or PlaceId must be provided.");
			}


			var parameters = base.GetQueryStringParameters();
			if (Path != null) parameters.Add("points", string.Join("|", Path));
			else if (PlaceId != null) parameters.Add("placeId", PlaceId);
			parameters.Add("unit", Unit.ToString());


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

