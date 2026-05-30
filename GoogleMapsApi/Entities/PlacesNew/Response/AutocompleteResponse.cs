using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNew.Request;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>Response from the Places API (New) Autocomplete endpoint.</summary>
    public sealed class AutocompleteResponse : IResponseFor<AutocompleteRequest>
    {
        /// <summary>Predicted suggestions, ordered from most to least relevant.</summary>
        [JsonPropertyName("suggestions")]
        public List<Suggestion>? Suggestions { get; set; }
    }
}
