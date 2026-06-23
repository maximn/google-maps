using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.AirQuality.Request
{
    /// <summary>
    /// Colour palette used for the Universal Air Quality Index in the response (and in heatmap tiles).
    /// Supplied via <c>uaqiColorPalette</c>.
    /// </summary>
    public enum ColorPalette
    {
        /// <summary>No palette specified; the API picks a default.</summary>
        [EnumMember(Value = "COLOR_PALETTE_UNSPECIFIED")] Unspecified,

        /// <summary>Red-to-green palette.</summary>
        [EnumMember(Value = "RED_GREEN")] RedGreen,

        /// <summary>Indigo-to-Persian palette, dark theme.</summary>
        [EnumMember(Value = "INDIGO_PERSIAN_DARK")] IndigoPersianDark,

        /// <summary>Indigo-to-Persian palette, light theme.</summary>
        [EnumMember(Value = "INDIGO_PERSIAN_LIGHT")] IndigoPersianLight,
    }
}
