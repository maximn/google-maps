using System;
using System.Collections.Generic;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Roads.Request
{
    /// <summary>
    /// Request for the Roads API Nearest Roads endpoint
    /// (<c>GET https://roads.googleapis.com/v1/nearestRoads</c>). Returns the nearest road segment
    /// for each of up to <see cref="MaxPoints"/> arbitrary points. Unlike Snap to Roads, the input
    /// points need not be in order or form a traveled path.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/roads/nearest"/>.
    /// </remarks>
    public sealed class NearestRoadsRequest : MapsBaseRequest
    {
        /// <summary>Maximum number of points accepted in a single request.</summary>
        public const int MaxPoints = 100;

        /// <summary>
        /// The points to find roads for. Required; 1 to <see cref="MaxPoints"/> points.
        /// </summary>
        public IEnumerable<Location>? Points { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            var points = RoadsRequestHelper.BuildPointsParameter(Points, MaxPoints, nameof(Points));
            RoadsRequestHelper.ValidateApiKeyAndSsl(ApiKey, IsSSL);

            var url =
                "https://roads.googleapis.com/v1/nearestRoads" +
                $"?points={points}" +
                $"&key={Uri.EscapeDataString(ApiKey!)}";

            return new Uri(url);
        }
    }
}
