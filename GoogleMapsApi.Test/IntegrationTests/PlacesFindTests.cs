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
        public void ReturnsPhotos()
        {
            var request = new PlacesFindRequest
            {
                ApiKey = ApiKey,
                Input = "ChIJL3osJJksDogRodsJu9TjTQA",
                InputType = InputType.TextQuery,
                Fields = "photo"
            };

            PlacesFindResponse result = GoogleMaps.PlacesFind.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsNotEmpty(result.Candidates);
            Assert.IsNotEmpty(result.Candidates.FirstOrDefault()?.Photos);
        }

        [Test]
        public void DoesNotReturnFieldsWhenNotRequested()
        {
            var request = new PlacesFindRequest
            {
                ApiKey = ApiKey,
                Input = "ChIJL3osJJksDogRodsJu9TjTQA",
                InputType = InputType.TextQuery,
                Fields = "place_id"
            };

            PlacesFindResponse result = GoogleMaps.PlacesFind.Query(request);

            //FormattedAddress should be null since it wasn't requested
            Assert.IsNotEmpty(result.Candidates);
            Assert.IsNull(result.Candidates.FirstOrDefault()?.FormattedAddress);
        }
    }
}
