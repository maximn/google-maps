using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Entities.Elevation.Response
{
	/// <summary>
	/// Response from the Google Elevation API containing elevation values for the requested locations or path.
	/// </summary>
	public class ElevationResponse : IResponseFor<ElevationRequest>
	{
		[JsonPropertyName("status")]
		[JsonConverter(typeof(EnumMemberJsonConverter<Status>))]
		public Status Status { get; set; }

		[JsonPropertyName("results")]
		public IEnumerable<Result>? Results { get; set; }


		public override string ToString()
		{
			return string.Format("ElevationResponse - Status: {0}, Results count: {1}", Status, Results != null ? Results.Count() : 0);
		}
	}
}
