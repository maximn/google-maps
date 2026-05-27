using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AddressValidation.Response
{
    /// <summary>
    /// High-level summary of how well the input address could be validated.
    /// </summary>
    public sealed class Verdict
    {
        /// <summary>Granularity of the input address as Google parsed it.</summary>
        [JsonPropertyName("inputGranularity")]
        public Granularity InputGranularity { get; set; }

        /// <summary>Granularity at which Google was able to fully validate the address.</summary>
        [JsonPropertyName("validationGranularity")]
        public Granularity ValidationGranularity { get; set; }

        /// <summary>Granularity at which the geocode for the address was determined.</summary>
        [JsonPropertyName("geocodeGranularity")]
        public Granularity GeocodeGranularity { get; set; }

        /// <summary>True if the address has no missing or unresolved components.</summary>
        [JsonPropertyName("addressComplete")]
        public bool AddressComplete { get; set; }

        /// <summary>True if any address component is unconfirmed.</summary>
        [JsonPropertyName("hasUnconfirmedComponents")]
        public bool HasUnconfirmedComponents { get; set; }

        /// <summary>True if Google inferred any address component (added information not in the input).</summary>
        [JsonPropertyName("hasInferredComponents")]
        public bool HasInferredComponents { get; set; }

        /// <summary>True if Google replaced any address component (e.g. corrected a misspelling or postal code).</summary>
        [JsonPropertyName("hasReplacedComponents")]
        public bool HasReplacedComponents { get; set; }
    }
}
