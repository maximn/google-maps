using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Entities.Directions.Response
{
	public class DirectionsResponse : IResponseFor<DirectionsRequest>
	{
		/// <summary>
		/// Error message received from the server when the call fails
		/// </summary>
		/// <value>Error message</value>
		[JsonPropertyName("error_message")]
		public string ErrorMessage { get; set; } = null!;

		/// <summary>
		/// "status" contains metadata on the request. See Status Codes below.
		/// </summary>
		[JsonPropertyName("status")]
		[JsonConverter(typeof(EnumMemberJsonConverter<DirectionsStatusCodes>))]
		public DirectionsStatusCodes Status { get; set; }

		/// <summary>
		/// "routes" contains an array of routes from the origin to the destination. See Routes below.
		/// </summary>
		[JsonPropertyName("routes")]
		public IEnumerable<Route>? Routes { get; set; }


		public override string ToString()
		{
			return string.Format("DirectionsResponse - Status: {0}, Results count: {1}", Status, Routes != null ? Routes.Count() : 0);
		}
	}
}
