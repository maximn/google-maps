using System;
using System.Linq;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using NUnit.Framework;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.Entities.Places.Response;
using System.Threading;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class PlacesSearchTests
    {
        private readonly string apiKey = TestConfigurations.ApiKey;

        [Test]
        public void ReturnsNearbySearchRequest()
        {
            if (apiKey == "") Assert.Inconclusive("API key not specified");
            var request = new PlacesRequest
            {
                ApiKey = apiKey,
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
            var correctPizzaPlace = result.Results.Where(t => t.Name.Contains("Pagliacci"));
            Assert.IsTrue(correctPizzaPlace != null && correctPizzaPlace.Count() > 0);
        }

        [Test]
        public void TestNearbySearchPagination()
        {
            if (apiKey == "") Assert.Inconclusive("API key not specified");
            var request = new PlacesRequest
            {
                ApiKey = apiKey,
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
                ApiKey = apiKey,
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