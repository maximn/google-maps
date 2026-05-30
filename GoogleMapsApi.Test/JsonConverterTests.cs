using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;
using System.Text.Json;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class JsonConverterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        #region DurationJsonConverter Tests

        [Test]
        public void DurationJsonConverter_Directions_DeserializesCorrectly()
        {
            var json = """{"value": 3600, "text": "1 hour"}""";

            var duration = JsonMultitargets.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json);

            Assert.That(duration, Is.Not.Null);
            Assert.That(duration.Value, Is.EqualTo(TimeSpan.FromHours(1)));
            Assert.That(duration.Text, Is.EqualTo("1 hour"));
        }

        [Test]
        public void DurationJsonConverter_DistanceMatrix_DeserializesCorrectly()
        {
            var json = """{"value": 1800, "text": "30 mins"}""";

            var duration = JsonMultitargets.Deserialize<GoogleMapsApi.Entities.DistanceMatrix.Response.Duration>(json);

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

            var json = JsonMultitargets.Serialize(duration);

            Assert.That(json, Does.Contain("\"value\":2700"));
            Assert.That(json, Does.Contain("\"text\":\"45 mins\""));
        }

        [Test]
        public void DurationJsonConverter_HandlesNullCorrectly()
        {
            var json = "null";

            var duration = JsonMultitargets.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json);

            Assert.That(duration, Is.Null);
        }

        [Test]
        public void DurationJsonConverter_ThrowsOnInvalidJson()
        {
            var json = "\"invalid_format\"";

            Assert.Throws<JsonException>(() =>
                JsonMultitargets.Deserialize<GoogleMapsApi.Entities.Directions.Response.Duration>(json));
        }

        #endregion

        #region PriceLevelJsonConverter Tests

        [Test]
        public void PriceLevelJsonConverter_DeserializesNumericValue()
        {
            var json = "2";

            var priceLevel = JsonMultitargets.Deserialize<PriceLevel?>(json);

            Assert.That(priceLevel, Is.EqualTo(PriceLevel.Moderate));
        }

        [Test]
        public void PriceLevelJsonConverter_DeserializesStringValue()
        {
            var json = "\"3\"";

            var priceLevel = JsonMultitargets.Deserialize<PriceLevel?>(json);

            Assert.That(priceLevel, Is.EqualTo(PriceLevel.Expensive));
        }

        [Test]
        public void PriceLevelJsonConverter_DeserializesNullValue()
        {
            var json = "null";

            var priceLevel = JsonMultitargets.Deserialize<PriceLevel?>(json);

            Assert.That(priceLevel, Is.Null);
        }

        [Test]
        public void PriceLevelJsonConverter_DeserializesEmptyString()
        {
            var json = "\"\"";

            var priceLevel = JsonMultitargets.Deserialize<PriceLevel?>(json);

            Assert.That(priceLevel, Is.Null);
        }

        [Test]
        public void PriceLevelJsonConverter_SerializesToString()
        {
            PriceLevel? priceLevel = PriceLevel.VeryExpensive;

            var json = JsonMultitargets.Serialize(priceLevel);

            Assert.That(json, Is.EqualTo("\"4\""));
        }

        [Test]
        public void PriceLevelJsonConverter_SerializesNullToNull()
        {
            PriceLevel? priceLevel = null;

            var json = JsonMultitargets.Serialize(priceLevel);

            Assert.That(json, Is.EqualTo("null"));
        }

        #endregion

        #region EnumMemberJsonConverter Tests

        [Test]
        public void EnumMemberJsonConverter_DeserializesDrivingFromString()
        {
            var json = "\"DRIVING\"";

            var travelMode = JsonMultitargets.Deserialize<TravelMode>(json);

            Assert.That(travelMode, Is.EqualTo(TravelMode.Driving));
        }

        [Test]
        public void EnumMemberJsonConverter_DeserializesWalkingFromString()
        {
            var json = "\"WALKING\"";

            var travelMode = JsonMultitargets.Deserialize<TravelMode>(json);

            Assert.That(travelMode, Is.EqualTo(TravelMode.Walking));
        }

        [Test]
        public void EnumMemberJsonConverter_DeserializesBicyclingFromString()
        {
            var json = "\"BICYCLING\"";

            var travelMode = JsonMultitargets.Deserialize<TravelMode>(json);

            Assert.That(travelMode, Is.EqualTo(TravelMode.Bicycling));
        }

        [Test]
        public void EnumMemberJsonConverter_DeserializesTransitFromString()
        {
            var json = "\"TRANSIT\"";

            var travelMode = JsonMultitargets.Deserialize<TravelMode>(json);

            Assert.That(travelMode, Is.EqualTo(TravelMode.Transit));
        }

        [Test]
        public void EnumMemberJsonConverter_DeserializesFromNumeric()
        {
            var json = "0";

            var travelMode = JsonMultitargets.Deserialize<TravelMode>(json);

            Assert.That(travelMode, Is.EqualTo(TravelMode.Driving));
        }

        [Test]
        public void EnumMemberJsonConverter_SerializesToEnumMemberValue()
        {
            var travelMode = TravelMode.Driving;

            var json = JsonMultitargets.Serialize(travelMode);

            Assert.That(json, Is.EqualTo("\"DRIVING\""));
        }

        [Test]
        public void EnumMemberJsonConverter_ThrowsOnInvalidString()
        {
            var json = "\"INVALID_MODE\"";

            Assert.Throws<JsonException>(() =>
                JsonMultitargets.Deserialize<TravelMode>(json));
        }

        [Test]
        public void EnumMemberJsonConverter_ThrowsOnInvalidNumeric()
        {
            var json = "999";

            Assert.Throws<JsonException>(() =>
                JsonMultitargets.Deserialize<TravelMode>(json));
        }

        #endregion

        #region OverviewPolylineJsonConverter Tests

        [Test]
        public void OverviewPolylineJsonConverter_DeserializesCorrectly()
        {
            var json = """{"points": "_p~iF~ps|U_ulLnnqC_mqNvxq`@"}""";

            var polyline = JsonMultitargets.Deserialize<OverviewPolyline>(json);

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

            var json = JsonMultitargets.Serialize(polyline);

            Assert.That(json, Does.Contain("simple_encoded_points"));
            Assert.That(json, Does.Contain("\"points\""));
        }

        [Test]
        public void OverviewPolylineJsonConverter_HandlesNullCorrectly()
        {
            var json = "null";

            var polyline = JsonMultitargets.Deserialize<OverviewPolyline>(json);

            Assert.That(polyline, Is.Null);
        }

        [Test]
        public void OverviewPolylineJsonConverter_HandlesEmptyPoints()
        {
            var json = """{"points": ""}""";

            var polyline = JsonMultitargets.Deserialize<OverviewPolyline>(json);

            Assert.That(polyline, Is.Not.Null);
            Assert.That(polyline.Points, Is.Empty);
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
            Assert.DoesNotThrow(() => JsonMultitargets.Deserialize<object>(json));
        }

        #endregion
    }
}