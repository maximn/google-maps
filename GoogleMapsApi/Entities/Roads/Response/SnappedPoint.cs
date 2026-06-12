using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Roads.Response
{
    /// <summary>
    /// A point snapped to a road segment. Returned by Snap to Roads, Nearest Roads, and (when a
    /// path is supplied) Speed Limits.
    /// </summary>
    public sealed class SnappedPoint
    {
        /// <summary>The snapped coordinate on the road.</summary>
        [JsonPropertyName("location")]
        public RoadsLocation? Location { get; set; }

        /// <summary>
        /// Zero-based index of the corresponding input point. Absent for interpolated points
        /// (Snap to Roads with <c>interpolate=true</c>), which is why this is nullable.
        /// </summary>
        [JsonPropertyName("originalIndex")]
        public int? OriginalIndex { get; set; }

        /// <summary>Unique identifier of the road segment this point was snapped to.</summary>
        [JsonPropertyName("placeId")]
        public string? PlaceId { get; set; }
    }
}
