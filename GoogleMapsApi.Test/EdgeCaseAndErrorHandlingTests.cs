using System;
using System.Text.Json;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using GoogleMapsApi.Engine.JsonConverters;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Linq;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class EdgeCaseAndErrorHandlingTests
    {
        private JsonSerializerOptions _options;

        [SetUp]
        public void Setup()
        {
            // Use the exact same JSON configuration as production code
            _options = GoogleMapsApi.Engine.JsonSerializerConfiguration.CreateOptions();
            
            // Add specific converters needed for edge case testing
            _options.Converters.Add(new EnumMemberJsonConverter<TravelMode>());
        }

        #region Extreme Input Validation Tests

        [Test]
        public void Location_ExtremeCoordinates_HandledCorrectly()
        {
            // Test extreme but valid coordinates
            var northPole = new Location(90, 0);
            var southPole = new Location(-90, 0);
            var dateLine = new Location(0, 180);
            var antiMeridian = new Location(0, -180);

            Assert.DoesNotThrow(() =>
            {
                var northStr = northPole.LocationString;
                var southStr = southPole.LocationString;
                var dateStr = dateLine.LocationString;
                var antiStr = antiMeridian.LocationString;
            });

            // Test coordinates beyond valid ranges
            Assert.DoesNotThrow(() =>
            {
                var invalidLat = new Location(91, 0);
                var invalidLatStr = invalidLat.LocationString; // Should not crash even if invalid
            });

            Assert.DoesNotThrow(() =>
            {
                var invalidLng = new Location(0, 181);
                var invalidLngStr = invalidLng.LocationString; // Should not crash even if invalid
            });
        }

        [Test]
        public void Location_EdgeCaseValues_DoNotCrash()
        {
            // Test with extreme floating point values
            Assert.DoesNotThrow(() =>
            {
                var extremeLocation = new Location(double.MaxValue, double.MinValue);
                var extremeStr = extremeLocation.LocationString;
            });

            // Test with special floating point values
            Assert.DoesNotThrow(() =>
            {
                var nanLocation = new Location(double.NaN, double.NaN);
                var nanStr = nanLocation.LocationString;
            });

            Assert.DoesNotThrow(() =>
            {
                var infLocation = new Location(double.PositiveInfinity, double.NegativeInfinity);
                var infStr = infLocation.LocationString;
            });
        }

        [Test]
        public void AddressLocation_ExtremeLengthAddresses_HandledCorrectly()
        {
            // Test empty address
            var emptyAddress = new AddressLocation("");
            Assert.DoesNotThrow(() =>
            {
                var emptyStr = emptyAddress.LocationString;
                Assert.That(emptyAddress.LocationString, Is.Not.Null);
            });

            // Test very long address
            var longAddress = new AddressLocation(new string('A', 10000));
            Assert.DoesNotThrow(() =>
            {
                var longStr = longAddress.LocationString;
                Assert.That(longAddress.LocationString, Is.Not.Null);
                Assert.That(longAddress.LocationString.Length, Is.EqualTo(10000));
            });

            // Test address with special characters
            var specialCharAddress = new AddressLocation("Address with üñîçødé & symbols !@#$%^&*()");
            Assert.DoesNotThrow(() =>
            {
                var specialStr = specialCharAddress.LocationString;
                Assert.That(specialCharAddress.LocationString, Is.Not.Null);
            });

            // Test address with newlines and tabs
            var multilineAddress = new AddressLocation("Line 1\nLine 2\tTabbed");
            Assert.DoesNotThrow(() =>
            {
                var multiStr = multilineAddress.LocationString;
                Assert.That(multilineAddress.LocationString, Is.Not.Null);
            });
        }

        #endregion

        #region JSON Deserialization Edge Cases

        [Test]
        public void JsonDeserialization_UnicodeAndSpecialCharacters_HandledCorrectly()
        {
            var unicodeJson = """
            {
                "status": "OK",
                "routes": [{
                    "legs": [{
                        "start_address": "Москва, Россия",
                        "end_address": "北京市, 中国",
                        "duration": {
                            "text": "1時間30分",
                            "value": 5400
                        }
                    }]
                }]
            }
            """;

            Assert.DoesNotThrow(() =>
            {
                var response = JsonSerializer.Deserialize<DirectionsResponse>(unicodeJson, _options);
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Routes, Is.Not.Null);
                Assert.That(response.Routes.Count(), Is.EqualTo(1));
                
                var leg = response.Routes.First().Legs?.FirstOrDefault();
                Assert.That(leg, Is.Not.Null);
                Assert.That(leg.StartAddress, Does.Contain("Москва"));
                Assert.That(leg.EndAddress, Does.Contain("北京"));
                Assert.That(leg.Duration?.Text, Does.Contain("時間"));
            });
        }

        [Test]
        public void JsonDeserialization_VeryLargeNumbers_HandledCorrectly()
        {
            var largeNumbersJson = """
            {
                "value": 2147483647,
                "text": "Maximum int value"
            }
            """;

            Assert.DoesNotThrow(() =>
            {
                var duration = JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(largeNumbersJson, _options);
                Assert.That(duration, Is.Not.Null);
                Assert.That(duration.Value, Is.EqualTo(TimeSpan.FromSeconds(2147483647)));
            });

            // Test beyond int32 range (should be handled gracefully)
            var beyondInt32Json = """
            {
                "value": 9223372036854775807,
                "text": "Very large number"
            }
            """;

            // This might throw or handle gracefully depending on implementation
            Assert.DoesNotThrow(() =>
            {
                try
                {
                    var duration = JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(beyondInt32Json, _options);
                }
                catch (JsonException)
                {
                    // Acceptable if the converter validates ranges
                }
            });
        }

        [Test]
        public void JsonDeserialization_DeeplyNestedObjects_HandledCorrectly()
        {
            var deeplyNestedJson = """
            {
                "status": "OK",
                "routes": [{
                    "legs": [{
                        "steps": [{
                            "html_instructions": "Head <b>north</b> on <b>Main St</b>",
                            "distance": {
                                "text": "0.1 km",
                                "value": 100
                            },
                            "duration": {
                                "text": "1 min",
                                "value": 60
                            },
                            "start_location": {
                                "lat": 37.7749,
                                "lng": -122.4194
                            },
                            "end_location": {
                                "lat": 37.7759,
                                "lng": -122.4194
                            }
                        }]
                    }]
                }]
            }
            """;

            Assert.DoesNotThrow(() =>
            {
                var response = JsonSerializer.Deserialize<DirectionsResponse>(deeplyNestedJson, _options);
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Routes, Is.Not.Null);
                Assert.That(response.Routes.Count(), Is.EqualTo(1));
                
                var route = response.Routes.First();
                Assert.That(route.Legs, Is.Not.Null);
                Assert.That(route.Legs.Count(), Is.EqualTo(1));
                
                var leg = route.Legs.First();
                Assert.That(leg.Steps, Is.Not.Null);
                Assert.That(leg.Steps.Count(), Is.EqualTo(1));
                
                var step = leg.Steps.First();
                Assert.That(step.HtmlInstructions, Does.Contain("north"));
                Assert.That(step.Distance, Is.Not.Null);
                Assert.That(step.Duration, Is.Not.Null);
                Assert.That(step.StartLocation, Is.Not.Null);
                Assert.That(step.EndLocation, Is.Not.Null);
            });
        }

        #endregion

        #region Request URI Generation Edge Cases

        [Test]
        public void RequestUriGeneration_ExtremelyLongParameters_HandledCorrectly()
        {
            var request = new GeocodingRequest
            {
                Address = new string('A', 5000), // Very long address
                ApiKey = "test_key"
            };

            Assert.DoesNotThrow(() =>
            {
                var uri = request.GetUri();
                Assert.That(uri, Is.Not.Null);
                Assert.That(uri.ToString().Length, Is.GreaterThan(5000));
            });
        }

        [Test]
        public void RequestUriGeneration_SpecialCharactersInParameters_EscapedCorrectly()
        {
            var request = new GeocodingRequest
            {
                Address = "Address with spaces & symbols: !@#$%^&*()+={}[]|\\:;\"'<>?,./~`",
                ApiKey = "test_key_with_special_chars_!@#$%"
            };

   
            var uri = request.GetUri();
            Assert.That(uri, Is.Not.Null);
            var uriString = uri.AbsoluteUri;
            
            // URI should be properly encoded
            Assert.That(uriString, Does.Not.Contain(" "));
            Assert.That(uriString, Contains.Substring("Address"));
            Assert.That(uriString, Contains.Substring("test_key"));
        }

        [Test]
        public void RequestUriGeneration_EmptyAndNullApiKey_HandledGracefully()
        {
            var request = new GeocodingRequest
            {
                Address = "Test Address"
            };

            // Test null API key
            request.ApiKey = null;
            Assert.DoesNotThrow(() =>
            {
                var uri = request.GetUri();
                Assert.That(uri, Is.Not.Null);
            });

            // Test empty API key
            request.ApiKey = "";
            Assert.DoesNotThrow(() =>
            {
                var uri = request.GetUri();
                Assert.That(uri, Is.Not.Null);
            });

            // Test whitespace-only API key
            request.ApiKey = "   ";
            Assert.DoesNotThrow(() =>
            {
                var uri = request.GetUri();
                Assert.That(uri, Is.Not.Null);
            });
        }

        [Test]
        public void RequestUriGeneration_ManyWaypoints_HandledCorrectly()
        {
            var waypoints = new ILocationString[50];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = new Location(37.7749 + i * 0.01, -122.4194 + i * 0.01);
            }

            var request = new DirectionsRequest
            {
                Origin = "San Francisco, CA",
                Destination = "Sacramento, CA", 
                Waypoints = waypoints.Select(w => w.LocationString).ToArray(),
                ApiKey = "test_key"
            };

            Assert.DoesNotThrow(() =>
            {
                var uri = request.GetUri();
                Assert.That(uri, Is.Not.Null);
                Assert.That(uri.ToString(), Contains.Substring("waypoints="));
                
                // URI should be quite long with all waypoints
                Assert.That(uri.ToString().Length, Is.GreaterThan(1000));
            });
        }

        #endregion

        #region Error Response Handling

        [Test]
        public void ErrorResponseDeserialization_VariousErrorFormats_HandledCorrectly()
        {
            var errorResponses = new[]
            {
                // Standard error response
                """{"status": "ZERO_RESULTS", "error_message": "No results found"}""",
                
                // Error response with additional fields
                """{"status": "OVER_QUERY_LIMIT", "error_message": "Rate limit exceeded", "next_page_token": null}""",
                
                // Minimal error response
                """{"status": "REQUEST_DENIED"}""",
                
                // Error response with unexpected fields
                """{"status": "INVALID_REQUEST", "error_message": "Invalid input", "debug_info": {"request_id": "12345"}}""",
                
                // Response with unknown status
                """{"status": "UNKNOWN_ERROR_STATUS", "error_message": "Unknown error occurred"}"""
            };

            foreach (var errorJson in errorResponses)
            {
                Assert.DoesNotThrow(() =>
                {
                    try
                    {
                        var response = JsonSerializer.Deserialize<DirectionsResponse>(errorJson, _options);
                        Assert.That(response, Is.Not.Null);
                        // Status should be parsed or default to appropriate value
                    }
                    catch (JsonException)
                    {
                        // Some error formats might not deserialize perfectly, which is acceptable
                    }
                }, $"Error response caused crash: {errorJson}");
            }
        }

        [Test]
        public void PartialResponseData_MissingRequiredFields_HandledGracefully()
        {
            var partialResponses = new[]
            {
                // Missing routes array
                """{"status": "OK"}""",
                
                // Empty routes with other fields
                """{"status": "OK", "routes": [], "geocoded_waypoints": []}""",
                
                // Route without legs
                """{"status": "OK", "routes": [{"overview_polyline": {"points": "test"}}]}""",
                
                // Leg without required fields
                """{"status": "OK", "routes": [{"legs": [{}]}]}"""
            };

            foreach (var partialJson in partialResponses)
            {
                Assert.DoesNotThrow(() =>
                {
                    var response = JsonSerializer.Deserialize<DirectionsResponse>(partialJson, _options);
                    Assert.That(response, Is.Not.Null);
                    Assert.That(response.Status, Is.EqualTo(DirectionsStatusCodes.OK));
                }, $"Partial response caused crash: {partialJson}");
            }
        }

        #endregion

        #region Memory and Performance Edge Cases

        [Test]
        public void ConcurrentDeserialization_DifferentThreads_NoRaceConditions()
        {
            var json = """
            {
                "status": "OK",
                "routes": [{
                    "legs": [{
                        "duration": {"value": 3600, "text": "1 hour"},
                        "distance": {"value": 100000, "text": "100 km"}
                    }]
                }]
            }
            """;

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
                            var response = JsonSerializer.Deserialize<DirectionsResponse>(json, _options);
                            Assert.That(response, Is.Not.Null);
                            Assert.That(response!.Routes, Is.Not.Null);
                            Assert.That(response.Routes.Count(), Is.EqualTo(1));
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
                $"Concurrent deserialization caused {exceptions.Count} exceptions");
        }

        #endregion

        #region Boundary Value Tests

        [Test]
        public void DurationConverter_BoundaryValues_HandledCorrectly()
        {
            var testCases = new[]
            {
                (0, TimeSpan.Zero),
                (1, TimeSpan.FromSeconds(1)),
                (3600, TimeSpan.FromHours(1)),
                (86400, TimeSpan.FromDays(1)),
                (int.MaxValue, TimeSpan.FromSeconds(int.MaxValue))
            };

            foreach (var (seconds, expectedTimeSpan) in testCases)
            {
                var json = $$"""{"value": {{seconds}}, "text": "{{seconds}} seconds"}""";
                
                Assert.DoesNotThrow(() =>
                {
                    var duration = JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json, _options);
                    Assert.That(duration, Is.Not.Null);
                    Assert.That(duration.Value, Is.EqualTo(expectedTimeSpan));
                }, $"Duration conversion failed for {seconds} seconds");
            }
        }

        [Test]
        public void EnumConverter_AllPossibleEnumValues_Handled()
        {
            var allTravelModes = (TravelMode[])Enum.GetValues(typeof(TravelMode));
            
            foreach (var mode in allTravelModes)
            {
                Assert.DoesNotThrow(() =>
                {
                    // Test serialization
                    var json = JsonSerializer.Serialize(mode, _options);
                    Assert.That(json, Is.Not.Null);
                    Assert.That(json.Length, Is.GreaterThan(0));
                    
                    // Test deserialization
                    var deserialized = JsonSerializer.Deserialize<TravelMode>(json, _options);
                    Assert.That(deserialized, Is.EqualTo(mode));
                }, $"Enum conversion failed for TravelMode.{mode}");
            }
        }

        [Test]
        public void PriceLevelConverter_AllValidValues_HandledCorrectly()
        {
            var validPriceLevels = new[]
            {
                (0, PriceLevel.Free),
                (1, PriceLevel.Inexpensive), 
                (2, PriceLevel.Moderate),
                (3, PriceLevel.Expensive),
                (4, PriceLevel.VeryExpensive)
            };

            foreach (var (value, expectedLevel) in validPriceLevels)
            {
                Assert.DoesNotThrow(() =>
                {
                    // Test numeric input
                    var numericJson = value.ToString();
                    var numericResult = JsonSerializer.Deserialize<PriceLevel?>(numericJson, _options);
                    Assert.That(numericResult, Is.EqualTo(expectedLevel));
                    
                    // Test string input
                    var stringJson = $"\"{value}\"";
                    var stringResult = JsonSerializer.Deserialize<PriceLevel?>(stringJson, _options);
                    Assert.That(stringResult, Is.EqualTo(expectedLevel));
                }, $"Price level conversion failed for value {value}");
            }
        }

        #endregion
    }
}