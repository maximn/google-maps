namespace GoogleMapsApi.Entities.Common
{
    /// <summary>
    /// Marks a response whose payload is raw bytes rather than JSON. When a response type
    /// implements this interface the engine reads the HTTP body as binary and populates
    /// <see cref="Content"/>/<see cref="ContentType"/> instead of deserializing JSON.
    /// Used by endpoints such as the Solar API GeoTIFF download.
    /// </summary>
    public interface IBinaryResponse
    {
        /// <summary>The raw response body bytes.</summary>
        byte[] Content { get; set; }

        /// <summary>The response's <c>Content-Type</c> media type, if the server supplied one.</summary>
        string? ContentType { get; set; }
    }
}
