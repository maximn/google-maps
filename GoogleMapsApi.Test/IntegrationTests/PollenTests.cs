using System.Threading.Tasks;
using GoogleMapsApi.Entities.Pollen.Request;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;

namespace GoogleMapsApi.Test.IntegrationTests
{
    /// <summary>
    /// Live Pollen API tests. The Pollen API is billable, so in live modes these are skipped unless
    /// <c>RUN_BILLABLE_TESTS=true</c> (with a key that has the Pollen API enabled). Under the default
    /// <c>VCR_MODE=replay</c> they serve committed cassettes — free and offline.
    /// </summary>
    [TestFixture]
    [BillableTest]
    public class PollenTests : BaseTestIntegration
    {
        // Madrid, Spain — reliably has pollen forecast coverage.
        private const double Latitude = 40.4168;
        private const double Longitude = -3.7038;

        [Test]
        public async Task Forecast_ValidLocation_ReturnsDailyInfo()
        {
            var response = await Maps.PollenForecast.QueryAsync(new PollenForecastRequest
            {
                ApiKey = ApiKey,
                Latitude = Latitude,
                Longitude = Longitude,
                Days = 3,
            });

            Assert.That(response.RegionCode, Is.Not.Null.And.Not.Empty);
            Assert.That(response.DailyInfo, Is.Not.Null.And.Not.Empty);
            Assert.That(response.DailyInfo![0].Date, Is.Not.Null);
        }

        [Test]
        public async Task HeatmapTile_ValidTile_DownloadsPngBytes()
        {
            var response = await Maps.PollenHeatmapTile.QueryAsync(new PollenHeatmapTileRequest
            {
                ApiKey = ApiKey,
                MapType = PollenMapType.TreeUpi,
                Zoom = 4,
                X = 8,
                Y = 6,
            });

            Assert.That(response.Content, Is.Not.Null.And.Not.Empty);
        }
    }
}
