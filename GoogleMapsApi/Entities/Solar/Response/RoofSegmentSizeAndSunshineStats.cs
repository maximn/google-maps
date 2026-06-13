using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>Size, orientation and sunshine statistics for a single roof segment.</summary>
    public sealed class RoofSegmentSizeAndSunshineStats
    {
        /// <summary>Angle of the roof segment relative to the horizontal, in degrees.</summary>
        [JsonPropertyName("pitchDegrees")]
        public float PitchDegrees { get; set; }

        /// <summary>Compass direction the roof segment faces, in degrees (0 = north, 90 = east).</summary>
        [JsonPropertyName("azimuthDegrees")]
        public float AzimuthDegrees { get; set; }

        /// <summary>Size and sunshine statistics for this segment.</summary>
        [JsonPropertyName("stats")]
        public SizeAndSunshineStats? Stats { get; set; }

        /// <summary>The centre of the roof segment.</summary>
        [JsonPropertyName("center")]
        public LatLng? Center { get; set; }

        /// <summary>The bounding box of the roof segment.</summary>
        [JsonPropertyName("boundingBox")]
        public LatLngBox? BoundingBox { get; set; }

        /// <summary>Height of the roof-segment plane above sea level at its centre, in meters.</summary>
        [JsonPropertyName("planeHeightAtCenterMeters")]
        public float PlaneHeightAtCenterMeters { get; set; }
    }
}
