using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>Opening-hours information for a place.</summary>
    public sealed class OpeningHours
    {
        /// <summary>Whether the place is currently open.</summary>
        [JsonPropertyName("openNow")]
        public bool? OpenNow { get; set; }

        /// <summary>The periods this place is open during the week.</summary>
        [JsonPropertyName("periods")]
        public List<OpeningHoursPeriod>? Periods { get; set; }

        /// <summary>Localized, human-readable descriptions of opening hours per day.</summary>
        [JsonPropertyName("weekdayDescriptions")]
        public List<string>? WeekdayDescriptions { get; set; }
    }
}
