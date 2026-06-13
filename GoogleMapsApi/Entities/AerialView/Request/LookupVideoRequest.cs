using System;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.AerialView.Request
{
    /// <summary>
    /// Request for the Aerial View API <c>lookupVideo</c> endpoint
    /// (<c>GET https://aerialview.googleapis.com/v1/videos:lookupVideo</c>).
    /// Retrieves a video and its current state by <see cref="VideoId"/> (recommended) or
    /// <see cref="Address"/> — supply exactly one.
    /// </summary>
    /// <remarks>
    /// A still-rendering video returns HTTP 200 with state <see cref="Response.VideoState.Processing"/>.
    /// A video that does not exist (or has no 3D imagery) returns HTTP 404, which the engine surfaces as
    /// an <see cref="System.Net.Http.HttpRequestException"/>. This endpoint is billable.
    /// </remarks>
    public sealed class LookupVideoRequest : MapsBaseRequest
    {
        /// <summary>The video id returned by a prior <c>RenderVideo</c> call. Mutually exclusive with <see cref="Address"/>.</summary>
        public string? VideoId { get; set; }

        /// <summary>The US postal address to look up. Mutually exclusive with <see cref="VideoId"/>.</summary>
        public string? Address { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException("ApiKey is required for the Aerial View API.");
            if (!IsSSL)
                throw new ArgumentException("Aerial View API requires SSL [IsSSL = true].");

            var hasVideoId = !string.IsNullOrWhiteSpace(VideoId);
            var hasAddress = !string.IsNullOrWhiteSpace(Address);
            if (hasVideoId == hasAddress)
                throw new InvalidOperationException("Specify exactly one of VideoId or Address for the Aerial View lookupVideo endpoint.");

            var selector = hasVideoId
                ? $"videoId={Uri.EscapeDataString(VideoId!)}"
                : $"address={Uri.EscapeDataString(Address!)}";

            return new Uri(
                "https://aerialview.googleapis.com/v1/videos:lookupVideo" +
                $"?key={Uri.EscapeDataString(ApiKey!)}" +
                $"&{selector}");
        }
    }
}
