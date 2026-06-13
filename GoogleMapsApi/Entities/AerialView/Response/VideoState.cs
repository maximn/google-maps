using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.AerialView.Response
{
    /// <summary>
    /// Rendering state of an Aerial View video.
    /// </summary>
    public enum VideoState
    {
        /// <summary>The state is unknown. Default value.</summary>
        [EnumMember(Value = "STATE_UNSPECIFIED")] Unspecified,

        /// <summary>The video is currently being rendered. Poll <c>LookupVideo</c> until the state changes.</summary>
        [EnumMember(Value = "PROCESSING")] Processing,

        /// <summary>Rendering finished; the video is viewable and <c>Uris</c> are populated.</summary>
        [EnumMember(Value = "ACTIVE")] Active,

        /// <summary>Rendering failed for this video.</summary>
        [EnumMember(Value = "FAILED")] Failed,
    }
}
