using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.AddressValidation.Request;

namespace GoogleMapsApi.Entities.AddressValidation.Response
{
    /// <summary>
    /// The address as Google understands and standardizes it after validation. Includes the
    /// formatted single-line form, a structured <see cref="PostalAddress"/>, and per-component
    /// validation flags.
    /// </summary>
    public sealed class ValidatedAddress
    {
        /// <summary>Single-line formatted address.</summary>
        [JsonPropertyName("formattedAddress")]
        public string? FormattedAddress { get; set; }

        /// <summary>Structured form of the validated address.</summary>
        [JsonPropertyName("postalAddress")]
        public PostalAddress? PostalAddress { get; set; }

        /// <summary>Per-component breakdown including confirmation and transformation flags.</summary>
        [JsonPropertyName("addressComponents")]
        public List<AddressComponent>? AddressComponents { get; set; }

        /// <summary>Component types that were missing from the input.</summary>
        [JsonPropertyName("missingComponentTypes")]
        public List<string>? MissingComponentTypes { get; set; }

        /// <summary>Component types that exist but could not be confirmed.</summary>
        [JsonPropertyName("unconfirmedComponentTypes")]
        public List<string>? UnconfirmedComponentTypes { get; set; }

        /// <summary>Tokens from the input that could not be matched to any component.</summary>
        [JsonPropertyName("unresolvedTokens")]
        public List<string>? UnresolvedTokens { get; set; }
    }
}
