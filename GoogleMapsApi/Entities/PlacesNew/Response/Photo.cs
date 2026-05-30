using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>
    /// A photo of a place. Use <see cref="Name"/> as
    /// <see cref="GoogleMapsApi.Entities.PlacesNew.Request.PlacePhotoRequest.PhotoName"/> to resolve a
    /// usable image URI.
    /// </summary>
    public sealed class Photo
    {
        /// <summary>Resource name of the photo (e.g. <c>places/X/photos/Y</c>).</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Maximum available width of the photo, in pixels.</summary>
        [JsonPropertyName("widthPx")]
        public int? WidthPx { get; set; }

        /// <summary>Maximum available height of the photo, in pixels.</summary>
        [JsonPropertyName("heightPx")]
        public int? HeightPx { get; set; }

        /// <summary>Attributions required to be shown when displaying this photo.</summary>
        [JsonPropertyName("authorAttributions")]
        public List<AuthorAttribution>? AuthorAttributions { get; set; }
    }
}
