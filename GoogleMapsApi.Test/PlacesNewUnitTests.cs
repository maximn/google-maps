using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.PlacesNew.Common;
using GoogleMapsApi.Entities.PlacesNew.Request;
using GoogleMapsApi.Entities.PlacesNew.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class PlacesNewUnitTests
    {
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
                Circle = new Circle { Center = new LatLng { Latitude = 37.0, Longitude = -122.0 }, Radius = 500 },
            },
        };

        private static PlaceDetailsRequest ValidDetails() => new PlaceDetailsRequest
        {
            ApiKey = "k",
            PlaceId = "ChIJ123",
        };

        private static AutocompleteRequest ValidAutocomplete() => new AutocompleteRequest
        {
            ApiKey = "k",
            Input = "Eiffel",
        };

        private static PlacePhotoRequest ValidPhoto() => new PlacePhotoRequest
        {
            ApiKey = "k",
            PhotoName = "places/ABC/photos/XYZ",
            MaxWidthPx = 400,
        };

        // --- SearchText ---

        [Test]
        public void SearchText_GetUri_TargetsHostAndPath_WithKeyAndFields()
        {
            var uri = ValidSearchText().GetUri();

            Assert.That(uri.Host, Is.EqualTo("places.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/places:searchText"));
            Assert.That(uri.Query, Does.Contain("key=k"));
            Assert.That(uri.Query, Does.Contain("$fields="));
        }

        [Test]
        public void SearchText_MissingTextQuery_Throws()
        {
            var request = new SearchTextRequest { ApiKey = "k", TextQuery = "" };
            Assert.Throws<ArgumentException>(() => request.GetUri());
        }

        [Test]
        public void SearchText_MissingApiKey_Throws()
        {
            var request = new SearchTextRequest { TextQuery = "pizza" };
            Assert.Throws<ArgumentException>(() => request.GetUri());
        }

        [Test]
        public async Task SearchText_IssuesPost_WithJsonBody_AndCanonicalFields()
        {
            var handler = new RecordingHandler("{\"places\":[]}");
            using var http = new HttpClient(handler);

            var request = ValidSearchText();
            request.RankPreference = RankPreference.Distance;
            request.OpenNow = true;

            await MapsAPIGenericEngine<SearchTextRequest, SearchTextResponse>
                .QueryGoogleAPIAsync(http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastContentType, Is.EqualTo("application/json"));
            Assert.That(handler.LastUri!.Host, Is.EqualTo("places.googleapis.com"));
            Assert.That(handler.LastUri.AbsolutePath, Is.EqualTo("/v1/places:searchText"));

            using var body = JsonDocument.Parse(handler.LastRequestBody!);
            var root = body.RootElement;
            Assert.That(root.GetProperty("textQuery").GetString(), Is.EqualTo("pizza in NY"));
            Assert.That(root.GetProperty("rankPreference").GetString(), Is.EqualTo("DISTANCE"));
            Assert.That(root.GetProperty("openNow").GetBoolean(), Is.True);
        }

        [Test]
        public async Task SearchText_OmitsUnsetOptionalFields()
        {
            var handler = new RecordingHandler("{\"places\":[]}");
            using var http = new HttpClient(handler);

            await MapsAPIGenericEngine<SearchTextRequest, SearchTextResponse>
                .QueryGoogleAPIAsync(http, ValidSearchText(), TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            using var body = JsonDocument.Parse(handler.LastRequestBody!);
            var root = body.RootElement;
            Assert.That(root.TryGetProperty("openNow", out _), Is.False);
            Assert.That(root.TryGetProperty("rankPreference", out _), Is.False);
            Assert.That(root.TryGetProperty("locationBias", out _), Is.False);
            Assert.That(root.TryGetProperty("minRating", out _), Is.False);
            Assert.That(root.TryGetProperty("priceLevels", out _), Is.False);
        }

        // --- SearchNearby ---

        [Test]
        public void SearchNearby_GetUri_TargetsHostAndPath_WithKeyAndFields()
        {
            var uri = ValidSearchNearby().GetUri();

            Assert.That(uri.Host, Is.EqualTo("places.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/places:searchNearby"));
            Assert.That(uri.Query, Does.Contain("key=k"));
            Assert.That(uri.Query, Does.Contain("$fields="));
        }

        [Test]
        public void SearchNearby_MissingLocationRestriction_Throws()
        {
            var request = new SearchNearbyRequest { ApiKey = "k", LocationRestriction = new LocationRestriction() };
            Assert.Throws<ArgumentException>(() => request.GetUri());
        }

        [Test]
        public async Task SearchNearby_IssuesPost_WithCircleAndRankPreference()
        {
            var handler = new RecordingHandler("{\"places\":[]}");
            using var http = new HttpClient(handler);

            var request = ValidSearchNearby();
            request.RankPreference = RankPreference.Distance;
            request.IncludedTypes = new List<string> { "restaurant" };

            await MapsAPIGenericEngine<SearchNearbyRequest, SearchNearbyResponse>
                .QueryGoogleAPIAsync(http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));

            using var body = JsonDocument.Parse(handler.LastRequestBody!);
            var root = body.RootElement;
            Assert.That(root.GetProperty("rankPreference").GetString(), Is.EqualTo("DISTANCE"));
            Assert.That(root.GetProperty("includedTypes")[0].GetString(), Is.EqualTo("restaurant"));
            var circle = root.GetProperty("locationRestriction").GetProperty("circle");
            Assert.That(circle.GetProperty("radius").GetDouble(), Is.EqualTo(500));
            Assert.That(circle.GetProperty("center").GetProperty("latitude").GetDouble(), Is.EqualTo(37.0));
        }

        // --- Place Details ---

        [Test]
        public void Details_GetUri_TargetsPlacePath_WithTopLevelFieldsAndKey()
        {
            var request = ValidDetails();
            request.LanguageCode = "en";
            request.SessionToken = "sess";
            var uri = request.GetUri();

            Assert.That(uri.Host, Is.EqualTo("places.googleapis.com"));
            Assert.That(uri.AbsolutePath, Does.StartWith("/v1/places/"));
            Assert.That(uri.AbsolutePath, Does.Contain("ChIJ123"));
            Assert.That(uri.Query, Does.Contain("key=k"));
            Assert.That(uri.Query, Does.Contain("$fields="));
            Assert.That(uri.Query, Does.Contain("languageCode=en"));
            Assert.That(uri.Query, Does.Contain("sessionToken=sess"));
            // Top-level paths in the default mask (no "places." prefix).
            Assert.That(PlaceDetailsRequest.DefaultFieldMask, Does.Not.Contain("places."));
        }

        [Test]
        public void Details_MissingPlaceId_Throws()
        {
            var request = new PlaceDetailsRequest { ApiKey = "k", PlaceId = "" };
            Assert.Throws<ArgumentException>(() => request.GetUri());
        }

        [Test]
        public async Task Details_IssuesGet_NoBody()
        {
            var handler = new RecordingHandler("{\"id\":\"ChIJ123\"}");
            using var http = new HttpClient(handler);

            var response = await MapsAPIGenericEngine<PlaceDetailsRequest, Place>
                .QueryGoogleAPIAsync(http, ValidDetails(), TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastRequestBody, Is.Null);
            Assert.That(response.Id, Is.EqualTo("ChIJ123"));
        }

        // --- Autocomplete ---

        [Test]
        public void Autocomplete_GetUri_HasKey_ButNoFieldMask()
        {
            var uri = ValidAutocomplete().GetUri();

            Assert.That(uri.Host, Is.EqualTo("places.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/places:autocomplete"));
            Assert.That(uri.Query, Does.Contain("key=k"));
            Assert.That(uri.Query, Does.Not.Contain("$fields"));
        }

        [Test]
        public void Autocomplete_MissingInput_Throws()
        {
            var request = new AutocompleteRequest { ApiKey = "k", Input = "" };
            Assert.Throws<ArgumentException>(() => request.GetUri());
        }

        [Test]
        public async Task Autocomplete_IssuesPost_WithInput()
        {
            var handler = new RecordingHandler("{\"suggestions\":[]}");
            using var http = new HttpClient(handler);

            await MapsAPIGenericEngine<AutocompleteRequest, AutocompleteResponse>
                .QueryGoogleAPIAsync(http, ValidAutocomplete(), TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            using var body = JsonDocument.Parse(handler.LastRequestBody!);
            Assert.That(body.RootElement.GetProperty("input").GetString(), Is.EqualTo("Eiffel"));
            Assert.That(handler.LastUri!.Query, Does.Not.Contain("$fields"));
        }

        // --- Place Photo ---

        [Test]
        public void Photo_GetUri_TargetsMediaPath_WithSkipRedirect_NoFieldMask()
        {
            var request = ValidPhoto();
            request.MaxHeightPx = 300;
            var uri = request.GetUri();

            Assert.That(uri.Host, Is.EqualTo("places.googleapis.com"));
            Assert.That(uri.AbsolutePath, Does.EndWith("/media"));
            Assert.That(uri.AbsolutePath, Does.Contain("places/ABC/photos/XYZ"));
            Assert.That(uri.Query, Does.Contain("skipHttpRedirect=true"));
            Assert.That(uri.Query, Does.Contain("key=k"));
            Assert.That(uri.Query, Does.Contain("maxWidthPx=400"));
            Assert.That(uri.Query, Does.Contain("maxHeightPx=300"));
            Assert.That(uri.Query, Does.Not.Contain("$fields"));
        }

        [Test]
        public void Photo_NoDimensions_Throws()
        {
            var request = new PlacePhotoRequest { ApiKey = "k", PhotoName = "places/A/photos/B" };
            Assert.Throws<ArgumentException>(() => request.GetUri());
        }

        [Test]
        public async Task Photo_IssuesGet_AndDeserializesUri()
        {
            var handler = new RecordingHandler("{\"name\":\"places/A/photos/B/media\",\"photoUri\":\"https://example.com/img.jpg\"}");
            using var http = new HttpClient(handler);

            var response = await MapsAPIGenericEngine<PlacePhotoRequest, PlacePhotoResponse>
                .QueryGoogleAPIAsync(http, ValidPhoto(), TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(response.PhotoUri, Is.EqualTo("https://example.com/img.jpg"));
        }

        // --- Deserialization / enums ---

        [Test]
        public void Place_DeserializesRepresentativeJson()
        {
            const string json = "{" +
                "\"id\":\"ChIJ123\"," +
                "\"displayName\":{\"text\":\"Joe's Pizza\",\"languageCode\":\"en\"}," +
                "\"formattedAddress\":\"7 Carmine St, New York\"," +
                "\"location\":{\"latitude\":40.73,\"longitude\":-74.0}," +
                "\"rating\":4.5," +
                "\"types\":[\"restaurant\",\"food\"]," +
                "\"businessStatus\":\"OPERATIONAL\"," +
                "\"priceLevel\":\"PRICE_LEVEL_MODERATE\"," +
                "\"photos\":[{\"name\":\"places/ChIJ123/photos/abc\",\"widthPx\":4000,\"heightPx\":3000}]," +
                "\"regularOpeningHours\":{\"openNow\":true,\"weekdayDescriptions\":[\"Monday: 9 AM - 10 PM\"]}" +
                "}";

            var options = JsonSerializerConfiguration.CreateOptions();
            var place = JsonSerializer.Deserialize<Place>(json, options);

            Assert.That(place, Is.Not.Null);
            Assert.That(place!.Id, Is.EqualTo("ChIJ123"));
            Assert.That(place.DisplayName!.Text, Is.EqualTo("Joe's Pizza"));
            Assert.That(place.Location!.Latitude, Is.EqualTo(40.73));
            Assert.That(place.Location.Longitude, Is.EqualTo(-74.0));
            Assert.That(place.Rating, Is.EqualTo(4.5));
            Assert.That(place.Types, Is.EquivalentTo(new[] { "restaurant", "food" }));
            Assert.That(place.BusinessStatus, Is.EqualTo(BusinessStatus.Operational));
            Assert.That(place.PriceLevel, Is.EqualTo(PriceLevel.Moderate));
            Assert.That(place.Photos, Has.Count.EqualTo(1));
            Assert.That(place.Photos![0].Name, Is.EqualTo("places/ChIJ123/photos/abc"));
            Assert.That(place.RegularOpeningHours!.OpenNow, Is.True);
            Assert.That(place.RegularOpeningHours.WeekdayDescriptions, Has.Count.EqualTo(1));
        }

        [Test]
        public void BusinessStatus_RoundTrips_FromEnumMemberValue()
        {
            var options = JsonSerializerConfiguration.CreateOptions();
            var place = JsonSerializer.Deserialize<Place>("{\"businessStatus\":\"OPERATIONAL\"}", options);
            Assert.That(place!.BusinessStatus, Is.EqualTo(BusinessStatus.Operational));
        }

        [Test]
        public void SearchText_RankPreference_SerializesEnumMemberValue()
        {
            var request = ValidSearchText();
            request.RankPreference = RankPreference.Distance;
            var body = request.GetRequestBodyForTest();
            Assert.That(body, Does.Contain("\"DISTANCE\""));
        }

        private sealed class RecordingHandler : HttpMessageHandler
        {
            private readonly string _responseJson;

            public RecordingHandler(string responseJson) { _responseJson = responseJson; }

            public HttpMethod? LastMethod { get; private set; }
            public Uri? LastUri { get; private set; }
            public string? LastRequestBody { get; private set; }
            public string? LastContentType { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastMethod = request.Method;
                LastUri = request.RequestUri;
                if (request.Content != null)
                {
                    LastRequestBody = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                    LastContentType = request.Content.Headers.ContentType?.MediaType;
                }
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_responseJson, Encoding.UTF8, "application/json")
                };
            }
        }
    }

    internal static class PlacesNewTestExtensions
    {
        public static string GetRequestBodyForTest(this SearchTextRequest request)
        {
            // GetRequestBody is protected internal on the base type, so reach it via reflection.
            var method = typeof(GoogleMapsApi.Entities.Common.MapsBaseRequest)
                .GetMethod("GetRequestBody", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var content = (HttpContent?)method!.Invoke(request, null);
            return content!.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }
}
