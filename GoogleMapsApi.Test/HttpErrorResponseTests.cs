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
        public void BadRequest_IncludesGoogleErrorBodyInMessage()
        {
            const string body = """
            {"error":{"code":400,"message":"The start time must be in the future.","status":"INVALID_ARGUMENT"}}
            """;
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => QueryAsync(HttpStatusCode.BadRequest, body));

            Assert.That(ex!.Message, Does.Contain("BadRequest"));
            Assert.That(ex.Message, Does.Contain("INVALID_ARGUMENT"));
            Assert.That(ex.Message, Does.Contain("The start time must be in the future."));
        }

        [Test]
        public void ErrorBody_RedactsApiKeyAndSignature()
        {
            const string body = """
            {"error":{"message":"Invalid request: https://airquality.googleapis.com/v1/forecast:lookup?key=SUPER_SECRET&signature=SIG"}}
            """;
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => QueryAsync(HttpStatusCode.BadRequest, body));

            Assert.That(ex!.Message, Does.Not.Contain("SUPER_SECRET"));
            Assert.That(ex.Message, Does.Not.Contain("SIG"));
            Assert.That(ex.Message, Does.Contain("key=REDACTED"));
            Assert.That(ex.Message, Does.Contain("signature=REDACTED"));
        }

        [Test]
        public void ErrorBody_IsTruncatedWhenOversized()
        {
            var body = new string('x', 10_000);
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => QueryAsync(HttpStatusCode.BadRequest, body));

            Assert.That(ex!.Message.Count(c => c == 'x'), Is.EqualTo(2000));
            Assert.That(ex.Message, Does.EndWith("..."));
        }

        [Test]
        public void EmptyErrorBody_KeepsTheStatusOnlyMessage()
        {
            var ex = Assert.ThrowsAsync<HttpRequestException>(() => QueryAsync(HttpStatusCode.BadRequest, ""));

            Assert.That(ex!.Message, Is.EqualTo("Failed with HttpResponse: BadRequest and message: Bad Request"));
        }

        [Test]
        public void Forbidden_StillThrowsAuthenticationExceptionWithRedactedBody()
        {
            const string body = """
            {"error":{"status":"PERMISSION_DENIED","message":"https://airquality.googleapis.com/v1/currentConditions:lookup?key=SUPER_SECRET is not authorized."}}
            """;
            var ex = Assert.ThrowsAsync<AuthenticationException>(() => QueryAsync(HttpStatusCode.Forbidden, body));

            Assert.That(ex!.Message, Does.Contain("PERMISSION_DENIED"));
            Assert.That(ex.Message, Does.Not.Contain("SUPER_SECRET"));
        }

        [Test]
        public void RequestTimeout_StillThrowsTimeoutException()
        {
            Assert.ThrowsAsync<TimeoutException>(() => QueryAsync(HttpStatusCode.RequestTimeout, "irrelevant"));
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
