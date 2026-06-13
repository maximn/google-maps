using System;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Solar.Request;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>
    /// Response from the Solar API <c>geoTiff:get</c> endpoint — the raw GeoTIFF bytes of a data
    /// layer. Being binary, this is populated directly by the engine rather than from JSON.
    /// </summary>
    public sealed class GeoTiffResponse : IResponseFor<GeoTiffRequest>, IBinaryResponse
    {
        /// <summary>The raw GeoTIFF bytes.</summary>
        public byte[] Content { get; set; } = Array.Empty<byte>();

        /// <summary>The response media type (typically <c>image/tiff</c>).</summary>
        public string? ContentType { get; set; }
    }
}
