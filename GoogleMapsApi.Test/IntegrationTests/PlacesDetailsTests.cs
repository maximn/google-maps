using System;
using System.Configuration;
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
        public void ReturnsNotFoundForWrongReferenceString()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = base.ApiKey,
                // Needs to be a correct looking reference. 1 character too short or long and google will return INVALID_REQUEST instead.
                Reference = "CnRqAAAAvs_8564VF4xq2St_9P-YaCYEep2qa86WfWBcBL6q-264bgWE3vWD1zI5kIcWVOA6r9XA2vOfOKZ3uEMs_FQNQZGpTGxyaaq5aTF8XJD36ZcYMbmPuTP00jVEXBPlEmnUxUuHHbxzDd_7fZwxABkPIhIQ4IypqCmBf4WOCXSnT9jiIRoUi8iVFfW6-txsNpGCFurUqA-qHos"
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Status.NOT_FOUND, result.Status);
        }

        [Test]
        public void ReturnsStronglyTypedPriceLevel()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                Reference = GetMyPlaceReference(),
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request);

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Status.OK, result.Status);
            Assert.AreEqual(PriceLevel.Moderate, result.Result.PriceLevel);
        }

        [Test]
        public void ReturnsOpeningTimes()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                Reference = GetMyPlaceReference(),
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request);

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

        private string cachedMyPlaceReference;
        private string GetMyPlaceReference()
        {
            if (cachedMyPlaceReference == null)
            {
                var referenceRequest = new Entities.Places.Request.PlacesRequest()
                {
                    ApiKey = ApiKey,
                    Name = "My Place Bar & Restaurant",
                    Location = new Location(-31.954453, 115.862717),
                    RankBy = Entities.Places.Request.RankBy.Distance,
                };
                var referenceResult = GoogleMaps.Places.Query(referenceRequest);
                if (referenceResult.Status == Entities.Places.Response.Status.OVER_QUERY_LIMIT)
                    Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
                cachedMyPlaceReference = referenceResult.Results.First().Reference;
            }
            return cachedMyPlaceReference;
        }
    }
}