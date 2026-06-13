using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>How a panel configuration distributes panels across one roof segment.</summary>
    public sealed class RoofSegmentSummary
    {
        /// <summary>Angle of the roof segment relative to the horizontal, in degrees.</summary>
        [JsonPropertyName("pitchDegrees")]
        public float PitchDegrees { get; set; }

        /// <summary>Compass direction the roof segment faces, in degrees (0 = north, 90 = east).</summary>
        [JsonPropertyName("azimuthDegrees")]
        public float AzimuthDegrees { get; set; }

        /// <summary>Number of panels placed on this segment in this configuration.</summary>
        [JsonPropertyName("panelsCount")]
        public int PanelsCount { get; set; }

        /// <summary>Yearly energy these panels would produce, in DC kWh.</summary>
        [JsonPropertyName("yearlyEnergyDcKwh")]
        public float YearlyEnergyDcKwh { get; set; }

        /// <summary>Index in <c>SolarPotential.RoofSegmentStats</c> of the segment this summary describes.</summary>
        [JsonPropertyName("segmentIndex")]
        public int SegmentIndex { get; set; }
    }
}
