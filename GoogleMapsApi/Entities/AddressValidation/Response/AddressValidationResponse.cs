using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.AddressValidation.Request;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.AddressValidation.Response
{
    /// <summary>
    /// Response from the Google Address Validation API. Contains the validation result and a
    /// response identifier that callers should pass back as
    /// <see cref="AddressValidationRequest.PreviousResponseId"/> when re-validating the same
    /// address after a user edit.
    /// </summary>
    public sealed class AddressValidationResponse : IResponseFor<AddressValidationRequest>
    {
        /// <summary>The validation result.</summary>
        [JsonPropertyName("result")]
        public ValidationResult? Result { get; set; }

        /// <summary>
        /// Identifier of this validation response. Pass this as <c>PreviousResponseId</c> on the next
        /// validation call for the same address to group the calls into a single validation session.
        /// </summary>
        [JsonPropertyName("responseId")]
        public string? ResponseId { get; set; }
    }
}
