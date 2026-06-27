using System;
using System.Text.Json;
using GoogleMapsApi.Engine.JsonConverters;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using GoogleMapsApi.Entities.Directions.Request;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class JsonConverterTests
    {
        private JsonSerializerOptions _options;

        [SetUp]
        public void Setup()
        {
            // Use centralized configuration for consistency with production code
            _options = JsonSerializerConfiguration.CreateOptions();
            
            // Add only the custom EnumMemberJsonConverter for testing
            _options.Converters.Add(new EnumMemberJsonConverter<TravelMode>());
        }

        #region DurationJsonConverter Tests

        [Test]
        public void DurationJsonConverter_Directions_DeserializesCorrectly()
        {
            var json = """{"value": 3600, "text": "1 hour"}""";
            
            var duration = JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json, _options);
            
            Assert.That(duration, Is.Not.Null);
            Assert.That(duration.Value, Is.EqualTo(TimeSpan.FromHours(1)));
            Assert.That(duration.Text, Is.EqualTo("1 hour"));
        }

        [Test]
        public void DurationJsonConverter_DistanceMatrix_DeserializesCorrectly()
        {
            var json = """{"value": 1800, "text": "30 mins"}""";
            
            var duration = JsonSerializer.Deserialize<GoogleMapsApi.Entities.DistanceMatrix.Response.Duration>(json, _options);
            
            Assert.That(duration, Is.Not.Null);
            Assert.That(duration.Value, Is.EqualTo(TimeSpan.FromMinutes(30)));
            Assert.That(duration.Text, Is.EqualTo("30 mins"));
        }

        [Test]
        public void DurationJsonConverter_SerializesCorrectly()
        {
            var duration = new GoogleMapsApi.Entities.Directions.Response.Duration
            {
                Value = TimeSpan.FromMinutes(45),
                Text = "45 mins"
            };

            var json = JsonSerializer.Serialize(duration, _options);
            
            Assert.That(json, Does.Contain("\"value\":2700"));
            Assert.That(json, Does.Contain("\"text\":\"45 mins\""));
        }

        [Test]
        public void DurationJsonConverter_HandlesNullCorrectly()
        {
            var json = "null";
            
            var duration = JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json, _options);
            
            Assert.That(duration, Is.Null);
        }

        [Test]
        public void DurationJsonConverter_ThrowsOnInvalidJson()
        {
            var json = "\"invalid_format\"";

            Assert.Throws<JsonException>(() =>
                JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json, _options));
        }

        [Test]
        public void DurationJsonConverter_Directions_OnlyValue_MapsValueAndLeavesTextNull()
        {
            var json = """{"value": 3600}""";

            var duration = JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json, _options);

            Assert.That(duration, Is.Not.Null);
            Assert.That(duration!.Value, Is.EqualTo(TimeSpan.FromHours(1)));
            Assert.That(duration.Text, Is.Null);
        }

        [Test]
        public void DurationJsonConverter_Directions_OnlyText_MapsTextAndLeavesValueZero()
        {
            var json = """{"text": "1 hour"}""";

            var duration = JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json, _options);

            Assert.That(duration, Is.Not.Null);
            Assert.That(duration!.Value, Is.EqualTo(TimeSpan.Zero));
            Assert.That(duration.Text, Is.EqualTo("1 hour"));
        }

        [Test]
        public void DurationJsonConverter_DistanceMatrix_OnlyValue_MapsValue()
        {
            var json = """{"value": 1800}""";

            var duration = JsonSerializer.Deserialize<GoogleMapsApi.Entities.DistanceMatrix.Response.Duration>(json, _options);

            Assert.That(duration, Is.Not.Null);
            Assert.That(duration!.Value, Is.EqualTo(TimeSpan.FromMinutes(30)));
        }

        [Test]
        public void DurationJsonConverter_EmptyObject_ReturnsDefaults()
        {
            var json = """{}""";

            var duration = JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json, _options);

            Assert.That(duration, Is.Not.Null);
            Assert.That(duration!.Value, Is.EqualTo(TimeSpan.Zero));
            Assert.That(duration.Text, Is.Null);
        }

        [Test]
        public void DurationJsonConverter_SkipsUnknownNestedProperty()
        {
            // A nested container on an unknown property must be skipped without desyncing the reader.
            var json = """{"value": 60, "foo": {"a": 1, "b": [2, 3]}, "text": "1 min"}""";

            var duration = JsonSerializer.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json, _options);

            Assert.That(duration, Is.Not.Null);
            Assert.That(duration!.Value, Is.EqualTo(TimeSpan.FromMinutes(1)));
            Assert.That(duration.Text, Is.EqualTo("1 min"));
        }

        [Test]
        public void DurationJsonConverter_NullText_OmitsTextFromJson()
        {
            var duration = new GoogleMapsApi.Entities.Directions.Response.Duration
            {
                Value = TimeSpan.FromMinutes(45),
                Text = null
            };

            var json = JsonSerializer.Serialize(duration, _options);

            Assert.That(json, Does.Contain("\"value\":2700"));
            Assert.That(json, Does.Not.Contain("text"));
        }

        #endregion

        #region EnumMemberJsonConverter Tests

        [Test]
        public void EnumMemberJsonConverter_DeserializesDrivingFromString()
        {
            var json = "\"DRIVING\"";
            
            var travelMode = JsonSerializer.Deserialize<TravelMode>(json, _options);
            
            Assert.That(travelMode, Is.EqualTo(TravelMode.Driving));
        }

        [Test]
        public void EnumMemberJsonConverter_DeserializesWalkingFromString()
        {
            var json = "\"WALKING\"";
            
            var travelMode = JsonSerializer.Deserialize<TravelMode>(json, _options);
            
            Assert.That(travelMode, Is.EqualTo(TravelMode.Walking));
        }

        [Test]
        public void EnumMemberJsonConverter_DeserializesBicyclingFromString()
        {
            var json = "\"BICYCLING\"";
            
            var travelMode = JsonSerializer.Deserialize<TravelMode>(json, _options);
            
            Assert.That(travelMode, Is.EqualTo(TravelMode.Bicycling));
        }

        [Test]
        public void EnumMemberJsonConverter_DeserializesTransitFromString()
        {
            var json = "\"TRANSIT\"";
            
            var travelMode = JsonSerializer.Deserialize<TravelMode>(json, _options);
            
            Assert.That(travelMode, Is.EqualTo(TravelMode.Transit));
        }

        [Test]
        public void EnumMemberJsonConverter_DeserializesFromNumeric()
        {
            var json = "0";
            
            var travelMode = JsonSerializer.Deserialize<TravelMode>(json, _options);
            
            Assert.That(travelMode, Is.EqualTo(TravelMode.Driving));
        }

        [Test]
        public void EnumMemberJsonConverter_SerializesToEnumMemberValue()
        {
            var travelMode = TravelMode.Driving;
            
            var json = JsonSerializer.Serialize(travelMode, _options);
            
            Assert.That(json, Is.EqualTo("\"DRIVING\""));
        }

        [Test]
        public void EnumMemberJsonConverter_ThrowsOnInvalidString()
        {
            var json = "\"INVALID_MODE\"";
            
            Assert.Throws<JsonException>(() => 
                JsonSerializer.Deserialize<TravelMode>(json, _options));
        }

        [Test]
        public void EnumMemberJsonConverter_ThrowsOnInvalidNumeric()
        {
            var json = "999";
            
            Assert.Throws<JsonException>(() => 
                JsonSerializer.Deserialize<TravelMode>(json, _options));
        }

        #endregion

        #region OverviewPolylineJsonConverter Tests

        [Test]
        public void OverviewPolylineJsonConverter_DeserializesCorrectly()
        {
            var json = """{"points": "_p~iF~ps|U_ulLnnqC_mqNvxq`@"}""";
            
            var polyline = JsonSerializer.Deserialize<OverviewPolyline>(json, _options);
            
            Assert.That(polyline, Is.Not.Null);
            Assert.That(polyline.Points, Is.Not.Empty);
        }

        [Test]
        public void OverviewPolylineJsonConverter_SerializesCorrectly()
        {
            // Create polyline with reflection since EncodedPoints is internal
            var polyline = new OverviewPolyline();
            var encodedPointsProperty = typeof(OverviewPolyline).GetProperty("EncodedPoints", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var testPoints = "simple_encoded_points";
            encodedPointsProperty?.SetValue(polyline, testPoints);
            
            // Call internal OnDeserialized method via reflection
            var onDeserializedMethod = typeof(OverviewPolyline).GetMethod("OnDeserialized",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            onDeserializedMethod?.Invoke(polyline, null);

            var json = JsonSerializer.Serialize(polyline, _options);
            
            Assert.That(json, Does.Contain("simple_encoded_points"));
            Assert.That(json, Does.Contain("\"points\""));
        }

        [Test]
        public void OverviewPolylineJsonConverter_HandlesNullCorrectly()
        {
            var json = "null";
            
            var polyline = JsonSerializer.Deserialize<OverviewPolyline>(json, _options);
            
            Assert.That(polyline, Is.Null);
        }

        [Test]
        public void OverviewPolylineJsonConverter_HandlesEmptyPoints()
        {
            var json = """{"points": ""}""";

            var polyline = JsonSerializer.Deserialize<OverviewPolyline>(json, _options);

            Assert.That(polyline, Is.Not.Null);
            Assert.That(polyline.Points, Is.Empty);
        }

        [Test]
        public void OverviewPolylineJsonConverter_MissingPoints_ReturnsEmpty()
        {
            var json = """{}""";

            var polyline = JsonSerializer.Deserialize<OverviewPolyline>(json, _options);

            Assert.That(polyline, Is.Not.Null);
            Assert.That(polyline!.GetRawPointsData(), Is.Null);
            Assert.That(polyline.Points, Is.Empty);
        }

        [Test]
        public void OverviewPolylineJsonConverter_SkipsUnknownProperty_StillDecodesPoints()
        {
            // An unknown property (here a nested object) must be skipped without losing "points".
            var json = """{"levels": {"a": 1}, "points": "_p~iF~ps|U_ulLnnqC_mqNvxq`@"}""";

            var polyline = JsonSerializer.Deserialize<OverviewPolyline>(json, _options);

            Assert.That(polyline, Is.Not.Null);
            Assert.That(polyline!.Points, Is.Not.Empty);
        }

        #endregion

        #region Integration Tests for Combined Scenarios

        [Test]
        public void AllConverters_WorkTogetherInComplexObject()
        {
            var json = """
            {
                "status": "OK",
                "routes": [{
                    "overview_polyline": {
                        "points": "_p~iF~ps|U_ulLnnqC_mqNvxq`@"
                    },
                    "legs": [{
                        "duration": {
                            "value": 3600,
                            "text": "1 hour"
                        }
                    }]
                }]
            }
            """;

            // This tests that the converters work together in a realistic scenario
            Assert.DoesNotThrow(() => JsonSerializer.Deserialize<object>(json, _options));
        }

        #endregion
    }
}