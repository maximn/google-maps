using System.Linq;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Elevation.Request;
using NUnit.Framework;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class ElevationTests : BaseTestIntegration
    {
        [Test]
        [Ignore("Google returns: Sorry but your computer or network may be sending automated queries. To protect our users, we can't process your request right now.")]
        public void Elevation_ReturnsCorrectElevation()
        {
            var request = new ElevationRequest { Locations = new[] { new Location(40.7141289, -73.9614074) } };

            var result = GoogleMaps.Elevation.Query(request);

            if (result.Status == Entities.Elevation.Response.Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Entities.Elevation.Response.Status.OK, result.Status);
            Assert.AreEqual(14.78, result.Results.First().Elevation, 1.0);
        }

        [Test]
        [Ignore("Google returns: Sorry but your computer or network may be sending automated queries. To protect our users, we can't process your request right now.")]
        public void ElevationAsync_ReturnsCorrectElevation()
        {
            var request = new ElevationRequest { Locations = new[] { new Location(40.7141289, -73.9614074) } };

            var result = GoogleMaps.Elevation.QueryAsync(request).Result;

            if (result.Status == Entities.Elevation.Response.Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Entities.Elevation.Response.Status.OK, result.Status);
            Assert.AreEqual(14.78, result.Results.First().Elevation, 1.0);
        } 
    }
}
