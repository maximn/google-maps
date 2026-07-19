using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Roads.Request
{
    /// <summary>
    /// Request for the Roads API Speed Limits endpoint
    /// (<c>GET https://roads.googleapis.com/v1/speedLimits</c>). Returns the posted speed limit for a
    /// road segment, either by snapping a <see cref="Path"/> or by looking up known <see cref="PlaceIds"/>.
    /// </summary>
    /// <remarks>
    /// The Speed Limits service is only available to customers with an Asset Tracking license; other
    /// keys receive an HTTP 403. Each returned speed limit is separately billable. Exactly one of
    /// <see cref="Path"/> or <see cref="PlaceIds"/> must be supplied. See
    /// <see href="https://developers.google.com/maps/documentation/roads/speed-limits"/>.
    /// </remarks>
    public sealed class SpeedLimitsRequest : MapsBaseRequest
    {
        /// <summary>Maximum number of points or place IDs accepted in a single request.</summary>
        public const int MaxItems = 100;

        /// <summary>
        /// A path to snap and return speed limits for; 1 to <see cref="MaxItems"/> points.
        /// Mutually exclusive with <see cref="PlaceIds"/>.
        /// </summary>
        public IEnumerable<Location>? Path { get; set; }

        /// <summary>
        /// Road-segment place IDs (e.g. from a prior Snap to Roads call) to return speed limits for;
        /// 1 to <see cref="MaxItems"/> IDs. Mutually exclusive with <see cref="Path"/>.
        /// </summary>
        public IEnumerable<string>? PlaceIds { get; set; }

        /// <summary>Unit system for the returned speed limits. Defaults to the API default (KPH).</summary>
        public SpeedUnits? Units { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            var hasPath = Path != null;
            var hasPlaceIds = PlaceIds != null;
            if (hasPath == hasPlaceIds)
                throw new ArgumentException("Exactly one of Path or PlaceIds must be specified for Speed Limits, and both cannot be specified.");

            RoadsRequestHelper.ValidateApiKey(ApiKey);

            var url = new StringBuilder("https://roads.googleapis.com/v1/speedLimits?");

            if (hasPath)
            {
                var path = RoadsRequestHelper.BuildPointsParameter(Path, MaxItems, nameof(Path));
                url.Append("path=").Append(path);
            }
            else
            {
                url.Append(BuildPlaceIdsParameter(PlaceIds!));
            }

            if (Units.HasValue)
                url.Append("&units=").Append(Units.Value == SpeedUnits.Mph ? "MPH" : "KPH");

            url.Append("&key=").Append(Uri.EscapeDataString(ApiKey!));

            return new Uri(url.ToString());
        }

        private static string BuildPlaceIdsParameter(IEnumerable<string> placeIds)
        {
            var list = placeIds.ToList();
            if (list.Count == 0)
                throw new ArgumentException("PlaceIds must contain at least one place ID.", nameof(placeIds));
            if (list.Count > MaxItems)
                throw new ArgumentException($"PlaceIds may contain at most {MaxItems} IDs.", nameof(placeIds));

            return string.Join("&", list.Select(id => "placeId=" + Uri.EscapeDataString(id)));
        }
    }
}
