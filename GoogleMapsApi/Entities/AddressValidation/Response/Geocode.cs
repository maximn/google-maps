using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AddressValidation.Response
{
    /// <summary>Geocoding information for the validated address.</summary>
    public sealed class Geocode
    {
        /// <summary>Latitude / longitude of the address.</summary>
        [JsonPropertyName("location")]
        public LatLng? Location { get; set; }

        /// <summary>Plus code representation of the location.</summary>
        [JsonPropertyName("plusCode")]
        public PlusCode? PlusCode { get; set; }

        /// <summary>Bounding box that contains the address feature.</summary>
        [JsonPropertyName("bounds")]
        public Viewport? Bounds { get; set; }

        /// <summary>Diagonal size of the feature in meters; indicates how precise the location is.</summary>
        [JsonPropertyName("featureSizeMeters")]
        public double? FeatureSizeMeters { get; set; }

        /// <summary>Google Places <c>place_id</c> for the geocoded address.</summary>
        [JsonPropertyName("placeId")]
        public string? PlaceId { get; set; }

        /// <summary>Place type tags (e.g. <c>"street_address"</c>, <c>"premise"</c>).</summary>
        [JsonPropertyName("placeTypes")]
        public List<string>? PlaceTypes { get; set; }
    }

    /// <summary>Latitude/longitude pair in WGS84.</summary>
    public sealed class LatLng
    {
        /// <summary>Latitude in decimal degrees.</summary>
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        /// <summary>Longitude in decimal degrees.</summary>
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }

    /// <summary>Open Location Code representation.</summary>
    public sealed class PlusCode
    {
        /// <summary>Full global plus code.</summary>
        [JsonPropertyName("globalCode")]
        public string? GlobalCode { get; set; }

        /// <summary>Compound plus code (relative to a reference locality).</summary>
        [JsonPropertyName("compoundCode")]
        public string? CompoundCode { get; set; }
    }

    /// <summary>Latitude/longitude bounding box.</summary>
    public sealed class Viewport
    {
        /// <summary>Southwest corner of the box.</summary>
        [JsonPropertyName("low")]
        public LatLng? Low { get; set; }

        /// <summary>Northeast corner of the box.</summary>
        [JsonPropertyName("high")]
        public LatLng? High { get; set; }
    }
}
