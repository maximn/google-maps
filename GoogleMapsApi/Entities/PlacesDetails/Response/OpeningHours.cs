using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesDetails.Response
{
    public class OpeningHours
    {
        /// <summary>
        ///  is a boolean value indicating if the Place is open at the current time.
        /// </summary>
        [JsonPropertyName("open_now")]
        public bool OpenNow { get; set; }


        /// <summary>
        /// periods[] is an array of opening periods covering seven days, starting from Sunday, in chronological order. Each period contains:
        /// open contains a pair of day and time objects describing when the Place opens
        /// </summary>
        [JsonPropertyName("periods")]
        public IEnumerable<Period> Periods { get; set; } = null!;
    }
}
