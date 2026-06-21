using System;

namespace GoogleMapsApi.Entities.AirQuality.Request
{
    /// <summary>
    /// A half-open-to-inclusive time range for <see cref="ForecastRequest.Period"/> and
    /// <see cref="HistoryRequest.Period"/>. Both bounds are sent to the API as RFC3339 UTC timestamps.
    /// </summary>
    public sealed class Interval
    {
        /// <summary>Start of the range.</summary>
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>End of the range (inclusive).</summary>
        public DateTimeOffset? EndTime { get; set; }
    }
}
