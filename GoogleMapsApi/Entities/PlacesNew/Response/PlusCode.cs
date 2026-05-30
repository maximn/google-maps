using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>Open Location Code (plus code) for a place.</summary>
    public sealed class PlusCode
    {
        /// <summary>Global (full) plus code.</summary>
        [JsonPropertyName("globalCode")]
        public string? GlobalCode { get; set; }

        /// <summary>Compound plus code (shortened, with a reference locality).</summary>
        [JsonPropertyName("compoundCode")]
        public string? CompoundCode { get; set; }
    }
}
