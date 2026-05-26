using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AddressValidation.Response
{
    /// <summary>
    /// Higher-level metadata flags describing the type of address (business, PO box, residence).
    /// Each flag is nullable — null means Google has no information either way.
    /// </summary>
    public sealed class AddressMetadata
    {
        /// <summary>True if Google identifies the address as a business location.</summary>
        [JsonPropertyName("business")]
        public bool? Business { get; set; }

        /// <summary>True if Google identifies the address as a PO box or virtual mailbox.</summary>
        [JsonPropertyName("poBox")]
        public bool? PoBox { get; set; }

        /// <summary>True if Google identifies the address as a residence.</summary>
        [JsonPropertyName("residential")]
        public bool? Residential { get; set; }
    }
}
