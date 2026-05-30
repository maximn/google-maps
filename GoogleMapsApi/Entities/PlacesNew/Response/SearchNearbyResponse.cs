using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNew.Request;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>Response from the Places API (New) Nearby Search endpoint.</summary>
    public sealed class SearchNearbyResponse : IResponseFor<SearchNearbyRequest>
    {
        /// <summary>Matching places. Which fields are populated depends on the request field mask.</summary>
        [JsonPropertyName("places")]
        public List<Place>? Places { get; set; }
    }
}
