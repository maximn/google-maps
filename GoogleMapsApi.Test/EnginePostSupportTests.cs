using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class EnginePostSupportTests
    {
        [Test]
        public async Task RequestWithNullBody_UsesGet_GeocodingRegression()
        {
            var handler = new RecordingHandler("{\"status\":\"ZERO_RESULTS\",\"results\":[]}");
            using var http = new HttpClient(handler);

            var request = new GeocodingRequest { Address = "anywhere", ApiKey = "k" };

            await MapsAPIGenericEngine<GeocodingRequest, GeocodingResponse>.QueryGoogleAPIAsync(
                http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastRequestBody, Is.Null);
        }

        [Test]
        public async Task RequestWithBody_UsesPost_AndSendsJsonContent()
        {
            var handler = new RecordingHandler("{\"ok\":true}");
            using var http = new HttpClient(handler);

            var request = new PostProbeRequest { Payload = "{\"hello\":\"world\"}", ApiKey = "k" };

            await MapsAPIGenericEngine<PostProbeRequest, PostProbeResponse>.QueryGoogleAPIAsync(
                http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastRequestBody, Is.EqualTo("{\"hello\":\"world\"}"));
            Assert.That(handler.LastContentType, Is.EqualTo("application/json"));
        }

        private sealed class RecordingHandler : HttpMessageHandler
        {
            private readonly string _responseJson;

            public RecordingHandler(string responseJson) { _responseJson = responseJson; }

            public HttpMethod? LastMethod { get; private set; }
            public string? LastRequestBody { get; private set; }
            public string? LastContentType { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastMethod = request.Method;
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

        public sealed class PostProbeRequest : MapsBaseRequest
        {
            public string Payload { get; set; } = "{}";

            protected internal override HttpContent? GetRequestBody()
                => new StringContent(Payload, Encoding.UTF8, "application/json");
        }

        public sealed class PostProbeResponse : IResponseFor<PostProbeRequest>
        {
            public bool Ok { get; set; }
        }
    }
}
