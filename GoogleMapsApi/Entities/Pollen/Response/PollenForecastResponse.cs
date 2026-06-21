using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Pollen.Request;

namespace GoogleMapsApi.Entities.Pollen.Response
{
    /// <summary>Response from the Pollen API <c>forecast:lookup</c> endpoint.</summary>
    public sealed class PollenForecastResponse : IResponseFor<PollenForecastRequest>
    {
        /// <summary>Region code (ISO 3166-1 alpha-2) of the location.</summary>
        [JsonPropertyName("regionCode")]
        public string? RegionCode { get; set; }

        /// <summary>The daily forecasts, one entry per requested day.</summary>
        [JsonPropertyName("dailyInfo")]
        public List<DayInfo>? DailyInfo { get; set; }

        /// <summary>Token to pass as <c>PageToken</c> on a follow-up request to fetch the next page, if any.</summary>
        [JsonPropertyName("nextPageToken")]
        public string? NextPageToken { get; set; }
    }
}
