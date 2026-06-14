using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>
    /// A whole or partial calendar date (mirrors <c>google.type.Date</c>); zero components mean
    /// "not specified".
    /// </summary>
    public sealed class Date
    {
        /// <summary>Year of the date, or 0 for a date without a year.</summary>
        [JsonPropertyName("year")]
        public int Year { get; set; }

        /// <summary>Month of the date (1–12), or 0 for a date without a month.</summary>
        [JsonPropertyName("month")]
        public int Month { get; set; }

        /// <summary>Day of the month (1–31), or 0 for a date without a day.</summary>
        [JsonPropertyName("day")]
        public int Day { get; set; }
    }
}
