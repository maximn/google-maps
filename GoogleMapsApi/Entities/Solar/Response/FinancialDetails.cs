using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>
    /// Financial details shared across all financing options for a given panel configuration.
    /// </summary>
    public sealed class FinancialDetails
    {
        /// <summary>Annual AC energy produced in the first year of operation, in kWh.</summary>
        [JsonPropertyName("initialAcKwhPerYear")]
        public float InitialAcKwhPerYear { get; set; }

        /// <summary>Utility bill still owed over the installation's lifetime, after solar.</summary>
        [JsonPropertyName("remainingLifetimeUtilityBill")]
        public Money? RemainingLifetimeUtilityBill { get; set; }

        /// <summary>Amount of federal tax incentive available.</summary>
        [JsonPropertyName("federalIncentive")]
        public Money? FederalIncentive { get; set; }

        /// <summary>Amount of state tax incentive available.</summary>
        [JsonPropertyName("stateIncentive")]
        public Money? StateIncentive { get; set; }

        /// <summary>Amount of utility incentive available.</summary>
        [JsonPropertyName("utilityIncentive")]
        public Money? UtilityIncentive { get; set; }

        /// <summary>Total value of all Solar Renewable Energy Credits over the lifetime.</summary>
        [JsonPropertyName("lifetimeSrecTotal")]
        public Money? LifetimeSrecTotal { get; set; }

        /// <summary>Total cost of electricity over the lifetime if no solar were installed.</summary>
        [JsonPropertyName("costOfElectricityWithoutSolar")]
        public Money? CostOfElectricityWithoutSolar { get; set; }

        /// <summary>Whether net metering is allowed.</summary>
        [JsonPropertyName("netMeteringAllowed")]
        public bool NetMeteringAllowed { get; set; }

        /// <summary>Percentage (0–100) of the consumer's energy use that solar supplies.</summary>
        [JsonPropertyName("solarPercentage")]
        public float SolarPercentage { get; set; }

        /// <summary>Percentage (0–100) of solar production exported to the grid.</summary>
        [JsonPropertyName("percentageExportedToGrid")]
        public float PercentageExportedToGrid { get; set; }
    }
}
