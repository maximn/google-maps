using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNew.Request;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>Response from the Places API (New) Text Search endpoint.</summary>
    public sealed class SearchTextResponse : IResponseFor<SearchTextRequest>
    {
        /// <summary>Matching places. Which fields are populated depends on the request field mask.</summary>
        [JsonPropertyName("places")]
        public List<Place>? Places { get; set; }

        /// <summary>Token to fetch the next page of results, if any.</summary>
        [JsonPropertyName("nextPageToken")]
        public string? NextPageToken { get; set; }

        /// <summary>
        /// Per-place routing summaries, populated only when routing parameters were requested.
        /// Modeled shallowly; inspect the raw JSON for full detail.
        /// </summary>
        [JsonPropertyName("routingSummaries")]
        public List<object>? RoutingSummaries { get; set; }
    }
}
