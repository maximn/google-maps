using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesRadar.Request;
using GoogleMapsApi.Entities.PlacesRadar.Response;
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
                Sensor = false,
            };

            PlacesRadarResponse result = GoogleMaps.PlacesRadar.Query(request);

            if (result.Status == GoogleMapsApi.Entities.PlacesRadar.Response.Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(GoogleMapsApi.Entities.PlacesRadar.Response.Status.OK, result.Status);
            Assert.IsTrue(result.Results.Count() > 5);
        }
    }
}