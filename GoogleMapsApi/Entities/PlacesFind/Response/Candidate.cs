using GoogleMapsApi.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.PlacesFind.Response
{
    [DataContract]
    public class Candidate
    {
        // basic fields
        [DataMember(Name = "formatted_address")]
        public string FormattedAddress { get; set; }

        [DataMember(Name = "geometry")]
        public Geometry Geometry { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "permanently_closed")]
        public bool? PermanentlyClosed { get; set; }

        [DataMember(Name = "photos")]
        public IEnumerable<Photo> Photos { get; set; }

        [DataMember(Name = "place_id")]
        public string PlaceId { get; set; }

        [DataMember(Name = "plus_code")]
        public string PlusCode { get; set; }

        [DataMember(Name = "scope")]
        public string Scope { get; set; }

        [DataMember(Name = "types")]
        public string[] Types { get; set; }

        // contact fields
        /// <summary>
        /// Place Search returns only open_now; use a Place Details request to get the full opening_hours results.
        /// </summary>
        [DataMember(Name = "opening_hours")]
        public OpeningHours OpeningHours { get; set; }

        // atmosphere fields
        [DataMember(Name = "price_level")]
        public int? PriceLevel { get; set; }

        [DataMember(Name = "rating")]
        public double? Rating { get; set; }
    }
}
