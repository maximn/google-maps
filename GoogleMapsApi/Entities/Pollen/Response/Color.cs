using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Pollen.Response
{
    /// <summary>An RGBA colour with components in the range [0, 1] (mirrors <c>google.type.Color</c>).</summary>
    public sealed class Color
    {
        /// <summary>Red component, in the range [0, 1].</summary>
        [JsonPropertyName("red")]
        public float? Red { get; set; }

        /// <summary>Green component, in the range [0, 1].</summary>
        [JsonPropertyName("green")]
        public float? Green { get; set; }

        /// <summary>Blue component, in the range [0, 1].</summary>
        [JsonPropertyName("blue")]
        public float? Blue { get; set; }

        /// <summary>Alpha (opacity), in the range [0, 1]. Often omitted, meaning fully opaque.</summary>
        [JsonPropertyName("alpha")]
        public float? Alpha { get; set; }
    }
}
