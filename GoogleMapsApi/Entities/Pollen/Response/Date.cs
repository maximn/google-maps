using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Pollen.Response
{
    /// <summary>A calendar date with no time zone (mirrors <c>google.type.Date</c>).</summary>
    public sealed class Date
    {
        /// <summary>Year of the date.</summary>
        [JsonPropertyName("year")]
        public int Year { get; set; }

        /// <summary>Month of the date, 1-12.</summary>
        [JsonPropertyName("month")]
        public int Month { get; set; }

        /// <summary>Day of the month, 1-31.</summary>
        [JsonPropertyName("day")]
        public int Day { get; set; }
    }
}
