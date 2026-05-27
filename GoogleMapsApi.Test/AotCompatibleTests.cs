#if NET5_0_OR_GREATER
using GoogleMapsApi.Engine.JsonConverters;
using NUnit.Framework;
using System.Reflection;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class AotCompatibleTests
    {
        [Test]
        public void EnumMemberJsonConverterFactory_Covers_AllEnumsInAssembly()
        {
            var convertersField = typeof(EnumMemberJsonConverterFactory)
                .GetField("Converters", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.That(convertersField, Is.Not.Null, "The 'Converters' field could not be found.");

            var converters = convertersField!.GetValue(null)
                as Dictionary<Type, Func<JsonConverter>>;

            Assert.That(converters, Is.Not.Null, "Failed to read the 'Converters' dictionary.");

            var assembly = typeof(GoogleMapsApi.GoogleMaps).Assembly;
            var missingEnums = assembly.GetTypes()
                .Where(t => t.IsEnum)
                .Where(t => !converters!.ContainsKey(t))
                .ToList();

            Assert.That(missingEnums, Is.Empty,
                "The following enums are not registered in EnumMemberJsonConverterFactory.Converters:\n"
                + string.Join("\n", missingEnums.Select(t => $"  - {t.FullName}")));
        }
    }
}
#endif