using System;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.PlacesRadar.Response
{
	[DataContract]
	public class Result
	{
		[DataMember(Name = "id")]
        [Obsolete("Use place_id instead.  See https://developers.google.com/places/documentation/search#deprecation for more information.")]
		public string ID { get; set; }

		[DataMember(Name = "reference")]
        [Obsolete("Use place_id instead.  See https://developers.google.com/places/documentation/search#deprecation for more information.")]
		public string Reference { get; set; }	

		[DataMember( Name = "geometry" )]
		public Geometry Geometry { get; set; }

        [DataMember(Name = "place_id")]
        public string PlaceId { get; set; }
	}
}
