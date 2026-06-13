using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>Solar potential of a building: how much energy it could generate and at what cost.</summary>
    public sealed class SolarPotential
    {
        /// <summary>Maximum number of panels that can fit on the roof.</summary>
        [JsonPropertyName("maxArrayPanelsCount")]
        public int MaxArrayPanelsCount { get; set; }

        /// <summary>Maximum roof area covered by panels, in square meters.</summary>
        [JsonPropertyName("maxArrayAreaMeters2")]
        public float MaxArrayAreaMeters2 { get; set; }

        /// <summary>Maximum yearly sunshine on any point of the roof, in hours.</summary>
        [JsonPropertyName("maxSunshineHoursPerYear")]
        public float MaxSunshineHoursPerYear { get; set; }

        /// <summary>Equivalent grid carbon intensity used for offset calculations, in kg CO2e per MWh.</summary>
        [JsonPropertyName("carbonOffsetFactorKgPerMwh")]
        public float CarbonOffsetFactorKgPerMwh { get; set; }

        /// <summary>Size and sunshine statistics over the entire roof.</summary>
        [JsonPropertyName("wholeRoofStats")]
        public SizeAndSunshineStats? WholeRoofStats { get; set; }

        /// <summary>Size and sunshine statistics over the whole building footprint.</summary>
        [JsonPropertyName("buildingStats")]
        public SizeAndSunshineStats? BuildingStats { get; set; }

        /// <summary>Per-segment size and sunshine statistics, referenced by panel <c>SegmentIndex</c>.</summary>
        [JsonPropertyName("roofSegmentStats")]
        public List<RoofSegmentSizeAndSunshineStats>? RoofSegmentStats { get; set; }

        /// <summary>Every individual panel position considered, ordered by descending production.</summary>
        [JsonPropertyName("solarPanels")]
        public List<SolarPanel>? SolarPanels { get; set; }

        /// <summary>Candidate layouts of increasing panel count, ordered by descending production.</summary>
        [JsonPropertyName("solarPanelConfigs")]
        public List<SolarPanelConfig>? SolarPanelConfigs { get; set; }

        /// <summary>Financial analyses for a range of assumed monthly bills.</summary>
        [JsonPropertyName("financialAnalyses")]
        public List<FinancialAnalysis>? FinancialAnalyses { get; set; }

        /// <summary>Capacity of each panel assumed in the calculations, in watts.</summary>
        [JsonPropertyName("panelCapacityWatts")]
        public float PanelCapacityWatts { get; set; }

        /// <summary>Height of each panel assumed in the calculations, in meters.</summary>
        [JsonPropertyName("panelHeightMeters")]
        public float PanelHeightMeters { get; set; }

        /// <summary>Width of each panel assumed in the calculations, in meters.</summary>
        [JsonPropertyName("panelWidthMeters")]
        public float PanelWidthMeters { get; set; }

        /// <summary>Assumed operational lifetime of the panels, in years.</summary>
        [JsonPropertyName("panelLifetimeYears")]
        public int PanelLifetimeYears { get; set; }
    }
}
