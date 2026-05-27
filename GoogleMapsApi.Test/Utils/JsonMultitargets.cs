using GoogleMapsApi.Engine;
using System.Text.Json;

#if NET7_0_OR_GREATER
using System.Text.Json.Serialization.Metadata;
#endif

namespace GoogleMapsApi.Test.Utils
{
    public static class JsonMultitargets
    {
        private static JsonSerializerOptions _options => GoogleMapsJsonSerializerContext.Default.Options;

        public static T Deserialize<T>(string json)
        {
#if NET7_0_OR_GREATER
            var typeResolver = (JsonTypeInfo<T>)GoogleMapsJsonSerializerContext.Default.GetTypeInfo(typeof(T))!;
            var result = JsonSerializer.Deserialize(json, typeResolver);
            return result!;
#else
            var duration = JsonSerializer.Deserialize<T>(json, _options);
            return duration!;
#endif
        }

        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, _options);
        }
    }
}
