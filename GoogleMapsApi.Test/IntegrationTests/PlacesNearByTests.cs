using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesNearBy.Request;
using GoogleMapsApi.Entities.PlacesNearBy.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class PlacesNearByTests : BaseTestIntegration
    {
        [Test]
        public async Task ReturnsNearbySearchRequest()
        {
            var request = new PlacesNearByRequest
            {
                ApiKey = ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
            };

            PlacesNearByResponse result = await GoogleMaps.PlacesNearBy.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            Assert.That(result.Results, Is.Not.Null.And.Not.Empty, "Results should not be null or empty");
            Assert.That(result.Results!.Count(), Is.GreaterThan(5));
        }

        [Test]
        public async Task TestNearbySearchType()
        {
            var request = new PlacesNearByRequest
            {
                ApiKey = ApiKey,
                Radius = 10000,
                Location = new Location(40.6782552, -73.8671761), // New York
                Type = "airport",
            };

            PlacesNearByResponse result = await GoogleMaps.PlacesNearBy.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            Assert.That(result.Results, Is.Not.Null.And.Not.Empty, "Results should not be null or empty");
            Assert.That(result.Results!.Any(), Is.True);
            Assert.That(result.Results!.Any(t => t.Name.Contains("John F. Kennedy")), Is.True);
        }

        [Test]
        public async Task TestNearbySearchPagination()
        {
            var request = new PlacesNearByRequest
            {
                ApiKey = ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
            };

            PlacesNearByResponse result = await GoogleMaps.PlacesNearBy.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            //we should have more than one page of pizza results from the NearBy Search
            Assert.That(!String.IsNullOrEmpty(result.NextPage), Is.True);
            Assert.That(result.Results, Is.Not.Null.And.Not.Empty, "Results should not be null or empty");
            //a full page of results is always 20
            Assert.That(result.Results!.Count(), Is.EqualTo(20));
            var resultFromFirstPage = result.Results!.FirstOrDefault(); //hold onto this

            //get the second page of results. Delay request by 2 seconds
            //Google API requires a short processing window to develop the second page. See Google API docs for more info on delay.
            
            Thread.Sleep(2000);
            request = new PlacesNearByRequest
            {
                ApiKey = ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
                PageToken = result.NextPage
            };
            result = await GoogleMaps.PlacesNearBy.QueryAsync(request);
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            //make sure the second page has some results
            Assert.That(result.Results, Is.Not.Null.And.Not.Empty, "Results should not be null or empty");
            Assert.That(result.Results!.Any(), Is.True);
            //make sure the result from the first page isn't on the second page to confirm we actually got a second page with new results
            Assert.That(resultFromFirstPage, Is.Not.Null);
            Assert.That(result.Results!.Any(t => t.PlaceId == resultFromFirstPage.PlaceId), Is.False);
        }
    }
}
