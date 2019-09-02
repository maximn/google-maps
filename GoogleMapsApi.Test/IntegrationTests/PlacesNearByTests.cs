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
        public async Task ReturnsNearbySearchRequestAsync()
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
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsTrue(result.Results.Count() > 5);
        }

        [Test]
        [Obsolete]
        public void ReturnsNearbySearchRequest()
        {
            var request = new PlacesNearByRequest
            {
                ApiKey = ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
            };

            PlacesNearByResponse result = GoogleMaps.PlacesNearBy.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsTrue(result.Results.Count() > 5);
        }

        [Test]
        public async Task TestNearbySearchTypeAsync()
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
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsTrue(result.Results.Any());
            Assert.IsTrue(result.Results.Any(t => t.Name.Contains("John F. Kennedy")));
        }

        [Test]
        [Obsolete]
        public void TestNearbySearchType()
        {
            var request = new PlacesNearByRequest
            {
                ApiKey = ApiKey,
                Radius = 10000,
                Location = new Location(40.6782552, -73.8671761), // New York
                Type = "airport",
            };

            PlacesNearByResponse result = GoogleMaps.PlacesNearBy.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsTrue(result.Results.Any());
            Assert.IsTrue(result.Results.Any(t => t.Name.Contains("John F. Kennedy")));
        }

        [Test]
        public async Task TestNearbySearchPaginationAsync()
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
            Assert.AreEqual(Status.OK, result.Status);
            //we should have more than one page of pizza results from the NearBy Search
            Assert.IsTrue(!String.IsNullOrEmpty(result.NextPage));
            //a full page of results is always 20
            Assert.IsTrue(result.Results.Count() == 20);
            var resultFromFirstPage = result.Results.FirstOrDefault(); //hold onto this

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
            Assert.AreEqual(GoogleMapsApi.Entities.PlacesNearBy.Response.Status.OK, result.Status);
            //make sure the second page has some results
            Assert.IsTrue(result.Results != null && result.Results.Any());
            //make sure the result from the first page isn't on the second page to confirm we actually got a second page with new results
            Assert.IsFalse(result.Results.Any(t => resultFromFirstPage != null && t.PlaceId == resultFromFirstPage.PlaceId));
        }

        [Test]
        [Obsolete]
        public void TestNearbySearchPagination()
        {
            var request = new PlacesNearByRequest
            {
                ApiKey = ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
            };

            PlacesNearByResponse result = GoogleMaps.PlacesNearBy.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            //we should have more than one page of pizza results from the NearBy Search
            Assert.IsTrue(!String.IsNullOrEmpty(result.NextPage));
            //a full page of results is always 20
            Assert.IsTrue(result.Results.Count() == 20);
            var resultFromFirstPage = result.Results.FirstOrDefault(); //hold onto this

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
            result = GoogleMaps.PlacesNearBy.Query(request);
            Assert.AreEqual(GoogleMapsApi.Entities.PlacesNearBy.Response.Status.OK, result.Status);
            //make sure the second page has some results
            Assert.IsTrue(result.Results != null && result.Results.Any());
            //make sure the result from the first page isn't on the second page to confirm we actually got a second page with new results
            Assert.IsNotNull(resultFromFirstPage);
            Assert.IsFalse(result.Results.Any(t => t.PlaceId == resultFromFirstPage.PlaceId));
        }
    }
}
