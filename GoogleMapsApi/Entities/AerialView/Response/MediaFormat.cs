using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.AerialView.Response
{
    /// <summary>
    /// Media format of an Aerial View video rendition. Used as the keys of
    /// <see cref="AerialViewVideoResponse.Uris"/>.
    /// </summary>
    public enum MediaFormat
    {
        /// <summary>The media format is unknown. Default value.</summary>
        [EnumMember(Value = "MEDIA_FORMAT_UNSPECIFIED")] Unspecified,

        /// <summary>A thumbnail image of the video.</summary>
        [EnumMember(Value = "IMAGE")] Image,

        /// <summary>High-quality MP4 rendition.</summary>
        [EnumMember(Value = "MP4_HIGH")] Mp4High,

        /// <summary>Medium-quality MP4 rendition.</summary>
        [EnumMember(Value = "MP4_MEDIUM")] Mp4Medium,

        /// <summary>Low-quality MP4 rendition.</summary>
        [EnumMember(Value = "MP4_LOW")] Mp4Low,

        /// <summary>MPEG-DASH adaptive-bitrate manifest.</summary>
        [EnumMember(Value = "DASH")] Dash,

        /// <summary>HLS adaptive-bitrate manifest (Apple).</summary>
        [EnumMember(Value = "HLS")] Hls,
    }
}
