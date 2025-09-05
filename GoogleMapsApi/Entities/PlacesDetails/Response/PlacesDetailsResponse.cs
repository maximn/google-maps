using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Entities.PlacesDetails.Response
{
    public class PlacesDetailsResponse : IResponseFor<PlacesDetailsRequest>
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
        [JsonPropertyName("result")]
        public Result Result { get; set; } = null!;
    }
}
