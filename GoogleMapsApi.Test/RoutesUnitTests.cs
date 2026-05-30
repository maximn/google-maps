using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Routes.Request;
using GoogleMapsApi.Entities.Routes.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class RoutesUnitTests
    {
        [Test]
        public void GetUri_TargetsRoutesHost_WithKeyAndFieldsInQueryString()
        {
            var request = new RoutesRequest
            {
                ApiKey = "abc 123",
                Origin = Waypoint.FromAddress("a"),
                Destination = Waypoint.FromAddress("b"),
                FieldMask = "routes.duration,routes.distanceMeters",
            };

            var uri = request.GetUri();

            Assert.That(uri.Host, Is.EqualTo("routes.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/directions/v2:computeRoutes"));
            Assert.That(uri.Query, Does.Contain("key=abc%20123"));
            Assert.That(uri.Query, Does.Contain("$fields=routes.duration%2Croutes.distanceMeters"));
        }

        [Test]
        public void GetUri_MissingApiKey_Throws()
        {
            var request = new RoutesRequest();
            Assert.Throws<InvalidOperationException>(() => request.GetUri());
        }

        [Test]
        public void GetUri_MissingFieldMask_Throws()
        {
            var request = new RoutesRequest { ApiKey = "k", FieldMask = "" };
            Assert.Throws<InvalidOperationException>(() => request.GetUri());
        }

        [Test]
        public void DefaultFieldMask_IsPopulatedAndIncludesCommonShape()
        {
            var request = new RoutesRequest();
            Assert.That(request.FieldMask, Is.Not.Null.And.Not.Empty);
            Assert.That(request.FieldMask, Does.Contain("routes.duration"));
            Assert.That(request.FieldMask, Does.Contain("routes.distanceMeters"));
            Assert.That(request.FieldMask, Does.Contain("routes.polyline.encodedPolyline"));
        }

        [Test]
        public async Task QueryAsync_IssuesPost_WithJsonBody_AndCanonicalFields()
        {
            var canned = "{\"routes\":[{\"distanceMeters\":1234,\"duration\":\"567s\"}]}";
            var handler = new RecordingHandler(canned);
            using var http = new HttpClient(handler);

            var request = new RoutesRequest
            {
                ApiKey = "k",
                Origin = Waypoint.FromAddress("San Francisco, CA"),
                Destination = Waypoint.FromAddress("Mountain View, CA"),
                TravelMode = RoutesTravelMode.Drive,
                RoutingPreference = RoutingPreference.TrafficAware,
                ComputeAlternativeRoutes = true,
                RouteModifiers = new RouteModifiers { AvoidTolls = true },
            };

            var response = await MapsAPIGenericEngine<RoutesRequest, RoutesResponse>
                .QueryGoogleAPIAsync(http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastContentType, Is.EqualTo("application/json"));
            Assert.That(handler.LastUri!.Host, Is.EqualTo("routes.googleapis.com"));

            using var body = JsonDocument.Parse(handler.LastRequestBody!);
            var root = body.RootElement;

            Assert.That(root.GetProperty("origin").GetProperty("address").GetString(), Is.EqualTo("San Francisco, CA"));
            Assert.That(root.GetProperty("destination").GetProperty("address").GetString(), Is.EqualTo("Mountain View, CA"));
            Assert.That(root.GetProperty("travelMode").GetString(), Is.EqualTo("DRIVE"));
            Assert.That(root.GetProperty("routingPreference").GetString(), Is.EqualTo("TRAFFIC_AWARE"));
            Assert.That(root.GetProperty("computeAlternativeRoutes").GetBoolean(), Is.True);
            Assert.That(root.GetProperty("routeModifiers").GetProperty("avoidTolls").GetBoolean(), Is.True);

            Assert.That(response.Routes, Has.Count.EqualTo(1));
            Assert.That(response.Routes![0].DistanceMeters, Is.EqualTo(1234));
            Assert.That(response.Routes[0].Duration, Is.EqualTo("567s"));
            Assert.That(response.Routes[0].DurationSeconds, Is.EqualTo(567d));
        }

        [Test]
        public async Task QueryAsync_OmitsUnsetOptionalFields()
        {
            var handler = new RecordingHandler("{\"routes\":[]}");
            using var http = new HttpClient(handler);

            var request = new RoutesRequest
            {
                ApiKey = "k",
                Origin = Waypoint.FromAddress("A"),
                Destination = Waypoint.FromAddress("B"),
            };

            await MapsAPIGenericEngine<RoutesRequest, RoutesResponse>
                .QueryGoogleAPIAsync(http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            using var body = JsonDocument.Parse(handler.LastRequestBody!);
            var root = body.RootElement;

            Assert.That(root.TryGetProperty("routingPreference", out _), Is.False);
            Assert.That(root.TryGetProperty("computeAlternativeRoutes", out _), Is.False);
            Assert.That(root.TryGetProperty("intermediates", out _), Is.False);
            Assert.That(root.TryGetProperty("departureTime", out _), Is.False);
            Assert.That(root.TryGetProperty("languageCode", out _), Is.False);
        }

        [Test]
        public async Task QueryAsync_SerializesWaypointVariants()
        {
            var handler = new RecordingHandler("{\"routes\":[]}");
            using var http = new HttpClient(handler);

            var request = new RoutesRequest
            {
                ApiKey = "k",
                Origin = Waypoint.FromCoordinates(37.7749, -122.4194),
                Destination = Waypoint.FromPlaceId("ChIJj61dQgK6j4AR4GeTYWZsKWw"),
                Intermediates = new List<Waypoint>
                {
                    Waypoint.FromAddress("Palo Alto, CA"),
                },
            };

            await MapsAPIGenericEngine<RoutesRequest, RoutesResponse>
                .QueryGoogleAPIAsync(http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            using var body = JsonDocument.Parse(handler.LastRequestBody!);
            var root = body.RootElement;

            var originLatLng = root.GetProperty("origin").GetProperty("location").GetProperty("latLng");
            Assert.That(originLatLng.GetProperty("latitude").GetDouble(), Is.EqualTo(37.7749));
            Assert.That(originLatLng.GetProperty("longitude").GetDouble(), Is.EqualTo(-122.4194));

            Assert.That(root.GetProperty("destination").GetProperty("placeId").GetString(), Is.EqualTo("ChIJj61dQgK6j4AR4GeTYWZsKWw"));
            Assert.That(root.GetProperty("intermediates")[0].GetProperty("address").GetString(), Is.EqualTo("Palo Alto, CA"));
        }

        [Test]
        public async Task QueryAsync_DepartureAndArrivalTime_AreMutuallyExclusive()
        {
            var handler = new RecordingHandler("{}");
            using var http = new HttpClient(handler);

            var request = new RoutesRequest
            {
                ApiKey = "k",
                Origin = Waypoint.FromAddress("A"),
                Destination = Waypoint.FromAddress("B"),
                DepartureTime = DateTimeOffset.UtcNow,
                ArrivalTime = DateTimeOffset.UtcNow.AddHours(1),
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await MapsAPIGenericEngine<RoutesRequest, RoutesResponse>
                    .QueryGoogleAPIAsync(http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null));
        }

        [Test]
        public void Response_DeserializesEncodedPolylineAndDecodesPoints()
        {
            // "mz~tF~uflVqAaC" — a short encoded polyline (4 points around lat 37, lng -122).
            const string encoded = "mz~tF~uflVqAaC";
            var json = "{\"routes\":[{\"polyline\":{\"encodedPolyline\":\"" + encoded + "\"}}]}";

            var options = JsonSerializerConfiguration.CreateOptions();
            var response = JsonSerializer.Deserialize<RoutesResponse>(json, options);

            Assert.That(response, Is.Not.Null);
            Assert.That(response!.Routes, Has.Count.EqualTo(1));
            Assert.That(response.Routes![0].Polyline!.EncodedPolyline, Is.EqualTo(encoded));
            Assert.That(response.Routes[0].Polyline!.DecodedPoints, Is.Not.Empty);
        }

        [Test]
        public void Response_DeserializesEnumsWithEnumMemberValues()
        {
            var json = "{\"routes\":[{\"routeLabels\":[\"DEFAULT_ROUTE\",\"FUEL_EFFICIENT\"]," +
                       "\"legs\":[{\"steps\":[{\"navigationInstruction\":{\"maneuver\":\"TURN_LEFT\"}}]}]}]}";

            var options = JsonSerializerConfiguration.CreateOptions();
            var response = JsonSerializer.Deserialize<RoutesResponse>(json, options);

            Assert.That(response!.Routes![0].RouteLabels, Is.EquivalentTo(new[] { RouteLabel.DefaultRoute, RouteLabel.FuelEfficient }));
            Assert.That(response.Routes[0].Legs![0].Steps![0].NavigationInstruction!.Maneuver, Is.EqualTo(Maneuver.TurnLeft));
        }

        [Test]
        public void DurationParser_HandlesFractionalAndMissingValues()
        {
            Assert.That(DurationParser.ToSeconds("123s"), Is.EqualTo(123d));
            Assert.That(DurationParser.ToSeconds("12.5s"), Is.EqualTo(12.5d));
            Assert.That(DurationParser.ToSeconds(null), Is.Null);
            Assert.That(DurationParser.ToSeconds(""), Is.Null);
            Assert.That(DurationParser.ToSeconds("garbage"), Is.Null);
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
}
