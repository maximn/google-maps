using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Engine.JsonConverters;
using GoogleMapsApi.Entities.AddressValidation.Request;
using GoogleMapsApi.Entities.AerialView.Request;
using GoogleMapsApi.Entities.AirQuality.Request;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNew.Common;
using GoogleMapsApi.Entities.PlacesNew.Request;
using GoogleMapsApi.Entities.Routes.Request;
using NUnit.Framework;
using PlacesLatLng = GoogleMapsApi.Entities.PlacesNew.Common.LatLng;

namespace GoogleMapsApi.Test
{
    /// <summary>
    /// Guards the consolidation of POST request-body serialization onto the single
    /// <see cref="JsonSerializerConfiguration.CreateRequestBodyOptions"/> factory: every POST endpoint must
    /// omit unset fields and emit enums by their wire value, so the per-request options cannot drift apart.
    /// </summary>
    [TestFixture]
    public class RequestBodySerializationTests
    {
        [Test]
        public void CreateRequestBodyOptions_OmitsNulls()
        {
            var options = JsonSerializerConfiguration.CreateRequestBodyOptions();
            Assert.That(options.DefaultIgnoreCondition, Is.EqualTo(JsonIgnoreCondition.WhenWritingNull));
        }

        [Test]
        public void CreateRequestBodyOptions_RegistersEnumMemberConverter()
        {
            var options = JsonSerializerConfiguration.CreateRequestBodyOptions();
            Assert.That(options.Converters.OfType<EnumMemberJsonConverterFactory>().Any(), Is.True);
        }

        [TestCaseSource(nameof(AllPostRequests))]
        public void PostBody_OmitsUnsetOptionalFields(MapsBaseRequest request)
        {
            using var doc = JsonDocument.Parse(SerializeBody(request));
            AssertNoNullTokens(doc.RootElement, "$");
        }

        [Test]
        public void Routes_TravelMode_SerializesEnumMemberWireValue()
        {
            var request = ValidRoutes();
            request.TravelMode = RoutesTravelMode.Drive;
            using var doc = JsonDocument.Parse(SerializeBody(request));
            Assert.That(doc.RootElement.GetProperty("travelMode").GetString(), Is.EqualTo("DRIVE"));
        }

        [Test]
        public void SearchText_RankPreference_SerializesEnumMemberWireValue()
        {
            var request = ValidSearchText();
            request.RankPreference = RankPreference.Distance;
            using var doc = JsonDocument.Parse(SerializeBody(request));
            Assert.That(doc.RootElement.GetProperty("rankPreference").GetString(), Is.EqualTo("DISTANCE"));
        }

        private static IEnumerable<TestCaseData> AllPostRequests()
        {
            yield return new TestCaseData(ValidRoutes()).SetName("Routes");
            yield return new TestCaseData(ValidAddressValidation()).SetName("AddressValidation");
            yield return new TestCaseData(ValidSearchText()).SetName("SearchText");
            yield return new TestCaseData(ValidSearchNearby()).SetName("SearchNearby");
            yield return new TestCaseData(ValidAutocomplete()).SetName("Autocomplete");
            yield return new TestCaseData(ValidRenderVideo()).SetName("AerialViewRenderVideo");
            yield return new TestCaseData(ValidCurrentConditions()).SetName("AirQualityCurrentConditions");
            yield return new TestCaseData(ValidHistory()).SetName("AirQualityHistory");
            yield return new TestCaseData(ValidForecast()).SetName("AirQualityForecast");
        }

        private static void AssertNoNullTokens(JsonElement element, string path)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Null:
                    Assert.Fail($"Request body leaked a null token at {path}; WhenWritingNull should have omitted it.");
                    break;
                case JsonValueKind.Object:
                    foreach (var property in element.EnumerateObject())
                        AssertNoNullTokens(property.Value, $"{path}.{property.Name}");
                    break;
                case JsonValueKind.Array:
                    var index = 0;
                    foreach (var item in element.EnumerateArray())
                        AssertNoNullTokens(item, $"{path}[{index++}]");
                    break;
            }
        }

        private static string SerializeBody(MapsBaseRequest request)
        {
            // GetRequestBody is protected internal on the base type, so reach it via reflection.
            var method = typeof(MapsBaseRequest).GetMethod(
                "GetRequestBody", BindingFlags.Instance | BindingFlags.NonPublic);
            var content = (HttpContent?)method!.Invoke(request, null);
            Assert.That(content, Is.Not.Null, "Expected a POST body but GetRequestBody returned null.");
            return content!.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        private static RoutesRequest ValidRoutes() => new RoutesRequest
        {
            ApiKey = "k",
            Origin = Waypoint.FromAddress("San Francisco, CA"),
            Destination = Waypoint.FromAddress("Mountain View, CA"),
        };

        private static AddressValidationRequest ValidAddressValidation() => new AddressValidationRequest
        {
            ApiKey = "k",
            Address = new PostalAddress
            {
                RegionCode = "US",
                AddressLines = new List<string> { "1600 Amphitheatre Pkwy" },
            },
        };

        private static SearchTextRequest ValidSearchText() => new SearchTextRequest
        {
            ApiKey = "k",
            TextQuery = "pizza in NY",
        };

        private static SearchNearbyRequest ValidSearchNearby() => new SearchNearbyRequest
        {
            ApiKey = "k",
            LocationRestriction = new LocationRestriction
            {
                Circle = new Circle { Center = new PlacesLatLng { Latitude = 37.0, Longitude = -122.0 }, Radius = 500 },
            },
        };

        private static AutocompleteRequest ValidAutocomplete() => new AutocompleteRequest
        {
            ApiKey = "k",
            Input = "Eiffel",
        };

        private static RenderVideoRequest ValidRenderVideo() => new RenderVideoRequest
        {
            ApiKey = "k",
            Address = "1600 Amphitheatre Pkwy, Mountain View, CA",
        };

        private static CurrentConditionsRequest ValidCurrentConditions() => new CurrentConditionsRequest
        {
            ApiKey = "k",
            Latitude = 37.0,
            Longitude = -122.0,
        };

        private static HistoryRequest ValidHistory() => new HistoryRequest
        {
            ApiKey = "k",
            Latitude = 37.0,
            Longitude = -122.0,
            Hours = 1,
        };

        private static ForecastRequest ValidForecast() => new ForecastRequest
        {
            ApiKey = "k",
            Latitude = 37.0,
            Longitude = -122.0,
        };
    }
}
