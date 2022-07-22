using GoogleMapsApi.Entities.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads.SnapToRoads.Response
{
    [DataContract]
    public class RoadsLocation 
	{
		[DataMember(Name = "latitude")]

		public double Latitude { get; set; }

		[DataMember(Name = "longitude")]
		public double Longitude { get; set; }

		public RoadsLocation(double lat, double lng)
		{
			Latitude = lat;
			Longitude = lng;
		}

		public string LocationString
		{
			get
			{
				return ToNonScientificString(Latitude) + "," + ToNonScientificString(Longitude);
			}
		}

		public override string ToString()
		{
			return LocationString;
		}

		private static string ToNonScientificString(double d)
		{
			var s = d.ToString(DoubleFormat, CultureInfo.InvariantCulture).TrimEnd('0');
			return s.Length == 0 ? "0.0" : s;
		}

		private static readonly string DoubleFormat = "0." + new string('#', 339);
	}
}
