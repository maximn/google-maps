using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNearBy.Request;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Entities.PlacesNearBy.Response
{
	public class PlacesNearByResponse : IResponseFor<PlacesNearByRequest>
	{
		/// <summary>
		/// "status" contains metadata on the request.
		/// </summary>
		[JsonPropertyName("status")]
		[JsonConverter(typeof(EnumMemberJsonConverter<Status>))]
		public Status Status { get; set; }

        /// <summary>
        /// If there is a next page of results, the token will be supplied by Google. Token should be set on the next PlacesRequest object to get the next page of results from Google.
        /// If null is returned, there is no next page of results.
        /// </summary>
        [JsonPropertyName("next_page_token")]
        public string NextPage
        {
            get;
            set;
        }

		/// <summary>
		/// "results" contains an array of places, with information about the place. See Place Search Results for information about these results. The Places API returns up to 20 establishment results. Additionally, political results may be returned which serve to identify the area of the request.
		/// </summary>
		[JsonPropertyName("results")]
		public IEnumerable<Result> Results { get; set; }
	}
}
