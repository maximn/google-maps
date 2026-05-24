using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesFind.Request;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Entities.PlacesFind.Response
{
    /// <summary>
    /// Response from the Google Find Place API containing candidate places matching the input text.
    /// </summary>
    public class PlacesFindResponse : IResponseFor<PlacesFindRequest>
    {
        [JsonPropertyName("status")]
        [JsonConverter(typeof(EnumMemberJsonConverter<Status>))]
        public Status Status { get; set; }

        /// <summary>
        /// Collection of places. Each result contains only the data types that were specified using the fields parameter, plus html_attributions.
        /// </summary>
        [JsonPropertyName("candidates")]
        public IEnumerable<Candidate> Candidates { get; set; } = null!;
    }
}
