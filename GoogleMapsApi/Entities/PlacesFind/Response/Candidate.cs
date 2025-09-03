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
        public string FormattedAddress { get; set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("permanently_closed")]
        public bool? PermanentlyClosed { get; set; }

        [JsonPropertyName("photos")]
        public IEnumerable<Photo> Photos { get; set; }

        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; }

        [JsonPropertyName("plus_code")]
        public string PlusCode { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("types")]
        public string[] Types { get; set; }

        // contact fields
        /// <summary>
        /// Place Search returns only open_now; use a Place Details request to get the full opening_hours results.
        /// </summary>
        [JsonPropertyName("opening_hours")]
        public OpeningHours OpeningHours { get; set; }

        // atmosphere fields
        [JsonPropertyName("price_level")]
        public int? PriceLevel { get; set; }

        [JsonPropertyName("rating")]
        public double? Rating { get; set; }
    }
}
