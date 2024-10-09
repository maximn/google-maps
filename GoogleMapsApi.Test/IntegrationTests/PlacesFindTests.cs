using GoogleMapsApi.Entities.PlacesFind.Request;
using GoogleMapsApi.Entities.PlacesFind.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class PlacesFindTests : BaseTestIntegration
    {
        [Test]
        public async Task ReturnsResults()
        {
            var request = new PlacesFindRequest
            {
                ApiKey = ApiKey,
                Input = "pizza chicago il",
                InputType = InputType.TextQuery
            };

            PlacesFindResponse result = await GoogleMaps.PlacesFind.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            Assert.That(result.Candidates, Is.Not.Empty);
        }

        [Test]
        public async Task DoesNotReturnFieldsWhenNotRequested()
        {
            var request = new PlacesFindRequest
            {
                ApiKey = ApiKey,
                Input = "pizza chicago il",
                InputType = InputType.TextQuery,
                Fields = "place_id"
            };

            PlacesFindResponse result = await GoogleMaps.PlacesFind.QueryAsync(request);

            //FormattedAddress should be null since it wasn't requested
            Assert.That(result.Candidates, Is.Not.Empty);
            Assert.That(result.Candidates.FirstOrDefault()?.FormattedAddress, Is.Null);
        }
    }
}
