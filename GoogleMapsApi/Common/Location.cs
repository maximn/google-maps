using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Common
{
    [DataContract]
    public class Location
    {
        [DataMember(Name="lat")]
        public double Latitude { get; set; }

        [DataMember(Name = "lng")]
        public double Longitude { get; set; }

				public Location(double lat, double lng)
				{
					Latitude = lat;
					Longitude = lng;
				}
    }
}
