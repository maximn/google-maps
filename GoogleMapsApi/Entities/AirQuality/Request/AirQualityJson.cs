using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Entities.AirQuality.Request
{
    /// <summary>
    /// Shared serialization helpers for the Air Quality POST request bodies. Internal: not part of the
    /// public surface.
    /// </summary>
    internal static class AirQualityJson
    {
        /// <summary>Options for serializing request bodies: omit nulls and emit enums by their wire value.</summary>
        public static readonly JsonSerializerOptions BodyOptions = BuildBodyOptions();

        private static JsonSerializerOptions BuildBodyOptions()
        {
            var opts = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
            opts.Converters.Add(new EnumMemberJsonConverterFactory());
            return opts;
        }

        /// <summary>Formats a timestamp as the RFC3339 UTC string the Air Quality API expects.</summary>
        public static string Rfc3339(DateTimeOffset value) =>
            value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
    }
}
