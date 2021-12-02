using System.Linq;
using GoogleMapsApi.Entities.PlacesText.Request;
using GoogleMapsApi.Entities.PlacesText.Response;
using NUnit.Framework;
using GoogleMapsApi.Test.Utils;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class PlacesTextTests : BaseTestIntegration
    {
        [Test]
        public async Task ReturnsFormattedAddress()
        {
            var request = new PlacesTextRequest
            {
                ApiKey = ApiKey,
                Query = "1 smith st parramatta",
                Types = "address"
            };

            PlacesTextResponse result = await GoogleMaps.PlacesText.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.AreEqual("1 Smith St, Parramatta NSW 2150, Australia", result.Results.First().FormattedAddress);
        }

        [Test]
        public async Task ReturnsPhotos()
        {
            var request = new PlacesTextRequest
            {
                ApiKey = ApiKey,
                Query = "1600 Pennsylvania Ave NW",
                Types = "address"
            };

            PlacesTextResponse result = await GoogleMaps.PlacesText.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.That(result.Results, Is.Not.Null.And.Not.Empty);
        }
    }
}
