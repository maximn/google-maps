using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AddressValidation.Response
{
    /// <summary>
    /// The validation result for a single address: verdict summary, standardized address,
    /// geocode, metadata, and (for US/PR addresses with USPS CASS enabled) USPS-processed data.
    /// </summary>
    public sealed class ValidationResult
    {
        /// <summary>High-level summary of how well the address was validated.</summary>
        [JsonPropertyName("verdict")]
        public Verdict? Verdict { get; set; }

        /// <summary>Validated, standardized form of the address.</summary>
        [JsonPropertyName("address")]
        public ValidatedAddress? Address { get; set; }

        /// <summary>Geocoding information (lat/lng, place ID, plus code) for the address.</summary>
        [JsonPropertyName("geocode")]
        public Geocode? Geocode { get; set; }

        /// <summary>Address type metadata (business / PO box / residential).</summary>
        [JsonPropertyName("metadata")]
        public AddressMetadata? Metadata { get; set; }

        /// <summary>USPS CASS-processed data. Populated only for US/PR with USPS CASS enabled.</summary>
        [JsonPropertyName("uspsData")]
        public UspsData? UspsData { get; set; }

        /// <summary>
        /// English/Latin-script form of the validated address. Populated only when
        /// <c>LanguageOptions.ReturnEnglishLatinAddress</c> was set on the request.
        /// </summary>
        [JsonPropertyName("englishLatinAddress")]
        public ValidatedAddress? EnglishLatinAddress { get; set; }
    }
}
