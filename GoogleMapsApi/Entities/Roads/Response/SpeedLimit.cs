using System.Text.Json.Serialization;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Entities.Roads.Response
{
    /// <summary>
    /// Posted speed limit for a single road segment, identified by <see cref="PlaceId"/>.
    /// </summary>
    public sealed class SpeedLimit
    {
        /// <summary>Unique identifier of the road segment this speed limit applies to.</summary>
        [JsonPropertyName("placeId")]
        public string? PlaceId { get; set; }

        /// <summary>The speed limit value, expressed in <see cref="Units"/>.</summary>
        [JsonPropertyName("speedLimit")]
        public double Value { get; set; }

        /// <summary>Unit of <see cref="Value"/>.</summary>
        [JsonPropertyName("units")]
        [JsonConverter(typeof(EnumMemberJsonConverter<SpeedLimitUnits>))]
        public SpeedLimitUnits? Units { get; set; }
    }
}
