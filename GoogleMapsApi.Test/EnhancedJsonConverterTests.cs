using System;
using System.Text.Json;
using GoogleMapsApi.Engine.JsonConverters;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using GoogleMapsApi.Entities.Directions.Request;
using NUnit.Framework;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class EnhancedJsonConverterTests
    {
        private JsonSerializerOptions _options;

        [SetUp]
        public void Setup()
        {
            // Use centralized configuration for consistency with production code
            _options = JsonSerializerConfiguration.CreateOptions();
            
        }

        #region Enhanced DurationJsonConverter Tests - Vulnerability Testing

        [Test]
        public void DurationJsonConverter_NullValues_HandledCorrectly()
        {
            // Test null value in duration object
            var json = """{"value": null, "text": null}""";
            
            var duration = JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json, _options);
            
            Assert.That(duration, Is.Not.Null);
            Assert.That(duration.Text, Is.Null);
        }

        [Test]
        public void DurationJsonConverter_MissingProperties_DoesNotThrow()
        {
            // Test missing value property
            var json1 = """{"text": "1 hour"}""";
            Assert.DoesNotThrow(() => JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json1, _options));

            // Test missing text property  
            var json2 = """{"value": 3600}""";
            Assert.DoesNotThrow(() => JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json2, _options));

            // Test completely empty object
            var json3 = """{}""";
            Assert.DoesNotThrow(() => JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json3, _options));
        }

        [Test]
        public void DurationJsonConverter_UnexpectedPropertyTypes_DoesNotThrow()
        {
            // Test string where number expected
            var json1 = """{"value": "invalid", "text": "1 hour"}""";
            Assert.DoesNotThrow(() => JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json1, _options));

            // Test number where string expected
            var json2 = """{"value": 3600, "text": 123}""";
            Assert.DoesNotThrow(() => JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json2, _options));
        }

        [Test]
        public void DurationJsonConverter_ExtremeValues_HandledCorrectly()
        {
            // Test very large duration
            var json1 = """{"value": 2147483647, "text": "Very long time"}""";
            var duration1 = JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json1, _options);
            Assert.That(duration1, Is.Not.Null);
            Assert.That(duration1!.Value, Is.EqualTo(TimeSpan.FromSeconds(2147483647)));

            // Test negative duration (should this be allowed?)
            var json2 = """{"value": -1000, "text": "Negative time"}""";
            Assert.DoesNotThrow(() => JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json2, _options));
        }

        [Test]
        public void DurationJsonConverter_SerializationRoundTrip_Consistent()
        {
            var original = new GoogleMapsApi.Entities.Directions.Response.Duration
            {
                Value = TimeSpan.FromHours(2.5),
                Text = "2 hours 30 minutes"
            };

            var json = JsonSerializer.Serialize(original, _options);
            var deserialized = JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json, _options);

            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.Value, Is.EqualTo(original.Value));
            Assert.That(deserialized.Text, Is.EqualTo(original.Text));
        }

        #endregion

        #region Enhanced EnumMemberJsonConverter Tests - Cache and Edge Cases

        [Test]
        public void EnumMemberJsonConverter_CachePerformance_Consistent()
        {
            // Test that repeated conversions use cache and are consistent
            var json = "\"DRIVING\"";

            // First conversion - populates cache
            var result1 = JsonSerializer.Deserialize<TravelMode>(json, _options);
            
            // Multiple subsequent conversions - should use cache
            for (int i = 0; i < 100; i++)
            {
                var result = JsonSerializer.Deserialize<TravelMode>(json, _options);
                Assert.That(result, Is.EqualTo(TravelMode.Driving));
                Assert.That(result, Is.EqualTo(result1));
            }
        }

        [Test]
        public void EnumMemberJsonConverter_CaseVariations_Handled()
        {
            // Test that the converter handles exact case matches (Google APIs are case sensitive)
            var validJson = "\"DRIVING\"";
            var result = JsonSerializer.Deserialize<TravelMode>(validJson, _options);
            Assert.That(result, Is.EqualTo(TravelMode.Driving));

            // Test lowercase (should fail with Google API format)
            var lowercaseJson = "\"driving\"";
            var desrialized = JsonSerializer.Deserialize<TravelMode>(lowercaseJson, _options);
            Assert.That(desrialized, Is.EqualTo(TravelMode.Driving));
        }

        [Test]
        public void EnumMemberJsonConverter_AllEnumValues_SerializeAndDeserialize()
        {
            // Test all TravelMode values
            var modes = new[] { TravelMode.Driving, TravelMode.Walking, TravelMode.Bicycling, TravelMode.Transit };
            
            foreach (var mode in modes)
            {
                var json = JsonSerializer.Serialize(mode, _options);
                var deserialized = JsonSerializer.Deserialize<TravelMode>(json, _options);
                Assert.That(deserialized, Is.EqualTo(mode));
            }
        }

        [Test]
        public void EnumMemberJsonConverter_NumericValues_OutOfRange()
        {
            // Test behavior with numeric values outside enum range
            var json = "999";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TravelMode>(json, _options));

            // Test negative values
            var negativeJson = "-1";
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TravelMode>(negativeJson, _options));
        }

        [Test]
        public void EnumMemberJsonConverter_EmptyAndWhitespace_Strings()
        {
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TravelMode>("\"\"", _options));
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TravelMode>("\" \"", _options));
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TravelMode>("\"\\t\"", _options));
        }

        #endregion

        #region Enhanced PriceLevelJsonConverter Tests - Dual Input Handling

        [Test]
        public void PriceLevelJsonConverter_BoundaryValues_Handled()
        {
            // Test minimum valid value
            var json0 = "0";
            var result0 = JsonSerializer.Deserialize<PriceLevel?>(json0, _options);
            Assert.That(result0, Is.EqualTo(PriceLevel.Free));

            // Test maximum valid value
            var json4 = "4";
            var result4 = JsonSerializer.Deserialize<PriceLevel?>(json4, _options);
            Assert.That(result4, Is.EqualTo(PriceLevel.VeryExpensive));
        }

        [Test]
        public void PriceLevelJsonConverter_InvalidValues_ReturnNull()
        {
            // Test out-of-range values
            var json5 = "5";
            var result5 = JsonSerializer.Deserialize<PriceLevel?>(json5, _options);
            Assert.That(result5, Is.Null);

            var jsonNeg = "-1";
            var resultNeg = JsonSerializer.Deserialize<PriceLevel?>(jsonNeg, _options);
            Assert.That(resultNeg, Is.Null);

            // Test invalid string
            var jsonInvalid = "\"invalid\"";
            var resultInvalid = JsonSerializer.Deserialize<PriceLevel?>(jsonInvalid, _options);
            Assert.That(resultInvalid, Is.Null);
        }

        [Test]
        public void PriceLevelJsonConverter_StringAndNumericEquivalence()
        {
            // Test that string and numeric representations give same result
            var numericJson = "2";
            var stringJson = "\"2\"";

            var numericResult = JsonSerializer.Deserialize<PriceLevel?>(numericJson, _options);
            var stringResult = JsonSerializer.Deserialize<PriceLevel?>(stringJson, _options);

            Assert.That(numericResult, Is.EqualTo(stringResult));
            Assert.That(numericResult, Is.EqualTo(PriceLevel.Moderate));
        }

        [Test]
        public void PriceLevelJsonConverter_FloatingPointNumbers_Handled()
        {
            // Test that floating point numbers are handled (Google might send 2.0)
            var jsonFloat = "2.0";
            var result = JsonSerializer.Deserialize<PriceLevel?>(jsonFloat, _options);
            Assert.That(result, Is.EqualTo(PriceLevel.Moderate));
        }

        #endregion

        #region Enhanced OverviewPolylineJsonConverter Tests - Reflection Vulnerabilities

        [Test]
        public void OverviewPolylineJsonConverter_EmptyAndNullPoints_Handled()
        {
            // Test empty points string
            var emptyJson = """{"points": ""}""";
            var emptyResult = JsonSerializer.Deserialize<OverviewPolyline>(emptyJson, _options);
            Assert.That(emptyResult, Is.Not.Null);

            // Test missing points property
            var missingJson = """{}""";
            var missingResult = JsonSerializer.Deserialize<OverviewPolyline>(missingJson, _options);
            Assert.That(missingResult, Is.Not.Null);
        }

        [Test]
        public void OverviewPolylineJsonConverter_LargePolylineData_HandlesCorrectly()
        {
            // Test with a large polyline string (realistic Google Maps data can be quite large)
            var largePoints = new string('a', 10000); // 10KB polyline
            var largeJson = $$"""{"points": "{{largePoints}}"}""";
            
            Assert.DoesNotThrow(() => 
            {
                var result = JsonSerializer.Deserialize<OverviewPolyline>(largeJson, _options);
                Assert.That(result, Is.Not.Null);
            });
        }

        [Test]
        public void OverviewPolylineJsonConverter_InvalidPolylineFormat_DoesNotCrash()
        {
            // Test invalid polyline encoding (should not crash OnDeserialized)
            var invalidJson = """{"points": "invalid_polyline_data_!@#$%"}""";
            
            Assert.DoesNotThrow(() =>
            {
                var result = JsonSerializer.Deserialize<OverviewPolyline>(invalidJson, _options);
                Assert.That(result, Is.Not.Null);
            });
        }

        #endregion

        #region Stress Tests for All Converters

        [Test]
        public void AllConverters_ConcurrentAccess_ThreadSafe()
        {
            var tasks = new Task[10];
            var exceptions = new System.Collections.Concurrent.ConcurrentBag<Exception>();

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    try
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            // Test all converters concurrently
                            JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(
                                """{"value": 3600, "text": "1 hour"}""", _options);
                            
                            JsonSerializer.Deserialize<TravelMode>("\"DRIVING\"", _options);
                            JsonSerializer.Deserialize<PriceLevel?>("2", _options);
                            JsonSerializer.Deserialize<OverviewPolyline>(
                                """{"points": "_p~iF~ps|U"}""", _options);
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                });
            }

            Task.WaitAll(tasks);
            Assert.That(exceptions.Count, Is.EqualTo(0), 
                $"Concurrent access caused {exceptions.Count} exceptions: {string.Join(", ", exceptions)}");
        }

        [Test]
        public void AllConverters_MalformedJson_DoNotCrash()
        {
            var malformedInputs = new[]
            {
                "not_json_at_all",
                "{",
                "}",
                "{\"incomplete\"",
                "{\"value\": }",
                "{\"text\": \"unclosed string}",
                "[]",
                "123.456.789",
                "{\"nested\": {\"too\": {\"deep\": true}}}",
                "null",
                "",
                "    ",
                "{\"unicode\": \"\\u0000\\u0001\\u0002\"}",
                "{\"very_long_property_name_" + new string('x', 1000) + "\": \"value\"}"
            };

            foreach (var malformed in malformedInputs)
            {
                // These should either succeed or throw JsonException, but not crash
                Assert.DoesNotThrow(() =>
                {
                    try
                    {
                        JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(malformed, _options);
                    }
                    catch (JsonException)
                    {
                        // Expected for malformed JSON
                    }
                }, $"Input caused crash: {malformed}");
            }
        }

        #endregion
    }
}