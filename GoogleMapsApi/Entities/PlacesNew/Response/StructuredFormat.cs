using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>A prediction broken into main and secondary text for display.</summary>
    public sealed class StructuredFormat
    {
        /// <summary>Main (primary) text of the prediction.</summary>
        [JsonPropertyName("mainText")]
        public FormattableText? MainText { get; set; }

        /// <summary>Secondary (subordinate) text of the prediction.</summary>
        [JsonPropertyName("secondaryText")]
        public FormattableText? SecondaryText { get; set; }
    }
}
