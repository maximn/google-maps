using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>An amount of money with its currency (Google's <c>google.type.Money</c>).</summary>
    public sealed class Money
    {
        /// <summary>ISO-4217 currency code.</summary>
        [JsonPropertyName("currencyCode")]
        public string? CurrencyCode { get; set; }

        /// <summary>Whole units of the amount (sent as a string by the API).</summary>
        [JsonPropertyName("units")]
        public string? Units { get; set; }

        /// <summary>Fractional part of the amount, in nanos (10^-9).</summary>
        [JsonPropertyName("nanos")]
        public int? Nanos { get; set; }
    }
}
