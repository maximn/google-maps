using System;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.AirQuality.Request
{
    /// <summary>
    /// Request for the Air Quality API heatmap-tile endpoint
    /// (<c>GET https://airquality.googleapis.com/v1/mapTypes/{mapType}/heatmapTiles/{zoom}/{x}/{y}</c>).
    /// Downloads a single 256×256 PNG map tile for the chosen air-quality index. The response is binary,
    /// not JSON.
    /// </summary>
    /// <remarks>
    /// Tile coordinates follow the standard Web Mercator scheme: <see cref="X"/> and <see cref="Y"/> are
    /// in the range <c>[0, 2^zoom)</c>. The Air Quality API is billable; calls beyond the free tier incur charges.
    /// </remarks>
    public sealed class HeatmapTileRequest : MapsBaseRequest
    {
        /// <summary>Which air-quality index and colour scheme the tile renders. Required.</summary>
        public AirQualityMapType MapType { get; set; }

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
                throw new InvalidOperationException("ApiKey is required for the Air Quality API.");
            if (Zoom < 0 || Zoom > 16)
                throw new ArgumentException("Zoom must be in the range [0, 16].", nameof(Zoom));
            if (X < 0 || Y < 0)
                throw new ArgumentException("Tile coordinates X and Y must be non-negative.");

            return new Uri(
                "https://airquality.googleapis.com/v1/mapTypes/" +
                $"{EnumMemberHelper.GetValue(MapType)}/heatmapTiles/{Zoom}/{X}/{Y}" +
                $"?key={Uri.EscapeDataString(ApiKey!)}");
        }
    }
}
