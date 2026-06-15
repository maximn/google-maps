using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Test.Vcr;
using NUnit.Framework;
using GeocodingStatus = GoogleMapsApi.Entities.Geocoding.Response.Status;

namespace GoogleMapsApi.Test
{
    /// <summary>
    /// Hermetic tests for the VCR harness itself — they use stub inner handlers and a temp cassette, so
    /// they need no Google key and hit no network. They prove the record→replay machinery independently of
    /// the recorded integration cassettes.
    /// </summary>
    [TestFixture]
    public class VcrHarnessTests
    {
        private const string SecretKey = "super-secret-key";
        private static readonly Uri GeocodeUri =
            new($"https://maps.googleapis.com/maps/api/geocode/json?address=NYC&key={SecretKey}");
        private const string ResponseJson = "{\"status\":\"OK\",\"results\":[]}";

        private string _cassettePath = null!;

        [SetUp]
        public void SetUp()
        {
            _cassettePath = Path.Combine(Path.GetTempPath(), "vcr-tests", Guid.NewGuid().ToString("N"), "cassette.json");
        }

        [TearDown]
        public void TearDown()
        {
            var dir = Path.GetDirectoryName(_cassettePath)!;
            if (Directory.Exists(dir))
                Directory.Delete(dir, recursive: true);
        }

        [Test]
        public async Task Record_ThenReplay_ServesFromCassette_WithoutHittingNetwork()
        {
            await RecordOnce();

            // Replay with a handler that fails if the network is touched, and a *different* key in the URL
            // to prove matching works on the redacted form.
            using var replay = new HttpMessageInvoker(
                new VcrDelegatingHandler(VcrMode.Replay, _cassettePath, new ThrowingHandler()));

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://maps.googleapis.com/maps/api/geocode/json?address=NYC&key=a-totally-different-key");
            using var response = await replay.SendAsync(request, CancellationToken.None);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(await response.Content.ReadAsStringAsync(), Is.EqualTo(ResponseJson));
        }

        [Test]
        public async Task Record_RedactsApiKeyOnDisk()
        {
            await RecordOnce();

            var onDisk = await File.ReadAllTextAsync(_cassettePath);

            Assert.That(onDisk, Does.Not.Contain(SecretKey));
            Assert.That(onDisk, Does.Contain("key=REDACTED"));
        }

        [Test]
        public async Task Record_ReplacesExistingCassette()
        {
            await RecordOnce();

            const string replacementJson = "{\"status\":\"ZERO_RESULTS\",\"results\":[]}";
            using (var record = new HttpMessageInvoker(
                       new VcrDelegatingHandler(VcrMode.Record, _cassettePath, new StubHandler(replacementJson))))
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, GeocodeUri);
                using var response = await record.SendAsync(request, CancellationToken.None);
                Assert.That(await response.Content.ReadAsStringAsync(), Is.EqualTo(replacementJson));
            }

            using var replay = new HttpMessageInvoker(
                new VcrDelegatingHandler(VcrMode.Replay, _cassettePath, new ThrowingHandler()));
            using var replayRequest = new HttpRequestMessage(HttpMethod.Get, GeocodeUri);
            using var replayResponse = await replay.SendAsync(replayRequest, CancellationToken.None);

            Assert.That(await replayResponse.Content.ReadAsStringAsync(), Is.EqualTo(replacementJson));
        }

        [Test]
        public void Replay_WithNoMatchingCassette_FailsLoudly()
        {
            using var replay = new HttpMessageInvoker(
                new VcrDelegatingHandler(VcrMode.Replay, _cassettePath, new ThrowingHandler()));

            var request = new HttpRequestMessage(HttpMethod.Get, GeocodeUri);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(
                () => replay.SendAsync(request, CancellationToken.None));
            Assert.That(ex!.Message, Does.Contain("No recorded response"));
            Assert.That(ex.Message, Does.Contain("VCR_MODE=record"));
        }

        [Test]
        public async Task Replay_HonorsCancellation()
        {
            await RecordOnce();

            using var replay = new HttpMessageInvoker(
                new VcrDelegatingHandler(VcrMode.Replay, _cassettePath, new ThrowingHandler()));

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            var request = new HttpRequestMessage(HttpMethod.Get, GeocodeUri);
            Assert.ThrowsAsync<OperationCanceledException>(() => replay.SendAsync(request, cts.Token));
        }

        [TestCase(null, VcrMode.Replay)]
        [TestCase("", VcrMode.Replay)]
        [TestCase("replay", VcrMode.Replay)]
        [TestCase("RECORD", VcrMode.Record)]
        [TestCase("Auto", VcrMode.Auto)]
        [TestCase("live", VcrMode.Live)]
        public void VcrModes_Parse_MapsKnownValues(string? value, VcrMode expected)
        {
            Assert.That(VcrModes.Parse(value), Is.EqualTo(expected));
        }

        [Test]
        public void VcrModes_Parse_RejectsUnknownValue()
        {
            Assert.Throws<InvalidOperationException>(() => VcrModes.Parse("bogus"));
        }

        [Test]
        public async Task EndToEnd_ThroughGoogleMapsClient_RecordsThenReplays()
        {
            const string geocodeJson = "{\"status\":\"OK\",\"results\":[]}";

            // Record through the real client + engine (stub stands in for Google).
            using (var http = new HttpClient(new VcrDelegatingHandler(VcrMode.Record, _cassettePath, new StubHandler(geocodeJson))))
            {
                var client = new GoogleMapsClient(http);
                var recorded = await client.Geocode.QueryAsync(
                    new GeocodingRequest { ApiKey = "secret", Address = "285 Bedford Ave, Brooklyn, NY" });
                Assert.That(recorded.Status, Is.EqualTo(GeocodingStatus.OK));
            }

            // Replay through a fresh client with a different key (matches on the redacted URL) and a
            // handler that fails if the network is touched.
            using (var http = new HttpClient(new VcrDelegatingHandler(VcrMode.Replay, _cassettePath, new ThrowingHandler())))
            {
                var client = new GoogleMapsClient(http);
                var replayed = await client.Geocode.QueryAsync(
                    new GeocodingRequest { ApiKey = "different", Address = "285 Bedford Ave, Brooklyn, NY" });
                Assert.That(replayed.Status, Is.EqualTo(GeocodingStatus.OK));
            }
        }

        private async Task RecordOnce()
        {
            using var record = new HttpMessageInvoker(
                new VcrDelegatingHandler(VcrMode.Record, _cassettePath, new StubHandler(ResponseJson)));

            var request = new HttpRequestMessage(HttpMethod.Get, GeocodeUri);
            using var response = await record.SendAsync(request, CancellationToken.None);
            Assert.That(await response.Content.ReadAsStringAsync(), Is.EqualTo(ResponseJson));
            Assert.That(File.Exists(_cassettePath), Is.True, "Record mode should write a cassette.");
        }

        private sealed class StubHandler : HttpMessageHandler
        {
            private readonly string _body;
            public StubHandler(string body) => _body = body;

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_body, System.Text.Encoding.UTF8, "application/json"),
                });
        }

        private sealed class ThrowingHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                => throw new InvalidOperationException("Network must not be hit in replay mode.");
        }
    }
}
