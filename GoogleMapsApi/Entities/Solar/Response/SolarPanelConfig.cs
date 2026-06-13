using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>
    /// A potential layout of a given number of panels, ordered by descending energy production.
    /// </summary>
    public sealed class SolarPanelConfig
    {
        /// <summary>Total number of panels in this configuration.</summary>
        [JsonPropertyName("panelsCount")]
        public int PanelsCount { get; set; }

        /// <summary>Total yearly energy this configuration would produce, in DC kWh.</summary>
        [JsonPropertyName("yearlyEnergyDcKwh")]
        public float YearlyEnergyDcKwh { get; set; }

        /// <summary>Per-roof-segment breakdown of where the panels are placed.</summary>
        [JsonPropertyName("roofSegmentSummaries")]
        public List<RoofSegmentSummary>? RoofSegmentSummaries { get; set; }
    }
}
