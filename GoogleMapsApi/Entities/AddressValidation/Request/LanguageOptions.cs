using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AddressValidation.Request
{
    /// <summary>
    /// Optional language preferences applied to the validation response. Currently only controls
    /// whether Google returns a transliterated English/Latin form of the address.
    /// </summary>
    public sealed class LanguageOptions
    {
        /// <summary>
        /// If true, also returns the validated address in English using the Latin script
        /// (populated in <c>result.englishLatinAddress</c>).
        /// </summary>
        [JsonPropertyName("returnEnglishLatinAddress")]
        public bool ReturnEnglishLatinAddress { get; set; }
    }
}
