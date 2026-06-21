using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AirQuality.Request
{
    /// <summary>
    /// Pairs a country/region with the local Air Quality Index to report for it, overriding the API's
    /// default index for that region. Supplied via <c>customLocalAqis</c>.
    /// </summary>
    public sealed class CustomLocalAqi
    {
        /// <summary>The country/region this override applies to (ISO 3166-1 alpha-2 code).</summary>
        [JsonPropertyName("regionCode")]
        public string? RegionCode { get; set; }

        /// <summary>The AQI to use for the region (e.g. <c>"usa_epa"</c>).</summary>
        [JsonPropertyName("aqi")]
        public string? Aqi { get; set; }
    }
}
