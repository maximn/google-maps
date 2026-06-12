using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Roads.Request;

namespace GoogleMapsApi.Entities.Roads.Response
{
    /// <summary>
    /// Response from the Roads API Nearest Roads endpoint.
    /// </summary>
    public sealed class NearestRoadsResponse : IResponseFor<NearestRoadsRequest>
    {
        /// <summary>
        /// The nearest road segment for each input point. Each <see cref="SnappedPoint.OriginalIndex"/>
        /// maps back to the input point; a single input point may yield multiple snapped points.
        /// </summary>
        [JsonPropertyName("snappedPoints")]
        public IReadOnlyList<SnappedPoint>? SnappedPoints { get; set; }
    }
}
