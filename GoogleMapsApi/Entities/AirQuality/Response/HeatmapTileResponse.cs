using System;
using GoogleMapsApi.Entities.AirQuality.Request;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.AirQuality.Response
{
    /// <summary>
    /// Response from the Air Quality API heatmap-tile endpoint — the raw PNG bytes of a map tile.
    /// Being binary, this is populated directly by the engine rather than from JSON.
    /// </summary>
    public sealed class HeatmapTileResponse : IResponseFor<HeatmapTileRequest>, IBinaryResponse
    {
        /// <summary>The raw PNG bytes of the tile.</summary>
        public byte[] Content { get; set; } = Array.Empty<byte>();

        /// <summary>The response media type (typically <c>image/png</c>).</summary>
        public string? ContentType { get; set; }
    }
}
