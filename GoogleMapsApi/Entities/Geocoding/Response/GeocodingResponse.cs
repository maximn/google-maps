using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Entities.Geocoding.Response
{
	/// <summary>
	/// Response from the Google Geocoding API containing geocoded results matching the input address or coordinates.
	/// </summary>
	public class GeocodingResponse : IResponseFor<GeocodingRequest>
	{
		[JsonPropertyName("status")]
		[JsonConverter(typeof(EnumMemberJsonConverter<Status>))]
		public Status Status { get; set; }

		[JsonPropertyName("results")]
		public IEnumerable<Result>? Results { get; set; }

		public override string ToString()
		{
			return string.Format("GeocodingResponse - Status: {0}, Results count: {1}", Status, Results != null ? Results.Count() : 0);
		}
	}
}