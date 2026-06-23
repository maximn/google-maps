using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AirQuality.Response
{
    /// <summary>A pollutant concentration measurement.</summary>
    public sealed class Concentration
    {
        /// <summary>Unit the <see cref="Value"/> is expressed in (e.g. <c>"PARTS_PER_BILLION"</c>).</summary>
        [JsonPropertyName("units")]
        public string? Units { get; set; }

        /// <summary>The measured concentration value.</summary>
        [JsonPropertyName("value")]
        public double Value { get; set; }
    }
}
