using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Diagnostics;
using GoogleMapsApi.Entities.Geocoding.Request;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    /// <summary>
    /// Hermetic tests asserting that each API call records to the metric instruments published from the
    /// <see cref="GoogleMapsMetrics.MeterName"/> meter. No live network calls — every HTTP exchange is
    /// intercepted by <see cref="StubHandler"/>.
    /// </summary>
    [TestFixture]
    public class DiagnosticsMetricsTests
    {
        private const string OkGeocodingJson = "{\"status\":\"OK\",\"results\":[]}";

        [Test]
        public async Task QueryAsync_Success_RecordsRequestAndDuration_NoErrors()
        {
            using var collector = new MetricsCollector();

            using var handler = new StubHandler(HttpStatusCode.OK, OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "1600 Amphitheatre Pkwy" });

            var requests = collector.Single("gmaps.client.requests");
            Assert.That(requests.Value, Is.EqualTo(1L));
            Assert.That(requests.Tags["gmaps.api"], Is.EqualTo("Geocoding"));
            Assert.That(requests.Tags["http.request.method"], Is.EqualTo("GET"));
            Assert.That(requests.Tags.ContainsKey("http.response.status_code"), Is.False);

            var duration = collector.Single("gmaps.client.request.duration");
            Assert.That(duration.Value, Is.GreaterThanOrEqualTo(0.0));
            Assert.That(duration.Tags["gmaps.api"], Is.EqualTo("Geocoding"));
            Assert.That(duration.Tags["http.response.status_code"], Is.EqualTo(200));
            Assert.That(duration.Tags["gmaps.response_status"], Is.EqualTo("OK"));

            Assert.That(collector.Count("gmaps.client.request.errors"), Is.EqualTo(0));
        }

        [Test]
        public void QueryAsync_HttpError_RecordsErrorAndDuration()
        {
            using var collector = new MetricsCollector();

            using var handler = new StubHandler(HttpStatusCode.Forbidden, "denied");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            Assert.ThrowsAsync<System.Security.Authentication.AuthenticationException>(
                (Func<Task>)(() => client.Geocode.QueryAsync(new GeocodingRequest { Address = "a" })));

            Assert.That(collector.Single("gmaps.client.requests").Value, Is.EqualTo(1L));

            var error = collector.Single("gmaps.client.request.errors");
            Assert.That(error.Value, Is.EqualTo(1L));
            Assert.That(error.Tags["gmaps.api"], Is.EqualTo("Geocoding"));
            Assert.That(error.Tags["error.type"], Is.Not.Null);

            var duration = collector.Single("gmaps.client.request.duration");
            Assert.That(duration.Tags["http.response.status_code"], Is.EqualTo(403));
        }

        [Test]
        public async Task QueryAsync_WithoutListener_DoesNotThrow()
        {
            using var handler = new StubHandler(HttpStatusCode.OK, OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            var response = await client.Geocode.QueryAsync(new GeocodingRequest { Address = "a" });

            Assert.That(response, Is.Not.Null);
        }

        /// <summary>Captures measurements from the GoogleMapsApi meter for the duration of a test.</summary>
        private sealed class MetricsCollector : IDisposable
        {
            private readonly MeterListener _listener;
            private readonly List<(string Instrument, double Value, Dictionary<string, object?> Tags)> _records = new();

            public MetricsCollector()
            {
                _listener = new MeterListener
                {
                    InstrumentPublished = (instrument, listener) =>
                    {
                        if (instrument.Meter.Name == GoogleMapsMetrics.MeterName)
                            listener.EnableMeasurementEvents(instrument);
                    }
                };
                _listener.SetMeasurementEventCallback<long>(
                    (inst, value, tags, _) => Add(inst.Name, value, tags));
                _listener.SetMeasurementEventCallback<double>(
                    (inst, value, tags, _) => Add(inst.Name, value, tags));
                _listener.Start();
            }

            private void Add(string instrument, double value, ReadOnlySpan<KeyValuePair<string, object?>> tags)
            {
                var dict = new Dictionary<string, object?>();
                foreach (var tag in tags)
                    dict[tag.Key] = tag.Value;
                _records.Add((instrument, value, dict));
            }

            public int Count(string instrument) => _records.Count(r => r.Instrument == instrument);

            public (double Value, Dictionary<string, object?> Tags) Single(string instrument)
            {
                var record = _records.Single(r => r.Instrument == instrument);
                return (record.Value, record.Tags);
            }

            public void Dispose() => _listener.Dispose();
        }

        private sealed class StubHandler : HttpMessageHandler
        {
            private readonly HttpStatusCode _statusCode;
            private readonly string _responseBody;

            public StubHandler(HttpStatusCode statusCode, string responseBody)
            {
                _statusCode = statusCode;
                _responseBody = responseBody;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new HttpResponseMessage(_statusCode)
                {
                    Content = new StringContent(_responseBody)
                });
            }
        }
    }
}
