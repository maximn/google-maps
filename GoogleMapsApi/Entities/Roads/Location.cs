using GoogleMapsApi.Entities.Common;
using System.Globalization;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Roads
{
    [DataContract]
	public class Location : ILocationString
	{
		[DataMember(Name = "latitude")]
		public double Latitude { get; set; }

		[DataMember(Name = "longitude")]
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
      var s = d.ToString(DoubleFormat, CultureInfo.InvariantCulture).TrimEnd('0');
      return s.Length == 0 ? "0.0" : s;
    }
        
	    private static readonly string DoubleFormat = "0." + new string('#', 339);
    }
}
