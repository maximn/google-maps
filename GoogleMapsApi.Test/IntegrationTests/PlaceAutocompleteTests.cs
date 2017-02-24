using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi.Entities.PlaceAutocomplete.Response;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    class PlaceAutocompleteTests : BaseTestIntegration
    {
        [Test]
        public void ReturnsNoResults()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "zxqtrb",
                Location = new GoogleMapsApi.Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Status.ZERO_RESULTS, result.Status);
        }

        [Test]
        public void OffsetTest()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abbeyjibberish",
                Location = new GoogleMapsApi.Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Status.ZERO_RESULTS, result.Status, "results for jibberish");

            var offsetRequest = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abbeyjibberish",
                Offset = 5,
                Location = new GoogleMapsApi.Entities.Common.Location(53.4635332, -2.2419169)
            };

            PlaceAutocompleteResponse offsetResult = GoogleMaps.PlaceAutocomplete.Query(offsetRequest);

            if (offsetResult.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Status.OK, offsetResult.Status, "results using offset");
        }

        [Test]
        public void TypeTest()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abb",
                Type = "geocode",
                Location = new GoogleMapsApi.Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");

            Assert.AreEqual(Status.OK, result.Status);

            foreach (var oneResult in result.Results)
            {
                Assert.IsNotNull(oneResult.Types, "result with no type classification");
                Assert.IsTrue(new List<string>(oneResult.Types).Contains("geocode"), "non-geocode result");
            }
        }


        [TestCase("oakfield road, chea", "CHEADLE")]
        [TestCase("128 abbey r", "MACCLESFIELD")]
        public void CheckForExpectedRoad( string aSearch, string anExpected)
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = aSearch,
                Location = new GoogleMapsApi.Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreNotEqual(Status.ZERO_RESULTS, result.Status);

            Assert.That(result.Results.Any(t => t.Description.ToUpper().Contains(anExpected)));
        }

        [Test(Description = "Ensures that it is ok to sent 0 as a radius value")]
        public void CheckZeroRadius() 
        {
            var request = CreatePlaceAutocompleteRequest("RIX", 0);
            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);
            AssertRequestsLimit(result);
            Assert.AreNotEqual(Status.ZERO_RESULTS, result.Status);
        }
        [Test(Description = "Ensures that it is ok to sent negative value as a radius")]
        public void CheckNegativeRadius() 
        {
            var request = CreatePlaceAutocompleteRequest("RIX", -1);
            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);
            AssertRequestsLimit(result);
            Assert.AreNotEqual(Status.ZERO_RESULTS, result.Status);
        }

        [Test(Description = "Ensures that it is ok to sent huge value as a radius")]
        public void CheckLargerThenEarthRadius() 
        {
            var request = CreatePlaceAutocompleteRequest("RIX", 30000000);
            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);
            AssertRequestsLimit(result);
            Assert.AreNotEqual(Status.ZERO_RESULTS, result.Status);
        }

        private void AssertRequestsLimit(PlaceAutocompleteResponse result) 
        {
            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
        }

        private PlaceAutocompleteRequest CreatePlaceAutocompleteRequest(string query, double? radius) 
        {
            return new PlaceAutocompleteRequest 
            {
                ApiKey = base.ApiKey,
                Input = query,
                Location = new GoogleMapsApi.Entities.Common.Location(0, 0),
                Radius = radius
            };
        }

    }
}
