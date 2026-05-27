using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AddressValidation.Request
{
    /// <summary>
    /// Structured postal address sent to the Address Validation API or returned in a validated response.
    /// Mirrors Google's <c>google.type.PostalAddress</c>. Only <see cref="RegionCode"/> is strictly required;
    /// supply as many of the other fields as you have to improve validation quality.
    /// </summary>
    public sealed class PostalAddress
    {
        /// <summary>Schema revision of the PostalAddress. Leave unset to use the latest.</summary>
        [JsonPropertyName("revision")]
        public int? Revision { get; set; }

        /// <summary>
        /// CLDR region code of the country/region of the address (e.g. <c>"US"</c>, <c>"GB"</c>). Required.
        /// </summary>
        [JsonPropertyName("regionCode")]
        public string? RegionCode { get; set; }

        /// <summary>BCP-47 language code of the contents of the address (e.g. <c>"en-US"</c>).</summary>
        [JsonPropertyName("languageCode")]
        public string? LanguageCode { get; set; }

        /// <summary>Postal code or ZIP code.</summary>
        [JsonPropertyName("postalCode")]
        public string? PostalCode { get; set; }

        /// <summary>Additional country-specific sorting code (e.g. CEDEX in France).</summary>
        [JsonPropertyName("sortingCode")]
        public string? SortingCode { get; set; }

        /// <summary>Highest administrative subdivision (state, province, prefecture).</summary>
        [JsonPropertyName("administrativeArea")]
        public string? AdministrativeArea { get; set; }

        /// <summary>City or town.</summary>
        [JsonPropertyName("locality")]
        public string? Locality { get; set; }

        /// <summary>Sub-locality (district or neighborhood).</summary>
        [JsonPropertyName("sublocality")]
        public string? Sublocality { get; set; }

        /// <summary>
        /// Unstructured address lines (e.g. <c>"1600 Amphitheatre Pkwy"</c>). Provide at least one
        /// address line for meaningful validation.
        /// </summary>
        [JsonPropertyName("addressLines")]
        public List<string>? AddressLines { get; set; }

        /// <summary>Recipients of the address (e.g. names).</summary>
        [JsonPropertyName("recipients")]
        public List<string>? Recipients { get; set; }

        /// <summary>Organization at the address.</summary>
        [JsonPropertyName("organization")]
        public string? Organization { get; set; }
    }
}
