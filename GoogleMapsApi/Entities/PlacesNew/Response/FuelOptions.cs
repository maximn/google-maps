using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>Fuel options for a fuel station.</summary>
    public sealed class FuelOptions
    {
        /// <summary>The latest known fuel prices for each fuel type.</summary>
        [JsonPropertyName("fuelPrices")]
        public List<FuelPrice>? FuelPrices { get; set; }
    }

    /// <summary>The price of a single fuel type at a fuel station.</summary>
    public sealed class FuelPrice
    {
        /// <summary>Fuel type (e.g. <c>REGULAR_UNLEADED</c>).</summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>Price of the fuel.</summary>
        [JsonPropertyName("price")]
        public Money? Price { get; set; }

        /// <summary>RFC-3339 timestamp the price was last updated.</summary>
        [JsonPropertyName("updateTime")]
        public string? UpdateTime { get; set; }
    }
}
