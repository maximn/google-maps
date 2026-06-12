using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Roads.Request;
using GoogleMapsApi.Entities.Roads.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class RoadsUnitTests
    {
        private static readonly Location P1 = new Location(-35.27801, 149.12958);
        private static readonly Location P2 = new Location(-35.28032, 149.12907);

        #region URL building

        [Test]
        public void SnapToRoads_BuildsExpectedUrl()
        {
            var uri = new SnapToRoadsRequest { ApiKey = "KEY", Path = new[] { P1, P2 }, Interpolate = true }.GetUri();

            Assert.That(uri.GetLeftPart(UriPartial.Path), Is.EqualTo("https://roads.googleapis.com/v1/snapToRoads"));
            Assert.That(uri.Query, Does.Contain("path=-35.27801%2C149.12958%7C-35.28032%2C149.12907"));
            Assert.That(uri.Query, Does.Contain("interpolate=true"));
            Assert.That(uri.Query, Does.Contain("key=KEY"));
        }

        [Test]
        public void SnapToRoads_InterpolateFalse_ByDefault()
        {
            var uri = new SnapToRoadsRequest { ApiKey = "KEY", Path = new[] { P1 } }.GetUri();
            Assert.That(uri.Query, Does.Contain("interpolate=false"));
        }

        [Test]
        public void NearestRoads_BuildsExpectedUrl()
        {
            var uri = new NearestRoadsRequest { ApiKey = "KEY", Points = new[] { P1, P2 } }.GetUri();

            Assert.That(uri.GetLeftPart(UriPartial.Path), Is.EqualTo("https://roads.googleapis.com/v1/nearestRoads"));
            Assert.That(uri.Query, Does.Contain("points=-35.27801%2C149.12958%7C-35.28032%2C149.12907"));
            Assert.That(uri.Query, Does.Contain("key=KEY"));
        }

        [Test]
        public void SpeedLimits_WithPathAndUnits_BuildsExpectedUrl()
        {
            var uri = new SpeedLimitsRequest { ApiKey = "KEY", Path = new[] { P1 }, Units = SpeedUnits.Mph }.GetUri();

            Assert.That(uri.GetLeftPart(UriPartial.Path), Is.EqualTo("https://roads.googleapis.com/v1/speedLimits"));
            Assert.That(uri.Query, Does.Contain("path=-35.27801%2C149.12958"));
            Assert.That(uri.Query, Does.Contain("units=MPH"));
            Assert.That(uri.Query, Does.Contain("key=KEY"));
        }

        [Test]
        public void SpeedLimits_WithPlaceIds_RepeatsPlaceIdParam_AndOmitsUnitsWhenUnset()
        {
            var uri = new SpeedLimitsRequest { ApiKey = "KEY", PlaceIds = new[] { "abc", "def" } }.GetUri();

            Assert.That(uri.Query, Does.Contain("placeId=abc"));
            Assert.That(uri.Query, Does.Contain("placeId=def"));
            Assert.That(uri.Query, Does.Not.Contain("units="));
        }

        #endregion

        #region Validation

        [Test]
        public void SnapToRoads_MissingApiKey_Throws()
            => Assert.Throws<ArgumentException>(() => new SnapToRoadsRequest { Path = new[] { P1 } }.GetUri());

        [Test]
        public void SnapToRoads_NotSsl_Throws()
            => Assert.Throws<ArgumentException>(() => new SnapToRoadsRequest { ApiKey = "KEY", IsSSL = false, Path = new[] { P1 } }.GetUri());

        [Test]
        public void SnapToRoads_EmptyPath_Throws()
            => Assert.Throws<ArgumentException>(() => new SnapToRoadsRequest { ApiKey = "KEY", Path = Array.Empty<Location>() }.GetUri());

        [Test]
        public void SnapToRoads_NullPath_Throws()
            => Assert.Throws<ArgumentException>(() => new SnapToRoadsRequest { ApiKey = "KEY" }.GetUri());

        [Test]
        public void SnapToRoads_OverHundredPoints_Throws()
        {
            var points = Enumerable.Range(0, SnapToRoadsRequest.MaxPathPoints + 1).Select(i => new Location(i, i)).ToArray();
            Assert.Throws<ArgumentException>(() => new SnapToRoadsRequest { ApiKey = "KEY", Path = points }.GetUri());
        }

        [Test]
        public void SpeedLimits_BothPathAndPlaceIds_Throws()
            => Assert.Throws<ArgumentException>(() =>
                new SpeedLimitsRequest { ApiKey = "KEY", Path = new[] { P1 }, PlaceIds = new[] { "abc" } }.GetUri());

        [Test]
        public void SpeedLimits_NeitherPathNorPlaceIds_Throws()
            => Assert.Throws<ArgumentException>(() => new SpeedLimitsRequest { ApiKey = "KEY" }.GetUri());

        #endregion

        #region Response parsing

        [Test]
        public async Task SnapToRoads_Parses_InterpolatedPoint_WithNullOriginalIndex()
        {
            const string json = """
            {
              "snappedPoints": [
                { "location": { "latitude": -35.27801, "longitude": 149.12958 }, "originalIndex": 0, "placeId": "ChIJ-A" },
                { "location": { "latitude": -35.27905, "longitude": 149.12931 }, "placeId": "ChIJ-B" }
              ],
              "warningMessage": "Input path is sparse."
            }
            """;

            var response = await QueryAsync<SnapToRoadsRequest, SnapToRoadsResponse>(
                new SnapToRoadsRequest { ApiKey = "KEY", Path = new[] { P1 }, Interpolate = true }, json);

            Assert.That(response.WarningMessage, Is.EqualTo("Input path is sparse."));
            Assert.That(response.SnappedPoints, Has.Count.EqualTo(2));
            Assert.That(response.SnappedPoints![0].OriginalIndex, Is.EqualTo(0));
            Assert.That(response.SnappedPoints[0].Location!.Latitude, Is.EqualTo(-35.27801));
            Assert.That(response.SnappedPoints[1].OriginalIndex, Is.Null);
            Assert.That(response.SnappedPoints[1].PlaceId, Is.EqualTo("ChIJ-B"));
        }

        [Test]
        public async Task SnapToRoads_NoWarning_LeavesWarningMessageNull()
        {
            const string json = """
            { "snappedPoints": [ { "location": { "latitude": 1, "longitude": 2 }, "originalIndex": 0, "placeId": "X" } ] }
            """;

            var response = await QueryAsync<SnapToRoadsRequest, SnapToRoadsResponse>(
                new SnapToRoadsRequest { ApiKey = "KEY", Path = new[] { P1 } }, json);

            Assert.That(response.WarningMessage, Is.Null);
            Assert.That(response.SnappedPoints, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task NearestRoads_Parses_SnappedPoints()
        {
            const string json = """
            { "snappedPoints": [ { "location": { "latitude": 1.5, "longitude": 2.5 }, "originalIndex": 0, "placeId": "Y" } ] }
            """;

            var response = await QueryAsync<NearestRoadsRequest, NearestRoadsResponse>(
                new NearestRoadsRequest { ApiKey = "KEY", Points = new[] { P1 } }, json);

            Assert.That(response.SnappedPoints, Has.Count.EqualTo(1));
            Assert.That(response.SnappedPoints![0].Location!.Longitude, Is.EqualTo(2.5));
        }

        [Test]
        public async Task SpeedLimits_Parses_SpeedLimitsAndUnits()
        {
            const string json = """
            {
              "speedLimits": [
                { "placeId": "A", "speedLimit": 105, "units": "KMPH" },
                { "placeId": "B", "speedLimit": 65, "units": "MPH" }
              ],
              "snappedPoints": [ { "location": { "latitude": 1, "longitude": 2 }, "originalIndex": 0, "placeId": "A" } ]
            }
            """;

            var response = await QueryAsync<SpeedLimitsRequest, SpeedLimitsResponse>(
                new SpeedLimitsRequest { ApiKey = "KEY", Path = new[] { P1 } }, json);

            Assert.That(response.SpeedLimits, Has.Count.EqualTo(2));
            Assert.That(response.SpeedLimits![0].Value, Is.EqualTo(105));
            Assert.That(response.SpeedLimits[0].Units, Is.EqualTo(SpeedLimitUnits.Kmph));
            Assert.That(response.SpeedLimits[1].Units, Is.EqualTo(SpeedLimitUnits.Mph));
            Assert.That(response.SnappedPoints, Has.Count.EqualTo(1));
        }

        #endregion

        private static Task<TResponse> QueryAsync<TRequest, TResponse>(TRequest request, string responseJson)
            where TRequest : MapsBaseRequest, new()
            where TResponse : IResponseFor<TRequest>
        {
            var handler = new StubHandler(responseJson);
            using var http = new HttpClient(handler);
            return MapsAPIGenericEngine<TRequest, TResponse>.QueryGoogleAPIAsync(
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
