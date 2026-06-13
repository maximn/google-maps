using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>A single solar panel in a potential layout.</summary>
    public sealed class SolarPanel
    {
        /// <summary>The centre of the panel.</summary>
        [JsonPropertyName("center")]
        public LatLng? Center { get; set; }

        /// <summary>The orientation of the panel.</summary>
        [JsonPropertyName("orientation")]
        public SolarPanelOrientation Orientation { get; set; }

        /// <summary>How much energy this panel would produce per year, in DC kWh.</summary>
        [JsonPropertyName("yearlyEnergyDcKwh")]
        public float YearlyEnergyDcKwh { get; set; }

        /// <summary>Index in <c>SolarPotential.RoofSegmentStats</c> of the segment this panel sits on.</summary>
        [JsonPropertyName("segmentIndex")]
        public int SegmentIndex { get; set; }
    }
}
