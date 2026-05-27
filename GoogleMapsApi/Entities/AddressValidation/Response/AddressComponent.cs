using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AddressValidation.Response
{
    /// <summary>
    /// A single component (street, locality, etc.) of the validated address along with
    /// confirmation and transformation flags.
    /// </summary>
    public sealed class AddressComponent
    {
        /// <summary>The localized text + language of this component.</summary>
        [JsonPropertyName("componentName")]
        public ComponentName? ComponentName { get; set; }

        /// <summary>Component type (e.g. <c>"street_number"</c>, <c>"route"</c>, <c>"locality"</c>).</summary>
        [JsonPropertyName("componentType")]
        public string? ComponentType { get; set; }

        /// <summary>Confidence that this component is correctly identified.</summary>
        [JsonPropertyName("confirmationLevel")]
        public ConfirmationLevel ConfirmationLevel { get; set; }

        /// <summary>True if Google inferred this component (it was not in the input).</summary>
        [JsonPropertyName("inferred")]
        public bool Inferred { get; set; }

        /// <summary>True if a spelling correction was applied.</summary>
        [JsonPropertyName("spellCorrected")]
        public bool SpellCorrected { get; set; }

        /// <summary>True if the component was replaced with a different one (e.g. correcting postal code).</summary>
        [JsonPropertyName("replaced")]
        public bool Replaced { get; set; }

        /// <summary>True if the component is in an unexpected position or value.</summary>
        [JsonPropertyName("unexpected")]
        public bool Unexpected { get; set; }
    }

    /// <summary>Localized text content of an address component.</summary>
    public sealed class ComponentName
    {
        /// <summary>The component value, localized to <see cref="LanguageCode"/>.</summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        /// <summary>BCP-47 language code of <see cref="Text"/>.</summary>
        [JsonPropertyName("languageCode")]
        public string? LanguageCode { get; set; }
    }
}
