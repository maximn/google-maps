using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Pollen.Request
{
    /// <summary>
    /// The pollen heatmap type (which pollen the tile renders) for a
    /// <see cref="PollenHeatmapTileRequest"/>. Each maps the Universal Pollen Index for one pollen type.
    /// </summary>
    public enum PollenMapType
    {
        /// <summary>No map type specified. Ignored by the server if sent.</summary>
        [EnumMember(Value = "MAP_TYPE_UNSPECIFIED")] Unspecified,

        /// <summary>Tree pollen, Universal Pollen Index.</summary>
        [EnumMember(Value = "TREE_UPI")] TreeUpi,

        /// <summary>Grass pollen, Universal Pollen Index.</summary>
        [EnumMember(Value = "GRASS_UPI")] GrassUpi,

        /// <summary>Weed pollen, Universal Pollen Index.</summary>
        [EnumMember(Value = "WEED_UPI")] WeedUpi,
    }
}
