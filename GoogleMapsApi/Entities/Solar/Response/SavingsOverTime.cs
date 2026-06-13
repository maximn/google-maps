using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>Financial savings a solar installation yields over various time horizons.</summary>
    public sealed class SavingsOverTime
    {
        /// <summary>Utility-bill savings in the first year.</summary>
        [JsonPropertyName("savingsYear1")]
        public Money? SavingsYear1 { get; set; }

        /// <summary>Total utility-bill savings over the first twenty years.</summary>
        [JsonPropertyName("savingsYear20")]
        public Money? SavingsYear20 { get; set; }

        /// <summary>Present value of the twenty-year savings.</summary>
        [JsonPropertyName("presentValueOfSavingsYear20")]
        public Money? PresentValueOfSavingsYear20 { get; set; }

        /// <summary>Total utility-bill savings over the installation's lifetime.</summary>
        [JsonPropertyName("savingsLifetime")]
        public Money? SavingsLifetime { get; set; }

        /// <summary>Present value of the lifetime savings.</summary>
        [JsonPropertyName("presentValueOfSavingsLifetime")]
        public Money? PresentValueOfSavingsLifetime { get; set; }

        /// <summary>Whether the financing option is considered financially viable.</summary>
        [JsonPropertyName("financiallyViable")]
        public bool FinanciallyViable { get; set; }
    }
}
