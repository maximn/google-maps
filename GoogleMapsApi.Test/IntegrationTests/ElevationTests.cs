using System.Linq;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Elevation.Request;
using NUnit.Framework;
using GoogleMapsApi.Test.Utils;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class ElevationTests : BaseTestIntegration
    {
        [Test]
        public async Task Elevation_ReturnsCorrectElevation()
        {
            var request = new ElevationRequest
            {
                ApiKey = ApiKey,
                Locations = new[] { new Location(40.7141289, -73.9614074) }
            };

            var result = await GoogleMaps.Elevation.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Entities.Elevation.Response.Status.OK));
            Assert.That(result.Results.First().Elevation, Is.EqualTo(16.92).Within(1.0));
            Assert.That(result.Results.First().Resolution, Is.EqualTo(75.0).Within(10.0));
        }

        [Test]
        public void ElevationAsync_ReturnsCorrectElevation()
        {
            var request = new ElevationRequest
            {
                ApiKey = ApiKey,
                Locations = new[] { new Location(40.7141289, -73.9614074) }
            };

            var result = GoogleMaps.Elevation.QueryAsync(request).Result;

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Entities.Elevation.Response.Status.OK));
            Assert.That(result.Results.First().Elevation, Is.EqualTo(16.92).Within(1.0));
            Assert.That(result.Results.First().Resolution, Is.EqualTo(75.0).Within(10.0));
        } 
    }
}
