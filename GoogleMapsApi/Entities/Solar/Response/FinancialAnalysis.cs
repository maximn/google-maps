using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>
    /// Cost/benefit analysis of a particular panel configuration against a particular monthly bill.
    /// </summary>
    public sealed class FinancialAnalysis
    {
        /// <summary>The monthly utility bill this analysis assumes.</summary>
        [JsonPropertyName("monthlyBill")]
        public Money? MonthlyBill { get; set; }

        /// <summary>True if this is the analysis for the API's default monthly bill.</summary>
        [JsonPropertyName("defaultBill")]
        public bool DefaultBill { get; set; }

        /// <summary>Average energy consumption per month, in kWh, implied by the monthly bill.</summary>
        [JsonPropertyName("averageKwhPerMonth")]
        public float AverageKwhPerMonth { get; set; }

        /// <summary>
        /// Index into <c>SolarPotential.SolarPanelConfigs</c> of the configuration analysed, or
        /// -1 when no configuration is viable for the assumed bill.
        /// </summary>
        [JsonPropertyName("panelConfigIndex")]
        public int PanelConfigIndex { get; set; }

        /// <summary>Financial details common to every financing option below.</summary>
        [JsonPropertyName("financialDetails")]
        public FinancialDetails? FinancialDetails { get; set; }

        /// <summary>Savings from leasing the installation.</summary>
        [JsonPropertyName("leasingSavings")]
        public LeasingSavings? LeasingSavings { get; set; }

        /// <summary>Savings from purchasing the installation with cash.</summary>
        [JsonPropertyName("cashPurchaseSavings")]
        public CashPurchaseSavings? CashPurchaseSavings { get; set; }

        /// <summary>Savings from purchasing the installation with a loan.</summary>
        [JsonPropertyName("financedPurchaseSavings")]
        public FinancedPurchaseSavings? FinancedPurchaseSavings { get; set; }
    }
}
