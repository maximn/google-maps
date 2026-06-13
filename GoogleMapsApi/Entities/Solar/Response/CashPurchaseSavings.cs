using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>Cost and savings of buying the solar installation outright with cash.</summary>
    public sealed class CashPurchaseSavings
    {
        /// <summary>Out-of-pocket cost before incentives.</summary>
        [JsonPropertyName("outOfPocketCost")]
        public Money? OutOfPocketCost { get; set; }

        /// <summary>Total upfront cost (out-of-pocket plus deferred incentives).</summary>
        [JsonPropertyName("upfrontCost")]
        public Money? UpfrontCost { get; set; }

        /// <summary>Total value of rebates.</summary>
        [JsonPropertyName("rebateValue")]
        public Money? RebateValue { get; set; }

        /// <summary>Number of years until the installation pays for itself.</summary>
        [JsonPropertyName("paybackYears")]
        public float PaybackYears { get; set; }

        /// <summary>Savings over time achieved by a cash purchase.</summary>
        [JsonPropertyName("savings")]
        public SavingsOverTime? Savings { get; set; }
    }
}
