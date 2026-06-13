using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>Cost and savings of leasing the solar installation.</summary>
    public sealed class LeasingSavings
    {
        /// <summary>Whether leasing is allowed in this jurisdiction.</summary>
        [JsonPropertyName("leasesAllowed")]
        public bool LeasesAllowed { get; set; }

        /// <summary>Whether leasing is supported by the Solar API for this region.</summary>
        [JsonPropertyName("leasesSupported")]
        public bool LeasesSupported { get; set; }

        /// <summary>Estimated annual leasing cost.</summary>
        [JsonPropertyName("annualLeasingCost")]
        public Money? AnnualLeasingCost { get; set; }

        /// <summary>Savings over time achieved by leasing.</summary>
        [JsonPropertyName("savings")]
        public SavingsOverTime? Savings { get; set; }
    }
}
