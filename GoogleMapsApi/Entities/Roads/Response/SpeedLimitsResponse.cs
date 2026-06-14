using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Roads.Request;

namespace GoogleMapsApi.Entities.Roads.Response
{
    /// <summary>
    /// Response from the Roads API Speed Limits endpoint.
    /// </summary>
    public sealed class SpeedLimitsResponse : IResponseFor<SpeedLimitsRequest>
    {
        /// <summary>The posted speed limits, one per road segment.</summary>
        [JsonPropertyName("speedLimits")]
        public IReadOnlyList<SpeedLimit>? SpeedLimits { get; set; }

        /// <summary>
        /// The points snapped to roads. Populated when the request supplied a path; absent when
        /// the request supplied place IDs.
        /// </summary>
        [JsonPropertyName("snappedPoints")]
        public IReadOnlyList<SnappedPoint>? SnappedPoints { get; set; }
    }
}
