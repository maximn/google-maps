using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNew.Request;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>
    /// Response from the Places API (New) Photo media endpoint when
    /// <c>skipHttpRedirect=true</c> is used — the resolved photo URI rather than binary image bytes.
    /// </summary>
    public sealed class PlacePhotoResponse : IResponseFor<PlacePhotoRequest>
    {
        /// <summary>Resource name of the photo media.</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Direct URI to the photo image.</summary>
        [JsonPropertyName("photoUri")]
        public string? PhotoUri { get; set; }
    }
}
