using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.Entities.Places.Response;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class PlacesSearchTests : BaseTestIntegration
    {
        [Test]
        public void ReturnsNearbySearchRequest()
        {
            var request = new PlacesRequest
            {
                ApiKey = ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
                Sensor = false,
            };

            PlacesResponse result = GoogleMaps.Places.Query(request);

            if (result.Status == GoogleMapsApi.Entities.Places.Response.Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(GoogleMapsApi.Entities.Places.Response.Status.OK, result.Status);
            Assert.IsTrue(result.Results.Count() > 5);
        }

        [Test]
        public void TestNearbySearchType()
        {
            var request = new PlacesRequest
            {
                ApiKey = ApiKey,
                Radius = 10000,
                Location = new Location(64.6247243, 21.0747553), // Skellefteå, Sweden
                Sensor = false,
                Type = "airport",
            };

            PlacesResponse result = GoogleMaps.Places.Query(request);

            if (result.Status == GoogleMapsApi.Entities.Places.Response.Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(GoogleMapsApi.Entities.Places.Response.Status.OK, result.Status);
            Assert.IsTrue(result.Results.Any());
            var correctAirport = result.Results.Where(t => t.Name.Contains("Skellefteå Airport"));
            Assert.IsTrue(correctAirport != null && correctAirport.Any());
        }

        [Test]
        public void TestNearbySearchPagination()
        {
            var request = new PlacesRequest
            {
                ApiKey = ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
                Sensor = false,
            };

            PlacesResponse result = GoogleMaps.Places.Query(request);

            if (result.Status == GoogleMapsApi.Entities.Places.Response.Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(GoogleMapsApi.Entities.Places.Response.Status.OK, result.Status);
            //we should have more than one page of pizza results from the NearBy Search
            Assert.IsTrue(!String.IsNullOrEmpty(result.NextPage));
            //a full page of results is always 20
            Assert.IsTrue(result.Results.Count() == 20);
            var resultFromFirstPage = result.Results.FirstOrDefault(); //hold onto this

            //get the second page of results. Delay request by 2 seconds
            //Google API requires a short processing window to develop the second page. See Google API docs for more info on delay.
            
            Thread.Sleep(2000);
            request = new PlacesRequest
            {
                ApiKey = ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
                Sensor = false,
                PageToken = result.NextPage
            };
            result = GoogleMaps.Places.Query(request);
            Assert.AreEqual(GoogleMapsApi.Entities.Places.Response.Status.OK, result.Status);
            //make sure the second page has some results
            Assert.IsTrue(result.Results != null && result.Results.Count() > 0);
            //make sure the result from the first page isn't on the second page to confirm we actually got a second page with new results
            Assert.IsFalse(result.Results.Any(t => t.Reference == resultFromFirstPage.Reference));
        }
    }
}
