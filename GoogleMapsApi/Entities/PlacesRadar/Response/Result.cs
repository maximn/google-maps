using System;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesRadar.Response
{
	public class Result
	{
		[JsonPropertyName("id")]
        [Obsolete("Use place_id instead.  See https://developers.google.com/places/documentation/search#deprecation for more information.")]
		public string ID { get; set; }

		[JsonPropertyName("reference")]
        [Obsolete("Use place_id instead.  See https://developers.google.com/places/documentation/search#deprecation for more information.")]
		public string Reference { get; set; }	

		[JsonPropertyName("geometry")]
		public Geometry Geometry { get; set; }

        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; }
	}
}
