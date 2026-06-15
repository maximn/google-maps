using System.Text.Json.Serialization;

namespace GoogleMapsApi.Test.Vcr
{
    /// <summary>
    /// One recorded HTTP request/response pair in a cassette. The response body is base64-encoded so a
    /// single shape covers both textual (JSON) and binary (GeoTIFF/photo) payloads. The stored
    /// <see cref="Url"/> and <see cref="RequestBody"/> have their API key/signature redacted, and matching
    /// redacts the incoming request the same way — so a replay request carrying a placeholder key still
    /// matches a recording made with a real key.
    /// </summary>
    public sealed class RecordedInteraction
    {
        public string Method { get; set; } = "GET";

        public string Url { get; set; } = string.Empty;

        /// <summary>Redacted request body for POST calls; <c>null</c> for GET.</summary>
        public string? RequestBody { get; set; }

        public int StatusCode { get; set; }

        public string? ContentType { get; set; }

        public string BodyBase64 { get; set; } = string.Empty;

        /// <summary>Stable match key: method + redacted URL + normalized redacted body.</summary>
        [JsonIgnore]
        public string MatchKey => $"{Method} {Url}\n{RequestBody}";
    }
}
