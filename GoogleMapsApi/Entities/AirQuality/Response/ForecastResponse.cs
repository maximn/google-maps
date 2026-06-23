using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.AirQuality.Request;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.AirQuality.Response
{
    /// <summary>Response from the Air Quality API <c>forecast:lookup</c> endpoint.</summary>
    public sealed class ForecastResponse : IResponseFor<ForecastRequest>
    {
        /// <summary>The forecast hours requested, one entry per hour.</summary>
        [JsonPropertyName("hourlyForecasts")]
        public List<HourlyForecast>? HourlyForecasts { get; set; }

        /// <summary>Region code (ISO 3166-1 alpha-2) of the location.</summary>
        [JsonPropertyName("regionCode")]
        public string? RegionCode { get; set; }

        /// <summary>Token to pass as <c>PageToken</c> on a follow-up request to fetch the next page, if any.</summary>
        [JsonPropertyName("nextPageToken")]
        public string? NextPageToken { get; set; }
    }
}
