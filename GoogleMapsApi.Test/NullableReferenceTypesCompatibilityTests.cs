using System;
using System.Text.Json;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using GoogleMapsApi.Entities.DistanceMatrix.Request;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using GoogleMapsApi.Entities.Common;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class NullableReferenceTypesCompatibilityTests
    {
        private JsonSerializerOptions _options;

        [SetUp]
        public void Setup()
        {
            // Use the exact same JSON configuration as production code
            _options = GoogleMapsApi.Engine.JsonSerializerConfiguration.CreateOptions();
        }

        #region Request Entity Null Safety Tests

        [Test]
        public void DirectionsRequest_NullableProperties_HandleCorrectly()
        {
            var request = new DirectionsRequest
            {
                Origin = "San Francisco, CA",
                Destination = "Los Angeles, CA", 
                ApiKey = "test_key"
            };

            // Test that optional nullable properties can be null without issues
            Assert.DoesNotThrow(() =>
            {
                var uri = request.GetUri();
                Assert.That(uri, Is.Not.Null);
                Assert.That(uri.ToString(), Contains.Substring("origin="));
                Assert.That(uri.ToString(), Contains.Substring("destination="));
            });

            // Test with waypoints (nullable property)
            request.Waypoints = null;
            Assert.DoesNotThrow(() => request.GetUri());

            // Test with empty waypoints
            request.Waypoints = new string[0];
            Assert.DoesNotThrow(() => request.GetUri());

            // Test with actual waypoints
            request.Waypoints = new string[] { "Fresno, CA" };
            Assert.DoesNotThrow(() => request.GetUri());
        }

        [Test]
        public void GeocodingRequest_NullableStringProperties_HandleCorrectly()
        {
            // Test with address only
            var request1 = new GeocodingRequest
            {
                Address = "1600 Amphitheatre Parkway, Mountain View, CA",
                ApiKey = "test_key"
            };
            
            Assert.DoesNotThrow(() =>
            {
                var uri = request1.GetUri();
                Assert.That(uri.ToString(), Contains.Substring("address="));
            });

            // Test with components only (address should be null/empty)
            var request2 = new GeocodingRequest
            {
                Components = new GeocodingComponents { Country = "US" },
                ApiKey = "test_key"
            };

            Assert.DoesNotThrow(() => request2.GetUri());
        }

        [Test]
        public void PlacesDetailsRequest_RequiredAndOptionalProperties_HandleCorrectly()
        {
            // Test with minimum required properties
            var request = new PlacesDetailsRequest
            {
                PlaceId = "ChIJN1t_tDeuEmsRUsoyG83frY4", // Google Sydney office
                ApiKey = "test_key"
            };

            Assert.DoesNotThrow(() =>
            {
                var uri = request.GetUri();
                Assert.That(uri.ToString(), Contains.Substring("placeid="));
            });

            // Test with optional nullable properties
            request.Language = null;
            request.SessionToken = null;

            Assert.DoesNotThrow(() => request.GetUri());

            // Test with populated optional properties
            request.Language = "en";
            request.SessionToken = "test_session_token";

            Assert.DoesNotThrow(() => request.GetUri());
        }

        [Test]
        public void DistanceMatrixRequest_NullableCollectionProperties_HandleCorrectly()
        {
            var origins = new[] { "San Francisco, CA" };
            var destinations = new[] { "Los Angeles, CA" };

            var request = new DistanceMatrixRequest
            {
                Origins = origins,
                Destinations = destinations,
                ApiKey = "test_key"
            };

            // Test with nullable avoid restrictions
            request.Avoid = null;
            Assert.DoesNotThrow(() => request.GetUri());

            // Test with populated avoid restrictions
            request.Avoid = DistanceMatrixRestrictions.tolls;
            Assert.DoesNotThrow(() => request.GetUri());
        }

        #endregion

        #region Response Entity Null Safety Tests

        [Test]
        public void DirectionsResponse_NullableCollections_DeserializeCorrectly()
        {
            // Test response with minimal data (many nullable collections)
            var minimalJson = """
            {
                "status": "OK",
                "routes": []
            }
            """;

            Assert.DoesNotThrow(() =>
            {
                var response = JsonSerializer.Deserialize<DirectionsResponse>(minimalJson, _options);
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Status, Is.EqualTo(DirectionsStatusCodes.OK));
                Assert.That(response.Routes, Is.Not.Null);
                Assert.That(response.Routes.Count(), Is.EqualTo(0));
            });

            // Test response with additional null properties
            var nullPropertiesJson = """
            {
                "status": "OK",
                "routes": [],
                "error_message": null
            }
            """;

            Assert.DoesNotThrow(() =>
            {
                var response = JsonSerializer.Deserialize<DirectionsResponse>(nullPropertiesJson, _options);
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Status, Is.EqualTo(DirectionsStatusCodes.OK));
            });
        }

        [Test]
        public void GeocodingResponse_NullableNestedObjects_DeserializeCorrectly()
        {
            // Test with null/missing address components
            var partialResultJson = """
            {
                "status": "OK",
                "results": [
                    {
                        "formatted_address": "Test Address",
                        "geometry": {
                            "location": {
                                "lat": 37.7749,
                                "lng": -122.4194
                            },
                            "location_type": "APPROXIMATE"
                        },
                        "place_id": "test_place_id",
                        "types": ["establishment"]
                    }
                ]
            }
            """;

            Assert.DoesNotThrow(() =>
            {
                var response = JsonSerializer.Deserialize<GeocodingResponse>(partialResultJson, _options);
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Results, Is.Not.Null);
                Assert.That(response.Results.Count(), Is.EqualTo(1));
                
                var result = response.Results.First();
                Assert.That(result.FormattedAddress, Is.EqualTo("Test Address"));
                Assert.That(result.Geometry, Is.Not.Null);
                Assert.That(result.PlaceId, Is.EqualTo("test_place_id"));
            });
        }

        [Test]
        public void PlacesDetailsResponse_OptionalProperties_HandleNulls()
        {
            // Test response with many optional null properties
            var minimalPlaceJson = """
            {
                "status": "OK",
                "result": {
                    "place_id": "test_place_id",
                    "name": "Test Place",
                    "formatted_address": "Test Address"
                }
            }
            """;

            Assert.DoesNotThrow(() =>
            {
                var response = JsonSerializer.Deserialize<PlacesDetailsResponse>(minimalPlaceJson, _options);
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Status, Is.EqualTo(GoogleMapsApi.Entities.PlacesDetails.Response.Status.OK));
                Assert.That(response.Result, Is.Not.Null);
                Assert.That(response.Result.PlaceId, Is.EqualTo("test_place_id"));
                Assert.That(response.Result.Name, Is.EqualTo("Test Place"));
                
                // These should be null/default and not cause issues
                Assert.DoesNotThrow(() =>
                {
                    var photos = response.Result.Photos;
                    var openingHours = response.Result.OpeningHours;
                    var geometry = response.Result.Geometry;
                });
            });
        }

        [Test]
        public void DistanceMatrixResponse_NullableElements_HandleCorrectly()
        {
            // Test with some null elements (common when origins/destinations are invalid)
            var partialResultsJson = """
            {
                "status": "OK",
                "rows": [
                    {
                        "elements": [
                            {
                                "status": "OK",
                                "distance": {
                                    "text": "100 km",
                                    "value": 100000
                                },
                                "duration": {
                                    "text": "1 hour",
                                    "value": 3600
                                }
                            },
                            {
                                "status": "NOT_FOUND"
                            }
                        ]
                    }
                ]
            }
            """;

            Assert.DoesNotThrow(() =>
            {
                var response = JsonSerializer.Deserialize<DistanceMatrixResponse>(partialResultsJson, _options);
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Rows, Is.Not.Null);
                Assert.That(response.Rows.Count(), Is.EqualTo(1));
                
                var row = response.Rows.First();
                Assert.That(row.Elements, Is.Not.Null);
                Assert.That(row.Elements.Count(), Is.EqualTo(2));

                var successElement = row.Elements.First();
                Assert.That(successElement.Status, Is.EqualTo(DistanceMatrixElementStatusCodes.OK));
                Assert.That(successElement.Distance, Is.Not.Null);
                Assert.That(successElement.Duration, Is.Not.Null);

                var failedElement = row.Elements.Skip(1).First();
                Assert.That(failedElement.Status, Is.EqualTo(DistanceMatrixElementStatusCodes.NOT_FOUND));
                // Distance and Duration should be null for failed elements
            });
        }

        #endregion

        #region Common Entity Null Safety Tests

        [Test]
        public void Location_NullSafetyInComparisons()
        {
            var location1 = new Location(37.7749, -122.4194);
            var location2 = new Location(37.7749, -122.4194);
            Location? nullLocation = null;

            // Test equality comparisons don't throw with nulls
            Assert.DoesNotThrow(() =>
            {
                var eq1 = location1.Equals(location2);
                var eq2 = location1.Equals(nullLocation);
                var eq3 = nullLocation?.Equals(location1);
            });

            // Test ToString doesn't throw
            Assert.DoesNotThrow(() =>
            {
                var str = location1.ToString();
            });

            // Test LocationString property
            Assert.DoesNotThrow(() =>
            {
                var locStr = location1.LocationString;
                Assert.That(location1.LocationString, Is.Not.Null);
                Assert.That(location1.LocationString, Is.Not.Empty);
            });
        }

        [Test]
        public void AddressLocation_NullableStringHandling()
        {
            // Test with null address
            var addressLocation1 = new AddressLocation(null!);
            Assert.DoesNotThrow(() =>
            {
                var locStr1 = addressLocation1.LocationString;
                var str1 = addressLocation1.ToString();
            });

            // Test with empty address
            var addressLocation2 = new AddressLocation("");
            Assert.DoesNotThrow(() =>
            {
                var locStr2 = addressLocation2.LocationString;
                var str2 = addressLocation2.ToString();
            });

            // Test with whitespace address
            var addressLocation3 = new AddressLocation("   ");
            Assert.DoesNotThrow(() =>
            {
                var locStr3 = addressLocation3.LocationString;
                var str3 = addressLocation3.ToString();
            });

            // Test with valid address
            var addressLocation4 = new AddressLocation("1600 Amphitheatre Parkway, Mountain View, CA");
            Assert.DoesNotThrow(() =>
            {
                var locStr4 = addressLocation4.LocationString;
                var str4 = addressLocation4.ToString();
                Assert.That(addressLocation4.LocationString, Is.Not.Null);
                Assert.That(addressLocation4.LocationString, Is.Not.Empty);
            });
        }

        [Test]
        public void Photo_NullableReferences_HandleCorrectly()
        {
            // Test Photo entity with null/missing properties
            var photoJson = """
            {
                "width": 100,
                "height": 100
            }
            """;

            Assert.DoesNotThrow(() =>
            {
                var photo = JsonSerializer.Deserialize<Photo>(photoJson, _options);
                Assert.That(photo, Is.Not.Null);
                Assert.That(photo.Width, Is.EqualTo(100));
                Assert.That(photo.Height, Is.EqualTo(100));
                
                // These properties might be null and should be handled safely
                Assert.DoesNotThrow(() =>
                {
                    var photoRef = photo.PhotoReference;
                    var htmlAttr = photo.HtmlAttributions;
                });
            });
        }

        #endregion

        #region Property Nullability Validation Tests

        [Test]
        public void AllResponseEntities_NullProperties_DoNotCauseExceptions()
        {
            var responseTypes = new[]
            {
                typeof(DirectionsResponse),
                typeof(GeocodingResponse),
                typeof(PlacesDetailsResponse),
                typeof(DistanceMatrixResponse)
            };

            foreach (var responseType in responseTypes)
            {
                // Get all public properties
                var properties = responseType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                
                foreach (var property in properties)
                {
                    Assert.DoesNotThrow(() =>
                    {
                        // Try to create instance and access property
                        var instance = Activator.CreateInstance(responseType);
                        if (instance != null && property.CanRead)
                        {
                            var value = property.GetValue(instance);
                        }
                    }, $"Property {property.Name} in {responseType.Name} caused exception when accessed with null instance");
                }
            }
        }

        [Test]
        public void AllRequestEntities_GetUri_WithNullProperties_DoNotCrash()
        {
            var requestTypes = new[]
            {
                typeof(DirectionsRequest),
                typeof(GeocodingRequest),
                typeof(PlacesDetailsRequest),
                typeof(DistanceMatrixRequest)
            };

            foreach (var requestType in requestTypes)
            {
                try
                {
                    var instance = Activator.CreateInstance(requestType) as MapsBaseRequest;
                    if (instance != null)
                    {
                        // Set minimum required properties to make GetUri() work
                        SetMinimumRequiredProperties(instance);
                        
                        Assert.DoesNotThrow(() =>
                        {
                            var uri = instance.GetUri();
                            Assert.That(uri, Is.Not.Null);
                        }, $"{requestType.Name}.GetUri() crashed with null optional properties");
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Failed to test {requestType.Name}: {ex.Message}");
                }
            }
        }

        private static void SetMinimumRequiredProperties(MapsBaseRequest request)
        {
            request.ApiKey = "test_key";
            
            switch (request)
            {
                case DirectionsRequest dir:
                    dir.Origin = "Test Origin";
                    dir.Destination = "Test Destination";
                    break;
                case GeocodingRequest geo:
                    geo.Address = "test_address";
                    break;
                case PlacesDetailsRequest places:
                    places.PlaceId = "test_place_id";
                    break;
                case DistanceMatrixRequest dist:
                    dist.Origins = new[] { "Test Origin" };
                    dist.Destinations = new[] { "Test Destination" };
                    break;
            }
        }

        #endregion

        #region Collection Null Safety Tests

        [Test]
        public void Collections_NullAndEmptyHandling_Consistent()
        {
            // Test that collections handle null and empty states consistently
            var directionsRequest = new DirectionsRequest
            {
                Origin = "Test Origin",
                Destination = "Test Destination",
                ApiKey = "test_key"
            };

            // Test null waypoints
            directionsRequest.Waypoints = null;
            Assert.DoesNotThrow(() => directionsRequest.GetUri());

            // Test empty waypoints array
            directionsRequest.Waypoints = new string[0];
            Assert.DoesNotThrow(() => directionsRequest.GetUri());
            Assert.DoesNotThrow(() => directionsRequest.GetUri());
        }

        #endregion
    }
}