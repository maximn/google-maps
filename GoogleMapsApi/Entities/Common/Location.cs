using System;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Common
{
    [DataContract]
    public class Location : ILocation
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

    	public string LocationString
    	{
				get { return string.Format("{0},{1}", this.Latitude, this.Longitude); }
    	}
    }
}
