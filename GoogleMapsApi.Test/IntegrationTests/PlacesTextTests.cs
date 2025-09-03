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
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            Assert.That(result.Results.First().FormattedAddress, Is.EqualTo("1 Smith St, Parramatta NSW 2150, Australia"));
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
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            Assert.That(result.Results, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Should_Support_Pagination_With_NextPageToken()
        {
            var request = new PlacesTextRequest
            {
                ApiKey = ApiKey,
                Query = "restaurants in New York City"
            };

            PlacesTextResponse firstResponse = await GoogleMaps.PlacesText.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(firstResponse);
            Assert.That(firstResponse.Status, Is.EqualTo(Status.OK));
            Assert.That(firstResponse.Results, Is.Not.Null.And.Not.Empty);
            
            if (string.IsNullOrWhiteSpace(firstResponse.NextPage))
            {
                Assert.Inconclusive("This query did not return a next_page_token, cannot test pagination. Try a broader query that returns more results.");
                return;
            }

            await Task.Delay(2000);

            var secondRequest = new PlacesTextRequest
            {
                ApiKey = ApiKey,
                Query = "restaurants in New York City",
                PageToken = firstResponse.NextPage
            };

            PlacesTextResponse secondResponse = await GoogleMaps.PlacesText.QueryAsync(secondRequest);

            AssertInconclusive.NotExceedQuota(secondResponse);
            Assert.That(secondResponse.Status, Is.EqualTo(Status.OK));
            Assert.That(secondResponse.Results, Is.Not.Null.And.Not.Empty);
            
            var firstResultsIds = firstResponse.Results.Select(r => r.PlaceId).ToArray();
            var secondResultsIds = secondResponse.Results.Select(r => r.PlaceId).ToArray();
            
            Assert.That(firstResultsIds.Intersect(secondResultsIds).Count(), Is.EqualTo(0), 
                "First and second page should contain different results");
        }
    }
}
