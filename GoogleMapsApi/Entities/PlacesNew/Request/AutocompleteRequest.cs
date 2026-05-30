using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNew.Common;

namespace GoogleMapsApi.Entities.PlacesNew.Request
{
    /// <summary>
    /// Request for the Places API (New) Autocomplete endpoint
    /// (<c>POST https://places.googleapis.com/v1/places:autocomplete</c>). Modern replacement for the
    /// legacy Place Autocomplete.
    /// </summary>
    /// <remarks>
    /// Autocomplete does <strong>not</strong> use a field mask. See
    /// <see href="https://developers.google.com/maps/documentation/places/web-service/place-autocomplete"/>.
    /// </remarks>
    public sealed class AutocompleteRequest : MapsBaseRequest
    {
        /// <summary>Text typed by the user to predict against. Required.</summary>
        public string Input { get; set; } = string.Empty;

        /// <summary>Soft region bias for predictions.</summary>
        public LocationBias? LocationBias { get; set; }

        /// <summary>Hard region restriction for predictions.</summary>
        public LocationRestriction? LocationRestriction { get; set; }

        /// <summary>Restrict predictions to these primary place types.</summary>
        public List<string>? IncludedPrimaryTypes { get; set; }

        /// <summary>Restrict predictions to these region codes (CLDR).</summary>
        public List<string>? IncludedRegionCodes { get; set; }

        /// <summary>BCP-47 language code for the response (e.g. <c>"en"</c>).</summary>
        public string? LanguageCode { get; set; }

        /// <summary>Unicode CLDR region code used for biasing/formatting (e.g. <c>"US"</c>).</summary>
        public string? RegionCode { get; set; }

        /// <summary>Autocomplete session token grouping requests for billing.</summary>
        public string? SessionToken { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(Input))
                throw new ArgumentException("Input is required for Autocomplete.", nameof(Input));
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new ArgumentException("ApiKey is required for the Places API (New).", nameof(ApiKey));
            if (!IsSSL)
                throw new ArgumentException("Places API (New) requires SSL [IsSSL = true].");

            return new Uri(
                "https://places.googleapis.com/v1/places:autocomplete" +
                $"?key={Uri.EscapeDataString(ApiKey!)}");
        }

        /// <inheritdoc/>
        protected internal override HttpContent? GetRequestBody()
        {
            if (string.IsNullOrWhiteSpace(Input))
                throw new ArgumentException("Input is required for Autocomplete.", nameof(Input));

            var payload = new Payload
            {
                Input = Input,
                LocationBias = LocationBias,
                LocationRestriction = LocationRestriction,
                IncludedPrimaryTypes = IncludedPrimaryTypes is { Count: > 0 } ? IncludedPrimaryTypes : null,
                IncludedRegionCodes = IncludedRegionCodes is { Count: > 0 } ? IncludedRegionCodes : null,
                LanguageCode = string.IsNullOrWhiteSpace(LanguageCode) ? null : LanguageCode,
                RegionCode = string.IsNullOrWhiteSpace(RegionCode) ? null : RegionCode,
                SessionToken = string.IsNullOrWhiteSpace(SessionToken) ? null : SessionToken,
            };
            var json = JsonSerializer.Serialize(payload, BodyOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static readonly JsonSerializerOptions BodyOptions = BuildBodyOptions();

        private static JsonSerializerOptions BuildBodyOptions()
        {
            var opts = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
            opts.Converters.Add(new Engine.JsonConverters.EnumMemberJsonConverterFactory());
            return opts;
        }

        private sealed class Payload
        {
            [JsonPropertyName("input")] public string? Input { get; set; }
            [JsonPropertyName("locationBias")] public LocationBias? LocationBias { get; set; }
            [JsonPropertyName("locationRestriction")] public LocationRestriction? LocationRestriction { get; set; }
            [JsonPropertyName("includedPrimaryTypes")] public List<string>? IncludedPrimaryTypes { get; set; }
            [JsonPropertyName("includedRegionCodes")] public List<string>? IncludedRegionCodes { get; set; }
            [JsonPropertyName("languageCode")] public string? LanguageCode { get; set; }
            [JsonPropertyName("regionCode")] public string? RegionCode { get; set; }
            [JsonPropertyName("sessionToken")] public string? SessionToken { get; set; }
        }
    }
}
