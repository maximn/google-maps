using System.Threading.Tasks;
using GoogleMapsApi.Entities.Solar.Request;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;

namespace GoogleMapsApi.Test.IntegrationTests
{
    /// <summary>
    /// Live Solar API tests. The Solar API is billable, so these are skipped by default; set
    /// <c>RUN_BILLABLE_TESTS=true</c> (and use a key with the Solar API enabled) to run them.
    /// </summary>
    [TestFixture]
    [BillableTest]
    public class SolarTests : BaseTestIntegration
    {
        // Mountain View, CA — well covered by Solar API imagery.
        private const double Latitude = 37.4450;
        private const double Longitude = -122.1390;

        [Test]
        public async Task BuildingInsights_ValidLocation_ReturnsSolarData()
        {
            var request = new BuildingInsightsRequest
            {
                ApiKey = ApiKey,
                Latitude = Latitude,
                Longitude = Longitude,
            };

            var response = await Maps.SolarBuildingInsights.QueryAsync(request);

            Assert.That(response.Name, Is.Not.Null.And.Not.Empty);
            Assert.That(response.Center, Is.Not.Null);
            Assert.That(response.SolarPotential, Is.Not.Null);
            Assert.That(response.SolarPotential!.MaxArrayPanelsCount, Is.GreaterThan(0));
        }

        [Test]
        public async Task DataLayers_ValidLocation_ReturnsLayerUrls()
        {
            var request = new DataLayersRequest
            {
                ApiKey = ApiKey,
                Latitude = Latitude,
                Longitude = Longitude,
                RadiusMeters = 50,
            };

            var response = await Maps.SolarDataLayers.QueryAsync(request);

            Assert.That(response.DsmUrl, Is.Not.Null.And.Not.Empty);
            Assert.That(response.AnnualFluxUrl, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task GeoTiff_FromDataLayerUrl_DownloadsBytes()
        {
            var layers = await Maps.SolarDataLayers.QueryAsync(new DataLayersRequest
            {
                ApiKey = ApiKey,
                Latitude = Latitude,
                Longitude = Longitude,
                RadiusMeters = 50,
            });

            var response = await Maps.SolarGeoTiff.QueryAsync(new GeoTiffRequest
            {
                ApiKey = ApiKey,
                Url = layers.DsmUrl!,
            });

            Assert.That(response.Content, Is.Not.Null.And.Not.Empty);
        }
    }
}
