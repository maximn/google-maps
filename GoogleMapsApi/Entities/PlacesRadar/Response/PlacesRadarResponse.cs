using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesRadar.Request;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Entities.PlacesRadar.Response
{
    [Obsolete("Radar search is deprecated since June 30 2018", true)]
	public class PlacesRadarResponse : IResponseFor<PlacesRadarRequest>
	{
		/// <summary>
		/// "status" contains metadata on the request.
		/// </summary>
		[JsonPropertyName("status")]
		[JsonConverter(typeof(EnumMemberJsonConverter<Status>))]
		public Status Status { get; set; }

		/// <summary>
		/// "results" contains an array of places, with information about the place. See Place Search Results for information about these results. The Places API returns up to 20 establishment results. Additionally, political results may be returned which serve to identify the area of the request.
		/// </summary>
		[JsonPropertyName("results")]
		public IEnumerable<Result> Results { get; set; }
	}
}
