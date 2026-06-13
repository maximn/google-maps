using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AerialView.Response
{
    /// <summary>
    /// Metadata describing a rendered Aerial View video. While processing, only <see cref="VideoId"/>
    /// is typically populated; the remaining fields become available once the video is
    /// <see cref="VideoState.Active"/>.
    /// </summary>
    public sealed class VideoMetadata
    {
        /// <summary>Identifier of the video. Use it for subsequent <c>LookupVideo</c> calls.</summary>
        [JsonPropertyName("videoId")] public string? VideoId { get; set; }

        /// <summary>Date the underlying imagery was captured.</summary>
        [JsonPropertyName("captureDate")] public CaptureDate? CaptureDate { get; set; }

        /// <summary>
        /// Video duration as a <c>google.protobuf.Duration</c> string (seconds with an <c>s</c> suffix,
        /// e.g. <c>"40s"</c>). Use <see cref="DurationValue"/> for a parsed <see cref="TimeSpan"/>.
        /// </summary>
        [JsonPropertyName("duration")] public string? Duration { get; set; }

        /// <summary>
        /// <see cref="Duration"/> parsed into a <see cref="TimeSpan"/>, or <c>null</c> when it is
        /// absent or not in the expected <c>"&lt;seconds&gt;s"</c> form.
        /// </summary>
        [JsonIgnore]
        public TimeSpan? DurationValue
        {
            get
            {
                var raw = Duration;
                if (string.IsNullOrWhiteSpace(raw) || !raw!.EndsWith("s", StringComparison.Ordinal))
                    return null;

                var seconds = raw.Substring(0, raw.Length - 1);
                return double.TryParse(seconds, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
                    ? TimeSpan.FromSeconds(value)
                    : (TimeSpan?)null;
            }
        }
    }
}
