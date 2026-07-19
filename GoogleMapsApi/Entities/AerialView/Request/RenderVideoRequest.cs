using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.AerialView.Request
{
    /// <summary>
    /// Request for the Aerial View API <c>renderVideo</c> endpoint
    /// (<c>POST https://aerialview.googleapis.com/v1/videos:renderVideo</c>).
    /// Enqueues rendering of a cinematic flyover video for a US postal address.
    /// </summary>
    /// <remarks>
    /// Rendering is asynchronous and can take a long time (up to a few hours). The response carries a
    /// <see cref="Response.VideoState.Processing"/> state plus a video id; poll
    /// <see cref="LookupVideoRequest"/> by that id until the state becomes
    /// <see cref="Response.VideoState.Active"/> (use exponential backoff). If a video already exists for
    /// the address, this returns immediately. The render endpoint itself is not billed.
    /// </remarks>
    public sealed class RenderVideoRequest : MapsBaseRequest
    {
        /// <summary>The US postal address to render a flyover for. Required.</summary>
        public string Address { get; set; } = null!;

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException("ApiKey is required for the Aerial View API.");

            return new Uri($"https://aerialview.googleapis.com/v1/videos:renderVideo?key={Uri.EscapeDataString(ApiKey!)}");
        }

        /// <inheritdoc/>
        protected internal override HttpContent? GetRequestBody()
        {
            if (string.IsNullOrWhiteSpace(Address))
                throw new ArgumentException("Address is required for the Aerial View renderVideo endpoint.", nameof(Address));

            var payload = new Payload { Address = Address };
            var json = JsonSerializer.Serialize(payload, BodyOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static readonly JsonSerializerOptions BodyOptions = JsonSerializerConfiguration.CreateRequestBodyOptions();

        private sealed class Payload
        {
            [JsonPropertyName("address")] public string? Address { get; set; }
        }
    }
}
