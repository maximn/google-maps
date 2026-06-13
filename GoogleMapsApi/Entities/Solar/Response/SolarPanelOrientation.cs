using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>Orientation of a solar panel relative to its roof segment.</summary>
    public enum SolarPanelOrientation
    {
        /// <summary>No orientation specified.</summary>
        [EnumMember(Value = "SOLAR_PANEL_ORIENTATION_UNSPECIFIED")] Unspecified,

        /// <summary>The panel's long side runs horizontally (east–west).</summary>
        [EnumMember(Value = "LANDSCAPE")] Landscape,

        /// <summary>The panel's long side runs vertically (north–south).</summary>
        [EnumMember(Value = "PORTRAIT")] Portrait,
    }
}
