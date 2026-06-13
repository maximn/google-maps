using System.Collections.Generic;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.AerialView.Request;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.AerialView.Response
{
    /// <summary>
    /// An Aerial View video (<c>Video</c>). Returned by both <c>RenderVideo</c> and <c>LookupVideo</c>.
    /// </summary>
    /// <remarks>
    /// While <see cref="State"/> is <see cref="VideoState.Processing"/>, <see cref="Uris"/> is absent and
    /// <see cref="Metadata"/> typically carries only the video id. Once <see cref="VideoState.Active"/>,
    /// <see cref="Uris"/> is populated with signed, short-lived links for each available
    /// <see cref="MediaFormat"/>.
    /// </remarks>
    public sealed class AerialViewVideoResponse
        : IResponseFor<RenderVideoRequest>, IResponseFor<LookupVideoRequest>
    {
        /// <summary>Current rendering state of the video.</summary>
        [JsonPropertyName("state")] public VideoState State { get; set; }

        /// <summary>Metadata about the video (id, capture date, duration).</summary>
        [JsonPropertyName("metadata")] public VideoMetadata? Metadata { get; set; }

        /// <summary>
        /// Signed, short-lived URIs keyed by media format (e.g. <c>"MP4_HIGH"</c>, <c>"HLS"</c>). Only
        /// present when <see cref="State"/> is <see cref="VideoState.Active"/>. The keys are
        /// <see cref="MediaFormat"/> wire values; this is left string-keyed so unknown future formats
        /// deserialize without throwing. Prefer <see cref="TryGetUris"/> for typed access.
        /// </summary>
        [JsonPropertyName("uris")] public Dictionary<string, AerialViewUris>? Uris { get; set; }

        /// <summary>
        /// Looks up the <see cref="AerialViewUris"/> for a known <see cref="MediaFormat"/>.
        /// </summary>
        /// <param name="format">The media format to retrieve.</param>
        /// <param name="uris">The matching URIs, or <c>null</c> if the format is not present.</param>
        /// <returns><c>true</c> if the format was present; otherwise <c>false</c>.</returns>
        public bool TryGetUris(MediaFormat format, out AerialViewUris? uris)
        {
            if (Uris != null && Uris.TryGetValue(ToWireValue(format), out var found))
            {
                uris = found;
                return true;
            }

            uris = null;
            return false;
        }

        private static string ToWireValue(MediaFormat format) => format switch
        {
            MediaFormat.Image => "IMAGE",
            MediaFormat.Mp4High => "MP4_HIGH",
            MediaFormat.Mp4Medium => "MP4_MEDIUM",
            MediaFormat.Mp4Low => "MP4_LOW",
            MediaFormat.Dash => "DASH",
            MediaFormat.Hls => "HLS",
            _ => "MEDIA_FORMAT_UNSPECIFIED",
        };
    }
}
