using System;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Solar.Request
{
    /// <summary>
    /// Request for the Google Solar API <c>geoTiff:get</c> endpoint
    /// (<c>GET https://solar.googleapis.com/v1/geoTiff:get</c>). Downloads the raw GeoTIFF bytes
    /// for a data layer URL returned by a <see cref="DataLayersRequest"/>.
    /// </summary>
    /// <remarks>
    /// Pass the layer URL from a <c>DataLayersResponse</c> (e.g. <c>DsmUrl</c>, <c>AnnualFluxUrl</c>)
    /// as <see cref="Url"/>; the API key is appended automatically. The response is binary, not JSON.
    /// </remarks>
    public sealed class GeoTiffRequest : MapsBaseRequest
    {
        /// <summary>
        /// A data-layer URL returned by the <c>dataLayers:get</c> endpoint
        /// (e.g. <c>https://solar.googleapis.com/v1/geoTiff:get?id=...</c>). Required.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException("ApiKey is required for the Solar API.");
            if (!IsSSL)
                throw new ArgumentException("Solar API requires SSL [IsSSL = true].");
            if (string.IsNullOrWhiteSpace(Url))
                throw new ArgumentException("Url is required; supply a data-layer URL from a DataLayersResponse.", nameof(Url));
            if (!Uri.TryCreate(Url, UriKind.Absolute, out var layerUri) ||
                layerUri.Scheme != Uri.UriSchemeHttps ||
                !layerUri.Host.Equals("solar.googleapis.com", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Url must be an absolute https solar.googleapis.com data-layer URL.", nameof(Url));

            var separator = Url.Contains("?") ? "&" : "?";
            return new Uri($"{Url}{separator}key={Uri.EscapeDataString(ApiKey!)}");
        }
    }
}
