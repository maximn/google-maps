using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.AddressValidation.Request
{
    /// <summary>
    /// Request for the Google Address Validation API
    /// (<c>POST https://addressvalidation.googleapis.com/v1:validateAddress</c>).
    /// Validates a postal address, returning a strongly-typed verdict, the address as Google
    /// understands it, geocoding data, and (for US addresses with <see cref="EnableUspsCass"/>)
    /// USPS CASS-processed data.
    /// </summary>
    public sealed class AddressValidationRequest : MapsBaseRequest
    {
        /// <summary>
        /// The postal address to validate. <see cref="PostalAddress.RegionCode"/> is required;
        /// supply <see cref="PostalAddress.AddressLines"/> plus locality/postal information for
        /// meaningful results.
        /// </summary>
        public PostalAddress Address { get; set; } = new PostalAddress();

        /// <summary>
        /// The <c>ResponseId</c> returned by a prior call to this API. Set this when re-validating
        /// the same address after a user has edited it; Google will then group calls into a single
        /// validation session for billing and accuracy purposes.
        /// </summary>
        public string? PreviousResponseId { get; set; }

        /// <summary>
        /// If true, enables USPS CASS processing for US/PR addresses. Adds <c>uspsData</c> to the
        /// response. US-only; ignored for other regions.
        /// </summary>
        public bool EnableUspsCass { get; set; }

        /// <summary>Optional language preferences applied to the validation response.</summary>
        public LanguageOptions? LanguageOptions { get; set; }

        /// <summary>
        /// Session token used to group multiple Address Validation calls into a single
        /// validation session for billing. Generate one UUID per end-user session.
        /// </summary>
        public string? SessionToken { get; set; }

        /// <inheritdoc/>
        public override Uri GetUri()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
                throw new InvalidOperationException("ApiKey is required for the Address Validation API.");

            return new Uri($"https://addressvalidation.googleapis.com/v1:validateAddress?key={Uri.EscapeDataString(ApiKey!)}");
        }

        /// <inheritdoc/>
        protected internal override HttpContent? GetRequestBody()
        {
            var payload = new Payload
            {
                Address = Address,
                PreviousResponseId = string.IsNullOrWhiteSpace(PreviousResponseId) ? null : PreviousResponseId,
                EnableUspsCass = EnableUspsCass ? true : (bool?)null,
                LanguageOptions = LanguageOptions,
                SessionToken = string.IsNullOrWhiteSpace(SessionToken) ? null : SessionToken,
            };
            var json = JsonSerializer.Serialize(payload, BodyOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static readonly JsonSerializerOptions BodyOptions = JsonSerializerConfiguration.CreateRequestBodyOptions();

        private sealed class Payload
        {
            [JsonPropertyName("address")] public PostalAddress? Address { get; set; }
            [JsonPropertyName("previousResponseId")] public string? PreviousResponseId { get; set; }
            [JsonPropertyName("enableUspsCass")] public bool? EnableUspsCass { get; set; }
            [JsonPropertyName("languageOptions")] public LanguageOptions? LanguageOptions { get; set; }
            [JsonPropertyName("sessionToken")] public string? SessionToken { get; set; }
        }
    }
}
