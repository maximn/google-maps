using System;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.PlacesNew.Request
{
    /// <summary>
    /// Request for the Places API (New) Place Details endpoint
    /// (<c>GET https://places.googleapis.com/v1/places/{placeId}</c>). Modern replacement for the
    /// legacy Place Details API; returns a bare <see cref="GoogleMapsApi.Entities.PlacesNew.Response.Place"/>.
    /// </summary>
    /// <remarks>
    /// Requires a field mask. Unlike the search endpoints, Place Details uses <strong>top-level</strong>
    /// field paths (no <c>places.</c> prefix). See
    /// <see href="https://developers.google.com/maps/documentation/places/web-service/place-details"/>.
    /// </remarks>
    public sealed class PlaceDetailsRequest : MapsBaseRequest
    {
        /// <summary>Default <see cref="FieldMask"/> covering a commonly useful set of place fields (top-level paths).</summary>
        public const string DefaultFieldMask =
            "id,displayName,formattedAddress,location,rating,types,googleMapsUri";

        /// <summary>
        /// Comma-separated response field mask using <strong>top-level</strong> paths.
        /// <strong>Required by the Places API (New)</strong>. Passed in the URL as <c>$fields</c>.
        /// </summary>
        public string FieldMask { get; set; } = DefaultFieldMask;

        /// <summary>Place ID of the place to fetch. Required.</summary>
        public string PlaceId { get; set; } = string.Empty;

        /// <summary>BCP-47 language code for the response (e.g. <c>"en"</c>).</summary>
        public string? LanguageCode { get; set; }

        /// <summary>Unicode CLDR region code used for formatting (e.g. <c>"US"</c>).</summary>
        public string? RegionCode { get; set; }

        /// <summary>Autocomplete session token, to bill this details call as part of a session.</summary>
        public string? SessionToken { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(PlaceId))
                throw new ArgumentException("PlaceId is required for Place Details.", nameof(PlaceId));
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new ArgumentException("ApiKey is required for the Places API (New).", nameof(ApiKey));
            if (!IsSSL)
                throw new ArgumentException("Places API (New) requires SSL [IsSSL = true].");
            if (string.IsNullOrWhiteSpace(FieldMask))
                throw new ArgumentException("FieldMask is required for the Places API (New). See PlaceDetailsRequest.DefaultFieldMask for a starting point.", nameof(FieldMask));

            var url =
                $"https://places.googleapis.com/v1/places/{Uri.EscapeDataString(PlaceId)}" +
                $"?key={Uri.EscapeDataString(ApiKey!)}" +
                $"&$fields={Uri.EscapeDataString(FieldMask)}";

            if (!string.IsNullOrWhiteSpace(LanguageCode))
                url += $"&languageCode={Uri.EscapeDataString(LanguageCode!)}";
            if (!string.IsNullOrWhiteSpace(RegionCode))
                url += $"&regionCode={Uri.EscapeDataString(RegionCode!)}";
            if (!string.IsNullOrWhiteSpace(SessionToken))
                url += $"&sessionToken={Uri.EscapeDataString(SessionToken!)}";

            return new Uri(url);
        }
    }
}
