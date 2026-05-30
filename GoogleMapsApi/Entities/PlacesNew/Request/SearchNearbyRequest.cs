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
    /// Request for the Places API (New) Nearby Search endpoint
    /// (<c>POST https://places.googleapis.com/v1/places:searchNearby</c>). Modern replacement for the
    /// legacy Places Nearby Search.
    /// </summary>
    /// <remarks>
    /// Requires a field mask (<c>places.</c>-prefixed) and a <see cref="LocationRestriction"/> with a
    /// circle. See
    /// <see href="https://developers.google.com/maps/documentation/places/web-service/nearby-search"/>.
    /// </remarks>
    public sealed class SearchNearbyRequest : MapsBaseRequest
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

        /// <summary>Restrict results to one or more place types.</summary>
        public List<string>? IncludedTypes { get; set; }

        /// <summary>Exclude results matching these place types.</summary>
        public List<string>? ExcludedTypes { get; set; }

        /// <summary>Maximum number of results to return (1–20).</summary>
        public int? MaxResultCount { get; set; }

        /// <summary>
        /// Region to search within. <strong>Required</strong> — must contain a
        /// <see cref="Common.Circle"/>.
        /// </summary>
        public LocationRestriction LocationRestriction { get; set; } = new LocationRestriction();

        /// <summary>How results should be ranked.</summary>
        public RankPreference? RankPreference { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            ValidateLocationRestriction();
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new ArgumentException("ApiKey is required for the Places API (New).", nameof(ApiKey));
            if (!IsSSL)
                throw new ArgumentException("Places API (New) requires SSL [IsSSL = true].");
            if (string.IsNullOrWhiteSpace(FieldMask))
                throw new ArgumentException("FieldMask is required for the Places API (New). See SearchNearbyRequest.DefaultFieldMask for a starting point.", nameof(FieldMask));

            return new Uri(
                "https://places.googleapis.com/v1/places:searchNearby" +
                $"?key={Uri.EscapeDataString(ApiKey!)}" +
                $"&$fields={Uri.EscapeDataString(FieldMask)}");
        }

        /// <inheritdoc/>
        protected internal override HttpContent? GetRequestBody()
        {
            ValidateLocationRestriction();

            var payload = new Payload
            {
                IncludedTypes = IncludedTypes is { Count: > 0 } ? IncludedTypes : null,
                ExcludedTypes = ExcludedTypes is { Count: > 0 } ? ExcludedTypes : null,
                MaxResultCount = MaxResultCount,
                LocationRestriction = LocationRestriction,
                RankPreference = RankPreference,
            };
            var json = JsonSerializer.Serialize(payload, BodyOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private void ValidateLocationRestriction()
        {
            if (LocationRestriction?.Circle == null)
                throw new ArgumentException("LocationRestriction with a Circle is required for Nearby Search.", nameof(LocationRestriction));
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
            [JsonPropertyName("includedTypes")] public List<string>? IncludedTypes { get; set; }
            [JsonPropertyName("excludedTypes")] public List<string>? ExcludedTypes { get; set; }
            [JsonPropertyName("maxResultCount")] public int? MaxResultCount { get; set; }
            [JsonPropertyName("locationRestriction")] public LocationRestriction? LocationRestriction { get; set; }
            [JsonPropertyName("rankPreference")] public RankPreference? RankPreference { get; set; }
        }
    }
}
