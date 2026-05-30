using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>A single open/close period within a place's weekly opening hours.</summary>
    public sealed class OpeningHoursPeriod
    {
        /// <summary>When the place opens.</summary>
        [JsonPropertyName("open")]
        public OpeningHoursPoint? Open { get; set; }

        /// <summary>When the place closes.</summary>
        [JsonPropertyName("close")]
        public OpeningHoursPoint? Close { get; set; }
    }

    /// <summary>A specific point (day and time) at which opening hours change.</summary>
    public sealed class OpeningHoursPoint
    {
        /// <summary>Day of the week, 0 (Sunday) through 6 (Saturday).</summary>
        [JsonPropertyName("day")]
        public int? Day { get; set; }

        /// <summary>Hour of the day, 0–23.</summary>
        [JsonPropertyName("hour")]
        public int? Hour { get; set; }

        /// <summary>Minute of the hour, 0–59.</summary>
        [JsonPropertyName("minute")]
        public int? Minute { get; set; }
    }
}
