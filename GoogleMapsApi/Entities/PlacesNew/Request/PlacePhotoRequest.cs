using System;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.PlacesNew.Request
{
    /// <summary>
    /// Request for the Places API (New) Photo media endpoint
    /// (<c>GET https://places.googleapis.com/v1/{photoName}/media</c>). Resolves a photo reference
    /// returned on <see cref="GoogleMapsApi.Entities.PlacesNew.Response.Photo.Name"/> to a usable image URI.
    /// </summary>
    /// <remarks>
    /// Uses <c>skipHttpRedirect=true</c> so the API returns a small JSON document
    /// (<c>{ name, photoUri }</c>) rather than redirecting to binary image bytes. At least one of
    /// <see cref="MaxHeightPx"/> or <see cref="MaxWidthPx"/> is required.
    /// </remarks>
    public sealed class PlacePhotoRequest : MapsBaseRequest
    {
        /// <summary>
        /// Resource name of the photo, taken from <c>Place.Photos[].Name</c>
        /// (e.g. <c>places/X/photos/Y</c>). Required.
        /// </summary>
        public string PhotoName { get; set; } = string.Empty;

        /// <summary>Maximum desired height of the image, in pixels (1–4800).</summary>
        public int? MaxHeightPx { get; set; }

        /// <summary>Maximum desired width of the image, in pixels (1–4800).</summary>
        public int? MaxWidthPx { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(PhotoName))
                throw new ArgumentException("PhotoName is required for Place Photo.", nameof(PhotoName));
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new ArgumentException("ApiKey is required for the Places API (New).", nameof(ApiKey));
            if (!MaxHeightPx.HasValue && !MaxWidthPx.HasValue)
                throw new ArgumentException("At least one of MaxHeightPx or MaxWidthPx is required for Place Photo.");

            // PhotoName already contains path segments (places/X/photos/Y); keep its slashes intact.
            var url =
                $"https://places.googleapis.com/v1/{PhotoName}/media" +
                $"?key={Uri.EscapeDataString(ApiKey!)}" +
                "&skipHttpRedirect=true";

            if (MaxHeightPx.HasValue)
                url += $"&maxHeightPx={MaxHeightPx.Value}";
            if (MaxWidthPx.HasValue)
                url += $"&maxWidthPx={MaxWidthPx.Value}";

            return new Uri(url);
        }
    }
}
