using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    /// <summary>
    /// Hermetic tests for the instance-based <see cref="GoogleMapsClient"/>. No live network calls —
    /// every HTTP exchange is intercepted by <see cref="CapturingHandler"/>.
    /// </summary>
    [TestFixture]
    public class GoogleMapsClientTests
    {
        private const string OkGeocodingJson = "{\"status\":\"OK\",\"results\":[]}";

        [Test]
        public async Task QueryAsync_RoundTrip_DeserializesResponse()
        {
            using var handler = new CapturingHandler(OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "test-key" });

            var response = await client.Geocode.QueryAsync(new GeocodingRequest { Address = "1600 Amphitheatre Pkwy" });

            Assert.That(response.Status, Is.EqualTo(Status.OK));
            Assert.That(handler.LastRequestUri, Is.Not.Null);
            Assert.That(handler.LastRequestUri!.Host, Is.EqualTo("maps.googleapis.com"));
        }

        [Test]
        public async Task QueryAsync_AmbientApiKey_IsAppliedWhenRequestHasNone()
        {
            using var handler = new CapturingHandler(OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "ambient-key" });

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "Pier 39" });

            Assert.That(handler.LastRequestUri!.Query, Does.Contain("key=ambient-key"));
        }

        [Test]
        public async Task QueryAsync_ExplicitApiKey_IsNotOverwrittenByAmbient()
        {
            using var handler = new CapturingHandler(OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "ambient-key" });

            var request = new GeocodingRequest { Address = "Pier 39", ApiKey = "explicit-key" };
            await client.Geocode.QueryAsync(request);

            Assert.That(handler.LastRequestUri!.Query, Does.Contain("key=explicit-key"));
            Assert.That(handler.LastRequestUri!.Query, Does.Not.Contain("ambient-key"));
        }

        [Test]
        public async Task QueryAsync_NoApiKeyAnywhere_SendsRequestWithoutKey()
        {
            using var handler = new CapturingHandler(OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http);

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "Pier 39" });

            Assert.That(handler.LastRequestUri!.Query, Does.Not.Contain("key="));
        }

        [Test]
        public async Task OnUriCreated_Event_IsScopedToSingleClient()
        {
            using var handler = new CapturingHandler(OkGeocodingJson);
            using var http = new HttpClient(handler);
            var clientA = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "key-a" });
            var clientB = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "key-b" });

            int firedOnA = 0, firedOnB = 0;
            clientA.Geocode.OnUriCreated += uri => { firedOnA++; return uri; };
            clientB.Geocode.OnUriCreated += uri => { firedOnB++; return uri; };

            await clientA.Geocode.QueryAsync(new GeocodingRequest { Address = "a" });

            Assert.That(firedOnA, Is.EqualTo(1));
            Assert.That(firedOnB, Is.EqualTo(0));
        }

        [Test]
        public async Task OnUriCreated_Event_CanRewriteUri()
        {
            using var handler = new CapturingHandler(OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            client.Geocode.OnUriCreated += _ => new Uri("https://proxy.example.com/maps/api/geocode/json?key=k");

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "a" });

            Assert.That(handler.LastRequestUri!.Host, Is.EqualTo("proxy.example.com"));
        }

        [Test]
        public async Task OnRawResponseReceived_Event_FiresWithBytes()
        {
            using var handler = new CapturingHandler(OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            byte[]? captured = null;
            client.Geocode.OnRawResponseReceived += bytes => captured = bytes;

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "a" });

            Assert.That(captured, Is.Not.Null);
            Assert.That(System.Text.Encoding.UTF8.GetString(captured!), Is.EqualTo(OkGeocodingJson));
        }

        [Test]
        public async Task TwoClients_WithDifferentKeys_DoNotCrossContaminate()
        {
            using var handler = new CapturingHandler(OkGeocodingJson);
            using var http = new HttpClient(handler);
            var clientA = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "key-a" });
            var clientB = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "key-b" });

            await clientA.Geocode.QueryAsync(new GeocodingRequest { Address = "a" });
            var uriA = handler.LastRequestUri!;

            await clientB.Geocode.QueryAsync(new GeocodingRequest { Address = "b" });
            var uriB = handler.LastRequestUri!;

            Assert.That(uriA.Query, Does.Contain("key=key-a"));
            Assert.That(uriB.Query, Does.Contain("key=key-b"));
        }

        [Test]
        public void QueryAsync_CancelledToken_Throws()
        {
            using var handler = new CapturingHandler(OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Assert.ThrowsAsync<TaskCanceledException>((Func<Task>)(() =>
                client.Geocode.QueryAsync(new GeocodingRequest { Address = "a" }, cts.Token)));
        }

        [Test]
        public void QueryAsync_NullRequest_ThrowsArgumentNullException()
        {
            using var handler = new CapturingHandler(OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http);

            Assert.ThrowsAsync<ArgumentNullException>((Func<Task>)(() => client.Geocode.QueryAsync(null!)));
        }

        [Test]
        public void Constructor_NullHttpClient_Throws()
        {
            Assert.Throws<ArgumentNullException>((Action)(() => _ = new GoogleMapsClient(null!)));
            Assert.Throws<ArgumentNullException>((Action)(() => _ = new GoogleMapsClient(null!, new GoogleMapsClientOptions())));
        }

        [Test]
        public void Constructor_NullOptions_Throws()
        {
            using var http = new HttpClient(new CapturingHandler(OkGeocodingJson));
            Assert.Throws<ArgumentNullException>((Action)(() => _ = new GoogleMapsClient(http, null!)));
        }

        [Test]
        public void AllApiAccessors_AreNonNull()
        {
            using var http = new HttpClient(new CapturingHandler(OkGeocodingJson));
            var client = new GoogleMapsClient(http);

            Assert.That(client.Geocode, Is.Not.Null);
            Assert.That(client.Directions, Is.Not.Null);
            Assert.That(client.Elevation, Is.Not.Null);
            Assert.That(client.Places, Is.Not.Null);
            Assert.That(client.PlacesText, Is.Not.Null);
            Assert.That(client.TimeZone, Is.Not.Null);
            Assert.That(client.PlacesDetails, Is.Not.Null);
            Assert.That(client.PlaceAutocomplete, Is.Not.Null);
            Assert.That(client.PlacesNearBy, Is.Not.Null);
            Assert.That(client.PlacesFind, Is.Not.Null);
            Assert.That(client.DistanceMatrix, Is.Not.Null);
        }

        private sealed class CapturingHandler : HttpMessageHandler
        {
            private readonly string _responseBody;

            public CapturingHandler(string responseBody)
            {
                _responseBody = responseBody;
            }

            public Uri? LastRequestUri { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                LastRequestUri = request.RequestUri;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_responseBody)
                });
            }
        }
    }
}
