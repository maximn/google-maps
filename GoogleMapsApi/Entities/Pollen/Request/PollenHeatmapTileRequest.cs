using System;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Pollen.Request
{
    /// <summary>
    /// Request for the Pollen API heatmap-tile endpoint
    /// (<c>GET https://pollen.googleapis.com/v1/mapTypes/{mapType}/heatmapTiles/{zoom}/{x}/{y}</c>).
    /// Downloads a single PNG map tile for the chosen pollen index. The response is binary, not JSON.
    /// </summary>
    /// <remarks>
    /// Tile coordinates follow the standard Web Mercator scheme: <see cref="X"/> and <see cref="Y"/> are
    /// in the range <c>[0, 2^zoom)</c>. The Pollen API is billable; calls beyond the free tier incur charges.
    /// </remarks>
    public sealed class PollenHeatmapTileRequest : MapsBaseRequest
    {
        /// <summary>Which pollen index the tile renders. Required.</summary>
        public PollenMapType MapType { get; set; }

        /// <summary>Zoom level, in the range [0, 16]; 0 is the whole world in one tile.</summary>
        public int Zoom { get; set; }

        /// <summary>The tile's east-west index, in the range [0, 2^zoom).</summary>
        public int X { get; set; }

        /// <summary>The tile's north-south index, in the range [0, 2^zoom).</summary>
        public int Y { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException("ApiKey is required for the Pollen API.");
            if (Zoom < 0 || Zoom > 16)
                throw new ArgumentException("Zoom must be in the range [0, 16].", nameof(Zoom));
            if (X < 0 || Y < 0)
                throw new ArgumentException("Tile coordinates X and Y must be non-negative.");

            return new Uri(
                "https://pollen.googleapis.com/v1/mapTypes/" +
                $"{EnumMemberHelper.GetValue(MapType)}/heatmapTiles/{Zoom}/{X}/{Y}" +
                $"?key={Uri.EscapeDataString(ApiKey!)}");
        }
    }
}
