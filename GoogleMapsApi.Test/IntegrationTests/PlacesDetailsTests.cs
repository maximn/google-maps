using System;
using System.Linq;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class PlacesDetailsTests  : BaseTestIntegration
    {
        [Test]
        public void ReturnsPhotos()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = "ChIJZ3VuVMQdLz4REP9PWpQ4SIY"
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request).Result;

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsNotEmpty(result.Result.Photos);
        }

        [Test]
        public void ReturnsNotFoundForWrongReferenceString()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = base.ApiKey,
                // Needs to be a correct looking reference. 1 character too short or long and google will return INVALID_REQUEST instead.
                PlaceId = "ChIJbWWgrQAVkFQReAwrXXWzlYs"
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request).Result;

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Status.NOT_FOUND, result.Status);
        }

        PriceLevel[] anyPriceLevel = new PriceLevel[] { PriceLevel.Free, PriceLevel.Inexpensive, PriceLevel.Moderate, PriceLevel.Expensive, PriceLevel.VeryExpensive };

        [Test]
        public void ReturnsStronglyTypedPriceLevel()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = GetMyPlaceId(),
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request).Result;

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Status.OK, result.Status);

            Assert.That(new PriceLevel[] { result.Result.PriceLevel.Value }, Is.SubsetOf(anyPriceLevel));
        }

        [Test]
        public void ReturnsOpeningTimes()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = GetMyPlaceId(),
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request).Result;

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Status.OK, result.Status);
            
            // commented out because seems like google doesn't have opening hours for this place anymore
            /*
            Assert.AreEqual(7, result.Result.OpeningHours.Periods.Count());
            var sundayPeriod = result.Result.OpeningHours.Periods.First();
            Assert.That(sundayPeriod.OpenTime.Day, Is.EqualTo(DayOfWeek.Sunday));
            Assert.That(sundayPeriod.OpenTime.Time, Is.GreaterThanOrEqualTo(0));
            Assert.That(sundayPeriod.OpenTime.Time, Is.LessThanOrEqualTo(2359));
            Assert.That(sundayPeriod.CloseTime.Time, Is.GreaterThanOrEqualTo(0));
            Assert.That(sundayPeriod.CloseTime.Time, Is.LessThanOrEqualTo(2359));
             */
        }

        private string cachedMyPlaceId;
        private string GetMyPlaceId()
        {
            if (cachedMyPlaceId == null)
            {
                var request = new Entities.Places.Request.PlacesRequest()
                {
                    ApiKey = ApiKey,
                    Name = "My Place Bar & Restaurant",
                    Location = new Location(-31.954453, 115.862717),
                    RankBy = Entities.Places.Request.RankBy.Distance,
                };
                var result = GoogleMaps.Places.Query(request).Result;
                if (result.Status == Entities.Places.Response.Status.OVER_QUERY_LIMIT)
                    Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
                cachedMyPlaceId = result.Results.First().PlaceId;
            }
            return cachedMyPlaceId;
        }
    }
}
