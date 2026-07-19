using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNew.Common;

namespace GoogleMapsApi.Entities.PlacesNew.Request
{
    /// <summary>
    /// Request for the Places API (New) Text Search endpoint
    /// (<c>POST https://places.googleapis.com/v1/places:searchText</c>). Modern replacement for the
    /// legacy Places Text Search.
    /// </summary>
    /// <remarks>
    /// The Places API (New) <strong>requires a field mask</strong>. <see cref="FieldMask"/> is
    /// pre-populated with a reasonable default using <c>places.</c>-prefixed paths; tighten it to
    /// reduce response size and cost. See
    /// <see href="https://developers.google.com/maps/documentation/places/web-service/text-search"/>.
    /// </remarks>
    public sealed class SearchTextRequest : MapsBaseRequest
    {
        /// <summary>Default <see cref="FieldMask"/> covering a commonly useful set of place fields.</summary>
        public const string DefaultFieldMask =
            "places.id,places.displayName,places.formattedAddress,places.location," +
            "places.rating,places.types,places.googleMapsUri";

        /// <summary>
        /// Comma-separated response field mask using <c>places.</c>-prefixed paths.
        /// <strong>Required by the Places API (New)</strong>. Passed in the URL as <c>$fields</c>.
        /// </summary>
        public string FieldMask { get; set; } = DefaultFieldMask;

        /// <summary>Free-text query (e.g. <c>"pizza in New York"</c>). Required.</summary>
        public string TextQuery { get; set; } = string.Empty;

        /// <summary>Restricts results to a single place type (e.g. <c>"restaurant"</c>).</summary>
        public string? IncludedType { get; set; }

        /// <summary>If true, return only places that are open now.</summary>
        public bool? OpenNow { get; set; }

        /// <summary>Filter out places with a rating below this value (1.0–5.0).</summary>
        public double? MinRating { get; set; }

        /// <summary>Restrict results to the given price levels.</summary>
        public List<PriceLevel>? PriceLevels { get; set; }

        /// <summary>How results should be ranked.</summary>
        public RankPreference? RankPreference { get; set; }

        /// <summary>Soft region bias for results.</summary>
        public LocationBias? LocationBias { get; set; }

        /// <summary>Hard region restriction for results.</summary>
        public LocationRestriction? LocationRestriction { get; set; }

        /// <summary>Maximum number of results per page (1–20).</summary>
        public int? PageSize { get; set; }

        /// <summary>Page token from a previous response, to fetch the next page.</summary>
        public string? PageToken { get; set; }

        /// <summary>BCP-47 language code for the response (e.g. <c>"en"</c>).</summary>
        public string? LanguageCode { get; set; }

        /// <summary>Unicode CLDR region code used for biasing/formatting (e.g. <c>"US"</c>).</summary>
        public string? RegionCode { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(TextQuery))
                throw new ArgumentException("TextQuery is required for Text Search.", nameof(TextQuery));
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new ArgumentException("ApiKey is required for the Places API (New).", nameof(ApiKey));
            if (string.IsNullOrWhiteSpace(FieldMask))
                throw new ArgumentException("FieldMask is required for the Places API (New). See SearchTextRequest.DefaultFieldMask for a starting point.", nameof(FieldMask));

            return new Uri(
                "https://places.googleapis.com/v1/places:searchText" +
                $"?key={Uri.EscapeDataString(ApiKey!)}" +
                $"&$fields={Uri.EscapeDataString(FieldMask)}");
        }

        /// <inheritdoc/>
        protected internal override HttpContent? GetRequestBody()
        {
            if (string.IsNullOrWhiteSpace(TextQuery))
                throw new ArgumentException("TextQuery is required for Text Search.", nameof(TextQuery));

            var payload = new Payload
            {
                TextQuery = TextQuery,
                IncludedType = string.IsNullOrWhiteSpace(IncludedType) ? null : IncludedType,
                OpenNow = OpenNow,
                MinRating = MinRating,
                PriceLevels = PriceLevels is { Count: > 0 } ? PriceLevels : null,
                RankPreference = RankPreference,
                LocationBias = LocationBias,
                LocationRestriction = LocationRestriction,
                PageSize = PageSize,
                PageToken = string.IsNullOrWhiteSpace(PageToken) ? null : PageToken,
                LanguageCode = string.IsNullOrWhiteSpace(LanguageCode) ? null : LanguageCode,
                RegionCode = string.IsNullOrWhiteSpace(RegionCode) ? null : RegionCode,
            };
            var json = JsonSerializer.Serialize(payload, BodyOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static readonly JsonSerializerOptions BodyOptions = JsonSerializerConfiguration.CreateRequestBodyOptions();

        private sealed class Payload
        {
            [JsonPropertyName("textQuery")] public string? TextQuery { get; set; }
            [JsonPropertyName("includedType")] public string? IncludedType { get; set; }
            [JsonPropertyName("openNow")] public bool? OpenNow { get; set; }
            [JsonPropertyName("minRating")] public double? MinRating { get; set; }
            [JsonPropertyName("priceLevels")] public List<PriceLevel>? PriceLevels { get; set; }
            [JsonPropertyName("rankPreference")] public RankPreference? RankPreference { get; set; }
            [JsonPropertyName("locationBias")] public LocationBias? LocationBias { get; set; }
            [JsonPropertyName("locationRestriction")] public LocationRestriction? LocationRestriction { get; set; }
            [JsonPropertyName("pageSize")] public int? PageSize { get; set; }
            [JsonPropertyName("pageToken")] public string? PageToken { get; set; }
            [JsonPropertyName("languageCode")] public string? LanguageCode { get; set; }
            [JsonPropertyName("regionCode")] public string? RegionCode { get; set; }
        }
    }
}
