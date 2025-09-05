using System.Text.Json;
using GoogleMapsApi.Engine.JsonConverters;

namespace GoogleMapsApi.Engine
{
    /// <summary>
    /// Provides consistent JsonSerializerOptions configuration for Google Maps API serialization.
    /// This ensures both production and test code use the same JSON serialization settings.
    /// </summary>
    public static class JsonSerializerConfiguration
    {
        /// <summary>
        /// Creates a configured JsonSerializerOptions instance with all necessary converters
        /// for Google Maps API entities.
        /// </summary>
        /// <returns>Configured JsonSerializerOptions instance</returns>
        public static JsonSerializerOptions CreateOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Add EnumMemberJsonConverter for all enums with proper EnumMember attribute support
            options.Converters.Add(new EnumMemberJsonConverterFactory());
            
            // Add custom converters
            options.Converters.Add(new PriceLevelJsonConverter());
            options.Converters.Add(new OverviewPolylineJsonConverter());
            
            // Add Duration converters for specific types
            options.Converters.Add(new DurationJsonConverter<GoogleMapsApi.Entities.DistanceMatrix.Response.Duration>());
            options.Converters.Add(new DurationJsonConverter<GoogleMapsApi.Entities.Directions.Response.Duration>());

            return options;
        }
    }
}