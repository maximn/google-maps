using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>Cost and savings of buying the solar installation with a loan.</summary>
    public sealed class FinancedPurchaseSavings
    {
        /// <summary>Annual loan repayment amount.</summary>
        [JsonPropertyName("annualLoanPayment")]
        public Money? AnnualLoanPayment { get; set; }

        /// <summary>Total value of rebates.</summary>
        [JsonPropertyName("rebateValue")]
        public Money? RebateValue { get; set; }

        /// <summary>Loan annual percentage rate (APR) assumed, as a percentage.</summary>
        [JsonPropertyName("loanInterestRate")]
        public float LoanInterestRate { get; set; }

        /// <summary>Savings over time achieved by a financed purchase.</summary>
        [JsonPropertyName("savings")]
        public SavingsOverTime? Savings { get; set; }
    }
}
