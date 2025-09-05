using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.Common
{
    /// <summary>
    /// An encoded location reference, derived from latitude and longitude coordinates, that represents an area,
    /// 1/8000th of a degree by 1/8000th of a degree (about 14m x 14m at the equator) or smaller.
    /// Plus codes can be used as a replacement for street addresses in places where they do not exist
    /// (where buildings are not numbered or streets are not named).
    /// </summary>
    public class PlusCode
    {
        /// <summary>
        /// A 4 character area code and 6 character or longer local code (849VCWC8+R9).
        /// </summary>
        [JsonPropertyName("global_code")]
        public string GlobalCode { get; set; } = null!;

        /// <summary>
        /// A 6 character or longer local code with an explicit location (CWC8+R9, Mountain View, CA, USA).
        /// May return an empty string if the compound_code is not available.
        /// </summary>
        [JsonPropertyName("compound_code")]
        public string CompoundCode { get; set; } = null!;
    }
}