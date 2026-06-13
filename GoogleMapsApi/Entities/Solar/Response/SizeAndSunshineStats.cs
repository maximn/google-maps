using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Solar.Response
{
    /// <summary>Size and sunshine quantiles for a roof area.</summary>
    public sealed class SizeAndSunshineStats
    {
        /// <summary>The area of the roof or roof segment, in square meters.</summary>
        [JsonPropertyName("areaMeters2")]
        public float AreaMeters2 { get; set; }

        /// <summary>
        /// Quantiles of the pointwise sunniness across the area, in hours per year. The first and
        /// last entries are the min and max; the middle entry is the median.
        /// </summary>
        [JsonPropertyName("sunshineQuantiles")]
        public List<float>? SunshineQuantiles { get; set; }

        /// <summary>The ground footprint area covered, in square meters.</summary>
        [JsonPropertyName("groundAreaMeters2")]
        public float GroundAreaMeters2 { get; set; }
    }
}
