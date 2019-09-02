using System.Linq;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Elevation.Request;
using NUnit.Framework;
using GoogleMapsApi.Test.Utils;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class ElevationTests : BaseTestIntegration
    {
        [Test]
        public async Task Elevation_ReturnsCorrectElevationAsync()
        {
            var request = new ElevationRequest
            {
                ApiKey = ApiKey,
                Locations = new[] { new Location(40.7141289, -73.9614074) }
            };

            var result = await GoogleMaps.Elevation.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Entities.Elevation.Response.Status.OK, result.Status);
            Assert.AreEqual(16.92, result.Results.First().Elevation, 1.0);
        }

        [Test]
        [System.Obsolete]
        public void Elevation_ReturnsCorrectElevation()
        {
            var request = new ElevationRequest
            {
                ApiKey = ApiKey,
                Locations = new[] { new Location(40.7141289, -73.9614074) }
            };

            var result = GoogleMaps.Elevation.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Entities.Elevation.Response.Status.OK, result.Status);
            Assert.AreEqual(16.92, result.Results.First().Elevation, 1.0);
        }

        [Test]
        public async Task ElevationAsync_ReturnsCorrectElevationAsync()
        {
            var request = new ElevationRequest
            {
                ApiKey = ApiKey,
                Locations = new[] { new Location(40.7141289, -73.9614074) }
            };

            var result = await GoogleMaps.Elevation.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Entities.Elevation.Response.Status.OK, result.Status);
            Assert.AreEqual(16.92, result.Results.First().Elevation, 1.0);
        }

        [Test]
        [System.Obsolete]
        public void ElevationAsync_ReturnsCorrectElevation()
        {
            var request = new ElevationRequest
            {
                ApiKey = ApiKey,
                Locations = new[] { new Location(40.7141289, -73.9614074) }
            };

            var result = GoogleMaps.Elevation.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Entities.Elevation.Response.Status.OK, result.Status);
            Assert.AreEqual(16.92, result.Results.First().Elevation, 1.0);
        }
    }
}
