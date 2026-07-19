using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.AirQuality.Request
{
    /// <summary>
    /// Request for the Air Quality API <c>forecast:lookup</c> endpoint
    /// (<c>POST https://airquality.googleapis.com/v1/forecast:lookup</c>). Returns projected hourly
    /// air-quality data for a coordinate over a time range.
    /// </summary>
    /// <remarks>
    /// Specify the time range with either <see cref="DateTime"/> (a single hour) or <see cref="Period"/>
    /// (a range) — not both. The Air Quality API is billable; calls beyond the free tier incur charges.
    /// </remarks>
    public sealed class ForecastRequest : MapsBaseRequest
    {
        /// <summary>Latitude of the query point, in degrees. Required.</summary>
        public double Latitude { get; set; }

        /// <summary>Longitude of the query point, in degrees. Required.</summary>
        public double Longitude { get; set; }

        /// <summary>A single hour to forecast (rounded down to the hour). Mutually exclusive with <see cref="Period"/>.</summary>
        public DateTimeOffset? DateTime { get; set; }

        /// <summary>A time range to forecast. Mutually exclusive with <see cref="DateTime"/>.</summary>
        public Interval? Period { get; set; }

        /// <summary>Maximum number of hourly records per page. Defaults to 24 server-side when unset.</summary>
        public int? PageSize { get; set; }

        /// <summary>Page token from a previous response's <c>NextPageToken</c> to fetch the next page.</summary>
        public string? PageToken { get; set; }

        /// <summary>Optional extra computations that enrich the response (local AQIs, health advice, pollutant detail).</summary>
        public List<ExtraComputation>? ExtraComputations { get; set; }

        /// <summary>Colour palette used for the Universal AQI in the response.</summary>
        public ColorPalette? UaqiColorPalette { get; set; }

        /// <summary>Per-region overrides selecting which local AQI to report.</summary>
        public List<CustomLocalAqi>? CustomLocalAqis { get; set; }

        /// <summary>Whether to include the Universal AQI in the response. Defaults to true server-side when unset.</summary>
        public bool? UniversalAqi { get; set; }

        /// <summary>IETF BCP-47 language code for textual fields in the response (e.g. <c>"en"</c>).</summary>
        public string? LanguageCode { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException("ApiKey is required for the Air Quality API.");

            return new Uri(
                "https://airquality.googleapis.com/v1/forecast:lookup" +
                $"?key={Uri.EscapeDataString(ApiKey!)}");
        }

        /// <inheritdoc/>
        protected internal override HttpContent? GetRequestBody()
        {
            if (DateTime.HasValue && Period != null)
                throw new InvalidOperationException("DateTime and Period are mutually exclusive on a forecast request.");

            var payload = new Payload
            {
                Location = new LatLng { Latitude = Latitude, Longitude = Longitude },
                DateTime = DateTime.HasValue ? AirQualityJson.Rfc3339(DateTime.Value) : null,
                Period = Period == null ? null : new IntervalPayload
                {
                    StartTime = Period.StartTime.HasValue ? AirQualityJson.Rfc3339(Period.StartTime.Value) : null,
                    EndTime = Period.EndTime.HasValue ? AirQualityJson.Rfc3339(Period.EndTime.Value) : null,
                },
                PageSize = PageSize,
                PageToken = string.IsNullOrWhiteSpace(PageToken) ? null : PageToken,
                ExtraComputations = ExtraComputations is { Count: > 0 } ? ExtraComputations : null,
                UaqiColorPalette = UaqiColorPalette,
                CustomLocalAqis = CustomLocalAqis is { Count: > 0 } ? CustomLocalAqis : null,
                UniversalAqi = UniversalAqi,
                LanguageCode = string.IsNullOrWhiteSpace(LanguageCode) ? null : LanguageCode,
            };
            var json = JsonSerializer.Serialize(payload, AirQualityJson.BodyOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private sealed class Payload
        {
            [JsonPropertyName("location")] public LatLng? Location { get; set; }
            [JsonPropertyName("dateTime")] public string? DateTime { get; set; }
            [JsonPropertyName("period")] public IntervalPayload? Period { get; set; }
            [JsonPropertyName("pageSize")] public int? PageSize { get; set; }
            [JsonPropertyName("pageToken")] public string? PageToken { get; set; }
            [JsonPropertyName("extraComputations")] public List<ExtraComputation>? ExtraComputations { get; set; }
            [JsonPropertyName("uaqiColorPalette")] public ColorPalette? UaqiColorPalette { get; set; }
            [JsonPropertyName("customLocalAqis")] public List<CustomLocalAqi>? CustomLocalAqis { get; set; }
            [JsonPropertyName("universalAqi")] public bool? UniversalAqi { get; set; }
            [JsonPropertyName("languageCode")] public string? LanguageCode { get; set; }
        }

        private sealed class IntervalPayload
        {
            [JsonPropertyName("startTime")] public string? StartTime { get; set; }
            [JsonPropertyName("endTime")] public string? EndTime { get; set; }
        }
    }
}
