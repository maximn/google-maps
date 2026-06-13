using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.AerialView.Request;
using GoogleMapsApi.Entities.AerialView.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class AerialViewUnitTests
    {
        private const string ActiveJson =
            "{\"uris\":{" +
            "\"IMAGE\":{\"landscapeUri\":\"https://img/l.jpg\",\"portraitUri\":\"https://img/p.jpg\"}," +
            "\"MP4_HIGH\":{\"landscapeUri\":\"https://v/l.mp4\",\"portraitUri\":\"https://v/p.mp4\"}}," +
            "\"state\":\"ACTIVE\"," +
            "\"metadata\":{\"videoId\":\"vid-123\",\"captureDate\":{\"year\":2022,\"month\":10,\"day\":24},\"duration\":\"40s\"}}";

        private const string ProcessingJson =
            "{\"state\":\"PROCESSING\",\"metadata\":{\"videoId\":\"proc-1\"}}";

        [Test]
        public void RenderVideo_GetUri_TargetsAerialViewHost_WithKeyInQueryString()
        {
            var request = new RenderVideoRequest { ApiKey = "abc 123", Address = "x" };

            var uri = request.GetUri();

            Assert.That(uri.Host, Is.EqualTo("aerialview.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/videos:renderVideo"));
            Assert.That(uri.Query, Is.EqualTo("?key=abc%20123"));
        }

        [Test]
        public void RenderVideo_GetUri_MissingApiKey_Throws()
        {
            var request = new RenderVideoRequest { Address = "x" };
            Assert.Throws<InvalidOperationException>(() => request.GetUri());
        }

        [Test]
        public async Task RenderVideo_IssuesPost_WithJsonAddressBody()
        {
            var handler = new RecordingHandler(ProcessingJson);
            using var http = new HttpClient(handler);

            var request = new RenderVideoRequest { ApiKey = "k", Address = "500 W 2nd St, Austin, TX 78701" };

            var response = await MapsAPIGenericEngine<RenderVideoRequest, AerialViewVideoResponse>
                .QueryGoogleAPIAsync(http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastContentType, Is.EqualTo("application/json"));
            Assert.That(handler.LastUri!.Host, Is.EqualTo("aerialview.googleapis.com"));
            Assert.That(handler.LastUri!.AbsolutePath, Is.EqualTo("/v1/videos:renderVideo"));

            using var body = JsonDocument.Parse(handler.LastRequestBody!);
            Assert.That(body.RootElement.GetProperty("address").GetString(), Is.EqualTo("500 W 2nd St, Austin, TX 78701"));

            Assert.That(response.State, Is.EqualTo(VideoState.Processing));
            Assert.That(response.Metadata!.VideoId, Is.EqualTo("proc-1"));
        }

        [Test]
        public void RenderVideo_GetRequestBody_MissingAddress_Throws()
        {
            var request = new RenderVideoRequest { ApiKey = "k" };
            using var http = new HttpClient(new RecordingHandler(ProcessingJson));

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await MapsAPIGenericEngine<RenderVideoRequest, AerialViewVideoResponse>
                    .QueryGoogleAPIAsync(http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null));
            Assert.That(ex!.ParamName, Is.EqualTo("Address"));
        }

        [Test]
        public void LookupVideo_GetUri_ByVideoId()
        {
            var request = new LookupVideoRequest { ApiKey = "k", VideoId = "vid 1" };

            var uri = request.GetUri();

            Assert.That(uri.Host, Is.EqualTo("aerialview.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/videos:lookupVideo"));
            Assert.That(uri.Query, Is.EqualTo("?key=k&videoId=vid%201"));
        }

        [Test]
        public void LookupVideo_GetUri_ByAddress()
        {
            var request = new LookupVideoRequest { ApiKey = "k", Address = "Austin, TX" };

            var uri = request.GetUri();

            Assert.That(uri.Query, Is.EqualTo("?key=k&address=Austin%2C%20TX"));
        }

        [Test]
        public void LookupVideo_GetUri_NeitherSelector_Throws()
        {
            var request = new LookupVideoRequest { ApiKey = "k" };
            Assert.Throws<InvalidOperationException>(() => request.GetUri());
        }

        [Test]
        public void LookupVideo_GetUri_BothSelectors_Throws()
        {
            var request = new LookupVideoRequest { ApiKey = "k", VideoId = "v", Address = "a" };
            Assert.Throws<InvalidOperationException>(() => request.GetUri());
        }

        [Test]
        public async Task LookupVideo_IssuesGet_WithNoBody()
        {
            var handler = new RecordingHandler(ActiveJson);
            using var http = new HttpClient(handler);

            var request = new LookupVideoRequest { ApiKey = "k", VideoId = "vid-123" };

            await MapsAPIGenericEngine<LookupVideoRequest, AerialViewVideoResponse>
                .QueryGoogleAPIAsync(http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Get));
            Assert.That(handler.LastRequestBody, Is.Null);
        }

        [Test]
        public void Response_Deserializes_ActiveVideo_WithUrisAndMetadata()
        {
            var options = JsonSerializerConfiguration.CreateOptions();
            var response = JsonSerializer.Deserialize<AerialViewVideoResponse>(ActiveJson, options);

            Assert.That(response, Is.Not.Null);
            Assert.That(response!.State, Is.EqualTo(VideoState.Active));
            Assert.That(response.Metadata!.VideoId, Is.EqualTo("vid-123"));
            Assert.That(response.Metadata.Duration, Is.EqualTo("40s"));
            Assert.That(response.Metadata.DurationValue, Is.EqualTo(TimeSpan.FromSeconds(40)));
            Assert.That(response.Metadata.CaptureDate!.Year, Is.EqualTo(2022));
            Assert.That(response.Metadata.CaptureDate.Month, Is.EqualTo(10));
            Assert.That(response.Metadata.CaptureDate.Day, Is.EqualTo(24));

            Assert.That(response.Uris!["MP4_HIGH"].LandscapeUri, Is.EqualTo("https://v/l.mp4"));
            Assert.That(response.TryGetUris(MediaFormat.Mp4High, out var mp4), Is.True);
            Assert.That(mp4!.PortraitUri, Is.EqualTo("https://v/p.mp4"));
            Assert.That(response.TryGetUris(MediaFormat.Dash, out var dash), Is.False);
            Assert.That(dash, Is.Null);
        }

        [Test]
        public void Response_Deserializes_ProcessingVideo_WithoutUris()
        {
            var options = JsonSerializerConfiguration.CreateOptions();
            var response = JsonSerializer.Deserialize<AerialViewVideoResponse>(ProcessingJson, options);

            Assert.That(response, Is.Not.Null);
            Assert.That(response!.State, Is.EqualTo(VideoState.Processing));
            Assert.That(response.Uris, Is.Null);
            Assert.That(response.Metadata!.VideoId, Is.EqualTo("proc-1"));
            Assert.That(response.Metadata.DurationValue, Is.Null);
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
