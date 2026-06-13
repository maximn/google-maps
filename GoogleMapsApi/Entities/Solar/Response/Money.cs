using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>An amount of money with its currency type (mirrors <c>google.type.Money</c>).</summary>
    public sealed class Money
    {
        /// <summary>The three-letter ISO 4217 currency code (e.g. <c>"USD"</c>).</summary>
        [JsonPropertyName("currencyCode")]
        public string? CurrencyCode { get; set; }

        /// <summary>
        /// The whole units of the amount. The API encodes this int64 as a JSON string, so reading
        /// from a string is enabled here.
        /// </summary>
        [JsonPropertyName("units")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public long? Units { get; set; }

        /// <summary>
        /// Fractional part of the amount in nano (10^-9) units; sign matches <see cref="Units"/>.
        /// </summary>
        [JsonPropertyName("nanos")]
        public int Nanos { get; set; }
    }
}
