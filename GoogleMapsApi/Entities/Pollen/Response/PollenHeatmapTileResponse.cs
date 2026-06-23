using System;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Pollen.Request;

namespace GoogleMapsApi.Entities.Pollen.Response
{
    /// <summary>
    /// Response from the Pollen API heatmap-tile endpoint — the raw PNG bytes of a map tile.
    /// Being binary, this is populated directly by the engine rather than from JSON.
    /// </summary>
    public sealed class PollenHeatmapTileResponse : IResponseFor<PollenHeatmapTileRequest>, IBinaryResponse
    {
        /// <summary>The raw PNG bytes of the tile.</summary>
        public byte[] Content { get; set; } = Array.Empty<byte>();

        /// <summary>The response media type (typically <c>image/png</c>).</summary>
        public string? ContentType { get; set; }
    }
}
