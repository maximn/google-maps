using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Common
{
    /// <summary>Price level of a place. Used both as a request filter and on response places.</summary>
    public enum PriceLevel
    {
        /// <summary>Price level unspecified or unknown.</summary>
        [EnumMember(Value = "PRICE_LEVEL_UNSPECIFIED")] Unspecified,

        /// <summary>Free.</summary>
        [EnumMember(Value = "PRICE_LEVEL_FREE")] Free,

        /// <summary>Inexpensive.</summary>
        [EnumMember(Value = "PRICE_LEVEL_INEXPENSIVE")] Inexpensive,

        /// <summary>Moderate.</summary>
        [EnumMember(Value = "PRICE_LEVEL_MODERATE")] Moderate,

        /// <summary>Expensive.</summary>
        [EnumMember(Value = "PRICE_LEVEL_EXPENSIVE")] Expensive,

        /// <summary>Very expensive.</summary>
        [EnumMember(Value = "PRICE_LEVEL_VERY_EXPENSIVE")] VeryExpensive,
    }
}
