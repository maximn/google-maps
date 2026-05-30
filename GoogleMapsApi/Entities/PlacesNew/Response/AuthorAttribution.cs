using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>Attribution for the author of a photo or review.</summary>
    public sealed class AuthorAttribution
    {
        /// <summary>Display name of the author.</summary>
        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        /// <summary>URI to the author's profile.</summary>
        [JsonPropertyName("uri")]
        public string? Uri { get; set; }

        /// <summary>URI to the author's profile photo.</summary>
        [JsonPropertyName("photoUri")]
        public string? PhotoUri { get; set; }
    }
}
