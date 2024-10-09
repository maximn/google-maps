using GoogleMapsApi.Entities.PlaceAutocomplete.Request;
using GoogleMapsApi.Entities.PlaceAutocomplete.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    class PlaceAutocompleteTests : BaseTestIntegration
    {
        [Test]
        public async Task ReturnsNoResults()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "zxqtrb",
                Location = new Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.ZERO_RESULTS));
        }

        [Test]
        public async Task OffsetTest()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abbeyjibberish",
                Location = new Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.ZERO_RESULTS), "results for jibberish");

            var offsetRequest = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abbeyjibberish",
                Offset = 5,
                Location = new Entities.Common.Location(53.4635332, -2.2419169)
            };

            PlaceAutocompleteResponse offsetResult = await GoogleMaps.PlaceAutocomplete.QueryAsync(offsetRequest);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(offsetResult.Status, Is.EqualTo(Status.OK), "results using offset");
        }

        [Test]
        public async Task TypeTest()
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = "abb",
                Type = "geocode",
                Location = new Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));

            foreach (var oneResult in result.Results)
            {
                Assert.That(oneResult.Types, Is.Not.Null, "result with no type classification");
                Assert.That(new List<string>(oneResult.Types).Contains("geocode"), "non-geocode result");
            }
        }


        [TestCase("oakfield road, chea", "CHEADLE")]
        [TestCase("128 abbey r", "MACCLESFIELD")]
        public async Task CheckForExpectedRoad( string aSearch, string anExpected)
        {
            var request = new PlaceAutocompleteRequest
            {
                ApiKey = base.ApiKey,
                Input = aSearch,
                Location = new GoogleMapsApi.Entities.Common.Location(53.4635332, -2.2419169),
                Radius = 30000
            };

            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.Not.EqualTo(Status.ZERO_RESULTS));

            Assert.That(result.Results.Any(t => t.Description.ToUpper().Contains(anExpected)));
        }

        [Test(Description = "Ensures that it is ok to sent 0 as a radius value")]
        public async Task CheckZeroRadius() 
        {
            var request = CreatePlaceAutocompleteRequest("RIX", 0);
            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);
            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.Not.EqualTo(Status.ZERO_RESULTS));
        }
        [Test(Description = "Ensures that it is ok to sent negative value as a radius")]
        public async Task CheckNegativeRadius() 
        {
            var request = CreatePlaceAutocompleteRequest("RIX", -1);
            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);
            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.Not.EqualTo(Status.ZERO_RESULTS));
        }

        [Test(Description = "Ensures that it is ok to sent huge value as a radius")]
        public async Task CheckLargerThenEarthRadius() 
        {
            var request = CreatePlaceAutocompleteRequest("RIX", 30000000);
            PlaceAutocompleteResponse result = await GoogleMaps.PlaceAutocomplete.QueryAsync(request);
            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.Not.EqualTo(Status.ZERO_RESULTS));
        }

        private PlaceAutocompleteRequest CreatePlaceAutocompleteRequest(string query, double? radius) 
        {
            return new PlaceAutocompleteRequest 
            {
                ApiKey = base.ApiKey,
                Input = query,
                Location = new Entities.Common.Location(0, 0),
                Radius = radius
            };
        }
    }
}
