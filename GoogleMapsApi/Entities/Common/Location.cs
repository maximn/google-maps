using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Common
{
	[DataContract]
	public class Location : ILocationString
	{
		[DataMember(Name = "lat")]
		public double Latitude { get; set; }

		[DataMember(Name = "lng")]
		public double Longitude { get; set; }

		public Location(double lat, double lng)
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
            return d.ToString(DoubleFormat).TrimEnd('0');
        }

        private static readonly string DoubleFormat = "0." + new string('#', 339);
    }
}
