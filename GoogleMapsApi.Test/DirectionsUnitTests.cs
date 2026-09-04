using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    /// <summary>
    /// Offline coverage for the Directions status codes. The live
    /// <c>DirectionsTests.Directions_ExceedingRouteLength</c> can only observe whatever Google's
    /// current server-side route-length threshold happens to be, so the contract we actually own -
    /// mapping the wire status onto <see cref="DirectionsStatusCodes"/> - is pinned here instead.
    /// </summary>
    [TestFixture]
    public class DirectionsUnitTests
    {
        [Test]
        public async Task MaxRouteLengthExceeded_IsDeserialized()
        {
            const string json = """
                {
                   "geocoded_waypoints" : [],
                   "routes" : [],
                   "status" : "MAX_ROUTE_LENGTH_EXCEEDED"
                }
                """;

            var response = await QueryAsync(json);

            Assert.That(response.Status, Is.EqualTo(DirectionsStatusCodes.MAX_ROUTE_LENGTH_EXCEEDED));
            Assert.That(response.Routes, Is.Empty);
        }

        [Test]
        public async Task ErrorStatus_CarriesErrorMessage()
        {
            const string json = """
                {
                   "error_message" : "The provided API key is invalid.",
                   "routes" : [],
                   "status" : "REQUEST_DENIED"
                }
                """;

            var response = await QueryAsync(json);

            Assert.That(response.Status, Is.EqualTo(DirectionsStatusCodes.REQUEST_DENIED));
            Assert.That(response.ErrorMessage, Is.EqualTo("The provided API key is invalid."));
        }

        [TestCase("OK", DirectionsStatusCodes.OK)]
        [TestCase("NOT_FOUND", DirectionsStatusCodes.NOT_FOUND)]
        [TestCase("ZERO_RESULTS", DirectionsStatusCodes.ZERO_RESULTS)]
        [TestCase("MAX_WAYPOINTS_EXCEEDED", DirectionsStatusCodes.MAX_WAYPOINTS_EXCEEDED)]
        [TestCase("MAX_ROUTE_LENGTH_EXCEEDED", DirectionsStatusCodes.MAX_ROUTE_LENGTH_EXCEEDED)]
        [TestCase("INVALID_REQUEST", DirectionsStatusCodes.INVALID_REQUEST)]
        [TestCase("OVER_QUERY_LIMIT", DirectionsStatusCodes.OVER_QUERY_LIMIT)]
        [TestCase("REQUEST_DENIED", DirectionsStatusCodes.REQUEST_DENIED)]
        [TestCase("UNKNOWN_ERROR", DirectionsStatusCodes.UNKNOWN_ERROR)]
        public async Task EveryDocumentedStatus_RoundTripsFromTheWire(string wireValue, DirectionsStatusCodes expected)
        {
            var response = await QueryAsync($$"""{ "routes" : [], "status" : "{{wireValue}}" }""");

            Assert.That(response.Status, Is.EqualTo(expected));
        }

        private static Task<DirectionsResponse> QueryAsync(string responseJson)
        {
            var request = new DirectionsRequest { Origin = "NYC, USA", Destination = "Miami, USA", ApiKey = "KEY" };
            var handler = new StubHandler(responseJson);
            using var http = new HttpClient(handler);
            return MapsAPIGenericEngine<DirectionsRequest, DirectionsResponse>.QueryGoogleAPIAsync(
                http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);
        }

        private sealed class StubHandler : HttpMessageHandler
        {
            private readonly string _responseJson;
            public StubHandler(string responseJson) { _responseJson = responseJson; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_responseJson, Encoding.UTF8, "application/json")
                });
        }
    }
}
