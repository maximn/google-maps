using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.PlacesText.Response
{
    public class Result
    {
        /// <summary>
        /// name contains the human-readable name for the returned result. For establishment results, this is usually the canonicalized business name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = null!;

        [JsonPropertyName("id")]
        [Obsolete("Use place_id instead.  See https://developers.google.com/places/documentation/search#deprecation for more information.")]
        public string ID { get; set; } = null!;

        [JsonPropertyName("reference")]
        [Obsolete("Use place_id instead.  See https://developers.google.com/places/documentation/search#deprecation for more information.")]
        public string Reference { get; set; } = null!;

        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; } = null!;

        [JsonPropertyName("types")]
        public string[] Types { get; set; } = null!;

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; } = null!;

        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; } = null!;

        [JsonPropertyName("photos")]
        public IEnumerable<Photo> Photos { get; set; } = null!;
    }
}
