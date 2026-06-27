using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Hermetic tests asserting that each API call emits a correctly tagged OpenTelemetry span from the
    /// <see cref="GoogleMapsActivity.SourceName"/> source. No live network calls — every HTTP exchange is
    /// intercepted by <see cref="StubHandler"/>.
    /// </summary>
    [TestFixture]
    public class DiagnosticsTracingTests
    {
        private const string OkGeocodingJson = "{\"status\":\"OK\",\"results\":[]}";

        private static ActivityListener CreateListener(List<Activity> captured)
        {
            var listener = new ActivityListener
            {
                ShouldListenTo = source => source.Name == GoogleMapsActivity.SourceName,
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
                ActivityStopped = activity => captured.Add(activity)
            };
            ActivitySource.AddActivityListener(listener);
            return listener;
        }

        [Test]
        public async Task QueryAsync_EmitsClientSpan_WithSemanticConventionTags()
        {
            var captured = new List<Activity>();
            using var listener = CreateListener(captured);

            using var handler = new StubHandler(HttpStatusCode.OK, OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "secret-key" });

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "1600 Amphitheatre Pkwy" });

            Assert.That(captured, Has.Count.EqualTo(1));
            var activity = captured[0];
            Assert.That(activity.OperationName, Is.EqualTo("GoogleMapsApi Geocoding"));
            Assert.That(activity.Kind, Is.EqualTo(ActivityKind.Client));
            Assert.That(activity.GetTagItem("gmaps.api"), Is.EqualTo("Geocoding"));
            Assert.That(activity.GetTagItem("http.request.method"), Is.EqualTo("GET"));
            Assert.That(activity.GetTagItem("server.address"), Is.EqualTo("maps.googleapis.com"));
            Assert.That(activity.GetTagItem("http.response.status_code"), Is.EqualTo(200));
            Assert.That(activity.GetTagItem("gmaps.response_status"), Is.EqualTo("OK"));
            Assert.That(activity.Status, Is.EqualTo(ActivityStatusCode.Unset));
        }

        [Test]
        public async Task QueryAsync_RedactsApiKey_FromUrlFullTag()
        {
            var captured = new List<Activity>();
            using var listener = CreateListener(captured);

            using var handler = new StubHandler(HttpStatusCode.OK, OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "super-secret-key" });

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "Pier 39" });

            var urlFull = (string)captured.Single().GetTagItem("url.full")!;
            Assert.That(urlFull, Does.Contain("key=REDACTED"));
            Assert.That(urlFull, Does.Not.Contain("super-secret-key"));
        }

        [Test]
        public void QueryAsync_HttpError_MarksSpanAsError()
        {
            var captured = new List<Activity>();
            using var listener = CreateListener(captured);

            using var handler = new StubHandler(HttpStatusCode.Forbidden, "denied");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            Assert.ThrowsAsync<System.Security.Authentication.AuthenticationException>(
                (Func<Task>)(() => client.Geocode.QueryAsync(new GeocodingRequest { Address = "a" })));

            var activity = captured.Single();
            Assert.That(activity.Status, Is.EqualTo(ActivityStatusCode.Error));
            Assert.That(activity.GetTagItem("http.response.status_code"), Is.EqualTo(403));
            Assert.That(activity.GetTagItem("error.type"), Is.Not.Null);
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

        [Test]
        public async Task QueryAsync_WhenGoogleMapsSpanNotRecorded_DoesNotTagAmbientParentSpan()
        {
            using var parentSource = new ActivitySource("Test.Parent.Source");
            using var parentListener = new ActivityListener
            {
                ShouldListenTo = s => s.Name == "Test.Parent.Source",
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
            };
            ActivitySource.AddActivityListener(parentListener);

            // Intentionally NO listener for GoogleMapsActivity.SourceName: the engine's StartActivity
            // returns null, leaving Activity.Current == the parent span.
            using var parent = parentSource.StartActivity("user-operation");
            Assert.That(parent, Is.Not.Null);

            using var handler = new StubHandler(HttpStatusCode.OK, OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "a" });

            Assert.That(parent!.GetTagItem("http.response.status_code"), Is.Null);
        }

        [Test]
        public async Task QueryAsync_TagsStatusCode_OnStartedSpan_WhenNestedUnderParent()
        {
            var captured = new List<Activity>();
            using var listener = CreateListener(captured);

            using var parentSource = new ActivitySource("Test.Parent.Source");
            using var parentListener = new ActivityListener
            {
                ShouldListenTo = s => s.Name == "Test.Parent.Source",
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllDataAndRecorded,
            };
            ActivitySource.AddActivityListener(parentListener);
            using var parent = parentSource.StartActivity("user-operation");

            using var handler = new StubHandler(HttpStatusCode.OK, OkGeocodingJson);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "a" });

            var span = captured.Single(a => a.OperationName == "GoogleMapsApi Geocoding");
            Assert.That(span.GetTagItem("http.response.status_code"), Is.EqualTo(200));
            Assert.That(parent!.GetTagItem("http.response.status_code"), Is.Null);
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
