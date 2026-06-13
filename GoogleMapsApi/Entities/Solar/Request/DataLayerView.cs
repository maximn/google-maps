using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Solar.Request
{
    /// <summary>Which subset of data layers the <c>dataLayers:get</c> endpoint should return.</summary>
    public enum DataLayerView
    {
        /// <summary>Equivalent to <see cref="FullLayers"/>.</summary>
        [EnumMember(Value = "DATA_LAYER_VIEW_UNSPECIFIED")] Unspecified,

        /// <summary>Digital surface model (DSM) only.</summary>
        [EnumMember(Value = "DSM_LAYER")] DsmLayer,

        /// <summary>DSM, RGB and mask.</summary>
        [EnumMember(Value = "IMAGERY_LAYERS")] ImageryLayers,

        /// <summary>Imagery layers plus the annual flux map.</summary>
        [EnumMember(Value = "IMAGERY_AND_ANNUAL_FLUX_LAYERS")] ImageryAndAnnualFluxLayers,

        /// <summary>Imagery layers plus annual and monthly flux maps.</summary>
        [EnumMember(Value = "IMAGERY_AND_ALL_FLUX_LAYERS")] ImageryAndAllFluxLayers,

        /// <summary>Every available layer, including hourly shade.</summary>
        [EnumMember(Value = "FULL_LAYERS")] FullLayers,
    }
}
