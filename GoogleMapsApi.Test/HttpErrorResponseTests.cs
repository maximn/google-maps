using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.AirQuality.Request;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    /// <summary>
    /// Hermetic tests for how the engine reports HTTP failures. Google returns a descriptive JSON error
    /// body on 4xx; it has to reach the caller, redacted and bounded.
    /// </summary>
    [TestFixture]
    public class HttpErrorResponseTests
    {
        private const double Latitude = 37.4;
        private const double Longitude = -122.1;

        [Test]
        public async Task BadRequest_IncludesGoogleErrorBodyInMessage()
        {
            const string body = """
            {"error":{"code":400,"message":"The start time must be in the future.","status":"INVALID_ARGUMENT"}}
            """;
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => QueryAsync(HttpStatusCode.BadRequest, body));

            Assert.That(ex!.Message, Does.Contain("BadRequest"));
            Assert.That(ex.Message, Does.Contain("INVALID_ARGUMENT"));
            Assert.That(ex.Message, Does.Contain("The start time must be in the future."));
        }

        [Test]
        public async Task ErrorBody_RedactsApiKeyAndSignature()
        {
            const string body = """
            {"error":{"message":"Invalid request: https://airquality.googleapis.com/v1/forecast:lookup?key=SUPER_SECRET&signature=SIG"}}
            """;
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => QueryAsync(HttpStatusCode.BadRequest, body));

            Assert.That(ex!.Message, Does.Not.Contain("SUPER_SECRET"));
            Assert.That(ex.Message, Does.Not.Contain("SIG"));
            Assert.That(ex.Message, Does.Contain("key=REDACTED"));
            Assert.That(ex.Message, Does.Contain("signature=REDACTED"));
        }

        // A single backslash; JSON unicode escapes below are built from it so they stay literal in the body.
        private const string Bs = @"\";
        private const string DummySecret = "DUMMY_SECRET_AIzaTest123";

        [TestCase(@"{""error"":{""message"":""https%3A%2F%2Fairquality.googleapis.com%2Fv1%2Flookup%3Fkey%3D" + DummySecret + @"%26x%3D1""}}", TestName = "PercentEncodedKey")]
        [TestCase(@"{""error"":{""message"":""https%3a%2f%2fairquality.googleapis.com%2fv1%2flookup%3fx%3d1%26signature%3d" + DummySecret + @"""}}", TestName = "PercentEncodedSignatureLowercase")]
        [TestCase(@"{""error"":{""message"":""https%253A%252F%252Fh%252Fp%253Fx%253D1%2526key%253D" + DummySecret + @"""}}", TestName = "DoublePercentEncodedKey")]
        [TestCase(@"{""error"":{""message"":""https://airquality.googleapis.com/v1/lookup" + Bs + "u003fkey" + Bs + "u003d" + DummySecret + Bs + @"u0026x=1""}}", TestName = "JsonUnicodeEscapedKey")]
        [TestCase(@"{""error"":{""message"":""https://airquality.googleapis.com/v1/lookup?x=1" + Bs + "u0026signature" + Bs + "u003D" + DummySecret + @"""}}", TestName = "JsonUnicodeEscapedSignature")]
        [TestCase(@"<html><a href=""https://airquality.googleapis.com/v1/lookup?x=1&amp;key=" + DummySecret + @"&amp;y=2"">retry</a></html>", TestName = "HtmlAmpEntityKey")]
        [TestCase(@"<html><a href=""https://airquality.googleapis.com/v1/lookup?x=1&amp;signature=" + DummySecret + @""">retry</a></html>", TestName = "HtmlAmpEntitySignature")]
        [TestCase(@"<p>https://airquality.googleapis.com/v1/lookup?x=1&#38;key&#61;" + DummySecret + @"</p>", TestName = "HtmlDecimalEntityKey")]
        [TestCase(@"<p>https://airquality.googleapis.com/v1/lookup?x=1&#x26;signature&#x3D;" + DummySecret + @"</p>", TestName = "HtmlHexEntitySignature")]
        public async Task ErrorBody_RedactsEncodedApiKeyAndSignature(string body)
        {
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => QueryAsync(HttpStatusCode.BadRequest, body));

            Assert.That(ex!.Message, Does.Not.Contain(DummySecret));
            Assert.That(ex.Message, Does.Contain("REDACTED"));
        }

        [Test]
        public async Task ErrorBody_LeavesLookalikeParametersAlone()
        {
            const string body = """
            {"error":{"message":"https://airquality.googleapis.com/v1/lookup?keyword=visible&monkey=banana&x=1"}}
            """;
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => QueryAsync(HttpStatusCode.BadRequest, body));

            Assert.That(ex!.Message, Does.Contain("keyword=visible"));
            Assert.That(ex.Message, Does.Contain("monkey=banana"));
            Assert.That(ex.Message, Does.Not.Contain("REDACTED"));
        }

        [Test]
        public async Task ErrorBody_IsTruncatedWhenOversized()
        {
            var body = new string('x', 10_000);
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => QueryAsync(HttpStatusCode.BadRequest, body));

            Assert.That(ex!.Message.Count(c => c == 'x'), Is.EqualTo(2000));
            Assert.That(ex.Message, Does.EndWith("..."));
        }

        [Test]
        public async Task EmptyErrorBody_KeepsTheStatusOnlyMessage()
        {
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => QueryAsync(HttpStatusCode.BadRequest, ""));

            Assert.That(ex!.Message, Is.EqualTo("Failed with HttpResponse: BadRequest and message: Bad Request"));
        }

        [Test]
        public async Task Forbidden_StillThrowsAuthenticationExceptionWithRedactedBody()
        {
            const string body = """
            {"error":{"status":"PERMISSION_DENIED","message":"https://airquality.googleapis.com/v1/currentConditions:lookup?key=SUPER_SECRET is not authorized."}}
            """;
            var ex = await Assert.ThrowsAsync<AuthenticationException>(() => QueryAsync(HttpStatusCode.Forbidden, body));

            Assert.That(ex!.Message, Does.Contain("PERMISSION_DENIED"));
            Assert.That(ex.Message, Does.Not.Contain("SUPER_SECRET"));
        }

        [Test]
        public async Task RequestTimeout_StillThrowsTimeoutException()
        {
            await Assert.ThrowsAsync<TimeoutException>(() => QueryAsync(HttpStatusCode.RequestTimeout, "irrelevant"));
        }

        private static async Task QueryAsync(HttpStatusCode status, string body)
        {
            using var handler = new FailingHandler(status, body);
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            await client.AirQualityCurrentConditions.QueryAsync(
                new CurrentConditionsRequest { Latitude = Latitude, Longitude = Longitude });
        }

        private sealed class FailingHandler : HttpMessageHandler
        {
            private readonly HttpStatusCode _status;
            private readonly string _body;

            public FailingHandler(HttpStatusCode status, string body)
            {
                _status = status;
                _body = body;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new HttpResponseMessage(_status)
                {
                    Content = new StringContent(_body, Encoding.UTF8, "application/json"),
                });
            }
        }
    }
}
