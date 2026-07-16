using System;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.AirQuality.Request;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;

namespace GoogleMapsApi.Test.IntegrationTests
{
    /// <summary>
    /// Live Air Quality API tests. The Air Quality API is billable, so in live modes these are skipped
    /// unless <c>RUN_BILLABLE_TESTS=true</c> (with a key that has the Air Quality API enabled). Under the
    /// default <c>VCR_MODE=replay</c> they serve committed cassettes — free and offline.
    /// </summary>
    [TestFixture]
    [BillableTest]
    public class AirQualityTests : BaseTestIntegration
    {
        // Mountain View, CA.
        private const double Latitude = 37.4220;
        private const double Longitude = -122.0841;

        [Test]
        public async Task CurrentConditions_ValidLocation_ReturnsIndexes()
        {
            var response = await Maps.AirQualityCurrentConditions.QueryAsync(new CurrentConditionsRequest
            {
                ApiKey = ApiKey,
                Latitude = Latitude,
                Longitude = Longitude,
                ExtraComputations = new() { ExtraComputation.HealthRecommendations, ExtraComputation.PollutantConcentration },
            });

            Assert.That(response.RegionCode, Is.Not.Null.And.Not.Empty);
            Assert.That(response.Indexes, Is.Not.Null.And.Not.Empty);
            Assert.That(response.Indexes![0].Aqi, Is.GreaterThan(0));
        }

        [Test]
        public async Task Forecast_ValidLocation_ReturnsHourlyForecasts()
        {
            // forecast:lookup requires an explicit future time window; without one the API returns 400.
            var now = DateTimeOffset.UtcNow;
            var response = await Maps.AirQualityForecast.QueryAsync(new ForecastRequest
            {
                ApiKey = ApiKey,
                Latitude = Latitude,
                Longitude = Longitude,
                Period = new Interval { StartTime = now, EndTime = now.AddHours(6) },
                PageSize = 6,
            });

            Assert.That(response.HourlyForecasts, Is.Not.Null.And.Not.Empty);
            Assert.That(response.HourlyForecasts![0].Indexes, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task History_ValidLocation_ReturnsHours()
        {
            var response = await Maps.AirQualityHistory.QueryAsync(new HistoryRequest
            {
                ApiKey = ApiKey,
                Latitude = Latitude,
                Longitude = Longitude,
                Hours = 6,
            });

            Assert.That(response.HoursInfo, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task HeatmapTile_ValidTile_DownloadsPngBytes()
        {
            var response = await Maps.AirQualityHeatmapTile.QueryAsync(new HeatmapTileRequest
            {
                ApiKey = ApiKey,
                MapType = AirQualityMapType.UsAqi,
                Zoom = 4,
                X = 4,
                Y = 6,
            });

            Assert.That(response.Content, Is.Not.Null.And.Not.Empty);
        }
    }
}
