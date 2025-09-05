using System;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesRadar.Response
{
	public class Result
	{
		[JsonPropertyName("id")]
        [Obsolete("Use place_id instead.  See https://developers.google.com/places/documentation/search#deprecation for more information.")]
		public string ID { get; set; } = null!;

		[JsonPropertyName("reference")]
        [Obsolete("Use place_id instead.  See https://developers.google.com/places/documentation/search#deprecation for more information.")]
		public string Reference { get; set; } = null!;

		[JsonPropertyName("geometry")]
		public Geometry Geometry { get; set; } = null!;

        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; } = null!;
	}
}
