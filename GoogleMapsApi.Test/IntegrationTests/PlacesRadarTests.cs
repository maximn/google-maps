using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesRadar.Request;
using GoogleMapsApi.Entities.PlacesRadar.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;
using System.Linq;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class PlacesRadarTests : BaseTestIntegration
    {
        [Test]
        public void ReturnsRadarSearchRequest()
        {
            var request = new PlacesRadarRequest
            {
                ApiKey = ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
            };

            PlacesRadarResponse result = GoogleMaps.PlacesRadar.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsTrue(result.Results.Count() > 5);
        }

        [Test]
        public void TestRadarSearchType()
        {
            var request = new PlacesRadarRequest
            {
                ApiKey = ApiKey,
                Radius = 10000,
                Location = new Location(64.6247243, 21.0747553), // Skellefteå, Sweden
                Type = "airport",
            };

            PlacesRadarResponse result = GoogleMaps.PlacesRadar.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsTrue(result.Results.Any());
        }
    }
}