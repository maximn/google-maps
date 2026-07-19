using System;
using System.Collections.Generic;
using System.Linq;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Roads.Request
{
    /// <summary>
    /// Request for the Roads API Snap to Roads endpoint
    /// (<c>GET https://roads.googleapis.com/v1/snapToRoads</c>). Snaps up to
    /// <see cref="MaxPathPoints"/> GPS points to the most likely road segments they were traveling along.
    /// </summary>
    /// <remarks>
    /// For best results, consecutive points should be within 300m of each other. See
    /// <see href="https://developers.google.com/maps/documentation/roads/snap"/>.
    /// </remarks>
    public sealed class SnapToRoadsRequest : MapsBaseRequest
    {
        /// <summary>Maximum number of points accepted in a single request.</summary>
        public const int MaxPathPoints = 100;

        /// <summary>
        /// The path to snap, as an ordered sequence of points. Required; 1 to
        /// <see cref="MaxPathPoints"/> points.
        /// </summary>
        public IEnumerable<Location>? Path { get; set; }

        /// <summary>
        /// When <c>true</c>, the response is interpolated to include additional points so the
        /// returned path smoothly follows road geometry. Interpolated points have no
        /// <see cref="Response.SnappedPoint.OriginalIndex"/>. Defaults to <c>false</c>.
        /// </summary>
        public bool Interpolate { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            var path = RoadsRequestHelper.BuildPointsParameter(Path, MaxPathPoints, nameof(Path));
            RoadsRequestHelper.ValidateApiKey(ApiKey);

            var url =
                "https://roads.googleapis.com/v1/snapToRoads" +
                $"?path={path}" +
                $"&interpolate={(Interpolate ? "true" : "false")}" +
                $"&key={Uri.EscapeDataString(ApiKey!)}";

            return new Uri(url);
        }
    }
}
