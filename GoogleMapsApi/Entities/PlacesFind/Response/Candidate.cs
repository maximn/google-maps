using GoogleMapsApi.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.PlacesFind.Response
{
    public class Candidate
    {
        // basic fields
        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; } = null!;

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; } = null!;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = null!;

        [JsonPropertyName("id")]
        public string ID { get; set; } = null!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("permanently_closed")]
        public bool? PermanentlyClosed { get; set; }

        [JsonPropertyName("photos")]
        public IEnumerable<Photo> Photos { get; set; } = null!;

        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; } = null!;

        [JsonPropertyName("plus_code")]
        public string PlusCode { get; set; } = null!;

        [JsonPropertyName("scope")]
        public string Scope { get; set; } = null!;

        [JsonPropertyName("types")]
        public string[] Types { get; set; } = null!;

        // contact fields
        /// <summary>
        /// Place Search returns only open_now; use a Place Details request to get the full opening_hours results.
        /// </summary>
        [JsonPropertyName("opening_hours")]
        public OpeningHours OpeningHours { get; set; } = null!;

        // atmosphere fields
        [JsonPropertyName("price_level")]
        public int? PriceLevel { get; set; }

        [JsonPropertyName("rating")]
        public double? Rating { get; set; }
    }
}
