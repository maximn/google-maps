using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AerialView.Response
{
    /// <summary>
    /// A pair of signed, short-lived URIs for a single <see cref="MediaFormat"/> of an Aerial View
    /// video — one for each aspect ratio. The URIs expire, so fetch them fresh via <c>LookupVideo</c>
    /// rather than caching them.
    /// </summary>
    public sealed class AerialViewUris
    {
        /// <summary>Signed URI for the landscape (16:9) rendition.</summary>
        [JsonPropertyName("landscapeUri")] public string? LandscapeUri { get; set; }

        /// <summary>Signed URI for the portrait (9:16) rendition.</summary>
        [JsonPropertyName("portraitUri")] public string? PortraitUri { get; set; }
    }
}
