using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Roads.Request;

namespace GoogleMapsApi.Entities.Roads.Response
{
    /// <summary>
    /// Response from the Roads API Snap to Roads endpoint.
    /// </summary>
    public sealed class SnapToRoadsResponse : IResponseFor<SnapToRoadsRequest>
    {
        /// <summary>
        /// The points snapped to roads, in order. With <c>interpolate=true</c> this includes
        /// interpolated points (whose <see cref="SnappedPoint.OriginalIndex"/> is <c>null</c>).
        /// </summary>
        [JsonPropertyName("snappedPoints")]
        public IReadOnlyList<SnappedPoint>? SnappedPoints { get; set; }

        /// <summary>
        /// A user-visible warning, present for example when the input path is too sparse for
        /// reliable snapping. <c>null</c> when there is no warning.
        /// </summary>
        [JsonPropertyName("warningMessage")]
        public string? WarningMessage { get; set; }
    }
}
