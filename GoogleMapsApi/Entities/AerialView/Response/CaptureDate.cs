using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AerialView.Response
{
    /// <summary>
    /// The date the underlying imagery was captured (a <c>google.type.Date</c>). Any field may be
    /// <c>0</c> when that component is not significant.
    /// </summary>
    public sealed class CaptureDate
    {
        /// <summary>Year of the date (e.g. 2022), or 0 if not significant.</summary>
        [JsonPropertyName("year")] public int Year { get; set; }

        /// <summary>Month of the date (1-12), or 0 if not significant.</summary>
        [JsonPropertyName("month")] public int Month { get; set; }

        /// <summary>Day of the month (1-31), or 0 if not significant.</summary>
        [JsonPropertyName("day")] public int Day { get; set; }
    }
}
