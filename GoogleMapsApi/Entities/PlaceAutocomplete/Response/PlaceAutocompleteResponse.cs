using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Entities.PlaceAutocomplete.Response
{
    public class PlaceAutocompleteResponse : IResponseFor<PlaceAutocompleteRequest>
	{
		/// <summary>
		/// "status" contains metadata on the request.
		/// </summary>
		[JsonPropertyName("status")]
		[JsonConverter(typeof(EnumMemberJsonConverter<Status>))]
		public Status Status { get; set; }

		/// <summary>
		/// "results" contains an array of predictions rather than full results, each including a description and a reference which can be queried further
        /// to get the full place details
		/// </summary>
		[JsonPropertyName("predictions")]
		public IEnumerable<Prediction> Results { get; set; } = null!;
	}
}
