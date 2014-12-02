using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi.Entities.PlaceAutocomplete.Response;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    class PlaceAutocompleteTests : BaseTestIntegration
    {
        [Test]
        public void ReturnsExpectedRoads()
        {
            if (ApiKey == "") Assert.Inconclusive("API key not specified");

            CheckForExpectedRoad("oakfield road, chea", "CHEADLE");
            CheckForExpectedRoad("128 abbey r", "MACCLESFIELD");
            CheckForExpectedRoad("SK10 3PA", "MACCLESFIELD");
        }

        [Test]
        public void ReturnsNoResults()
        {
            if (ApiKey == "") Assert.Inconclusive("API key not specified");

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
            if (ApiKey == "") Assert.Inconclusive("API key not specified");

            // Nothing should match the jibberish input
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

            // Restrict input to first five characters - this should return results
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
            Assert.AreNotEqual(Status.ZERO_RESULTS, offsetResult.Status, "results using offset");
        }

        [Test]
        public void TypeTest()
        {
            if (ApiKey == "") Assert.Inconclusive("API key not specified");

            // Restrict results to geocode (exclude establishments etc)
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abb",
                Types = "geocode",
                Location = new GoogleMapsApi.Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = GoogleMaps.PlaceAutocomplete.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");

            // Should be plenty of matches for a good request
            Assert.AreEqual(Status.OK, result.Status);

            // Verify that results were filtered by type
            foreach (var oneResult in result.Results)
            {
                Assert.IsNotNull(oneResult.Types, "result with no type classification");
                Assert.IsTrue(new List<string>(oneResult.Types).Contains("geocode"), "non-geocode result");
            }
        }

        private void CheckForExpectedRoad( string aSearch, string aExpected )
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
            foreach (var oneResult in result.Results)
            {
                if (oneResult.Description.ToUpper().Contains(aExpected))
                {
                    // Found the expected result
                    return;
                }
            }

            Assert.Fail("Expected street not found for <" + aSearch + ">");
        }
    }
}
