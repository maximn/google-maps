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
        public void ReturnsPlaceId()
        {
            var request = new PlacesFindRequest
            {
                ApiKey = ApiKey,
                Input = "hilton chicago",
                InputType = "textquery"
            };

            PlacesFindResponse result = GoogleMaps.PlacesFind.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.AreEqual("ChIJL3osJJksDogRodsJu9TjTQA", result.Candidates.FirstOrDefault()?.PlaceId);
        }
    }
}
