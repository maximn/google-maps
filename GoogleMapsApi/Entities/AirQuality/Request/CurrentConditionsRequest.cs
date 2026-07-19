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
    /// Request for the Air Quality API <c>currentConditions:lookup</c> endpoint
    /// (<c>POST https://airquality.googleapis.com/v1/currentConditions:lookup</c>). Returns hourly
    /// air-quality information for a coordinate.
    /// </summary>
    /// <remarks>The Air Quality API is billable; calls beyond the free tier incur charges.</remarks>
    public sealed class CurrentConditionsRequest : MapsBaseRequest
    {
        /// <summary>Latitude of the query point, in degrees. Required.</summary>
        public double Latitude { get; set; }

        /// <summary>Longitude of the query point, in degrees. Required.</summary>
        public double Longitude { get; set; }

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
                "https://airquality.googleapis.com/v1/currentConditions:lookup" +
                $"?key={Uri.EscapeDataString(ApiKey!)}");
        }

        /// <inheritdoc/>
        protected internal override HttpContent? GetRequestBody()
        {
            var payload = new Payload
            {
                Location = new LatLng { Latitude = Latitude, Longitude = Longitude },
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
            [JsonPropertyName("extraComputations")] public List<ExtraComputation>? ExtraComputations { get; set; }
            [JsonPropertyName("uaqiColorPalette")] public ColorPalette? UaqiColorPalette { get; set; }
            [JsonPropertyName("customLocalAqis")] public List<CustomLocalAqi>? CustomLocalAqis { get; set; }
            [JsonPropertyName("universalAqi")] public bool? UniversalAqi { get; set; }
            [JsonPropertyName("languageCode")] public string? LanguageCode { get; set; }
        }
    }
}
