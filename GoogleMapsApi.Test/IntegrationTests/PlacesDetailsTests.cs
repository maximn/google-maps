using System.Linq;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using NUnit.Framework;
using GoogleMapsApi.Test.Utils;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class PlacesDetailsTests  : BaseTestIntegration
    {
        [Test]
        public async Task ReturnsPhotos()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = "ChIJZ3VuVMQdLz4REP9PWpQ4SIY"
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            Assert.That(result.Result.Photos, Is.Not.Empty);
        }

        [Test]
        public async Task ReturnsNotFoundForWrongReferenceString()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = base.ApiKey,
                // Needs to be a correct looking reference. 1 character too short or long and google will return INVALID_REQUEST instead.
                PlaceId = "ChIJbWWgrQAVkFQReAwrXXWzlYs"
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(Status.NOT_FOUND, Is.EqualTo(result.Status));
        }

        readonly PriceLevel[] anyPriceLevel = new PriceLevel[] { PriceLevel.Free, PriceLevel.Inexpensive, PriceLevel.Moderate, PriceLevel.Expensive, PriceLevel.VeryExpensive };

        [Test]
        public async Task ReturnsStronglyTypedPriceLevel()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = await GetMyPlaceId(),
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(Status.OK, Is.EqualTo(result.Status));
            Assert.That(new PriceLevel[] { result.Result.PriceLevel.Value }, Is.SubsetOf(anyPriceLevel));
        }

        [Test]
        public async Task ReturnsOpeningTimes()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = await GetMyPlaceId(),
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(Status.OK, Is.EqualTo(result.Status));
            
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
        private async Task<string> GetMyPlaceId()
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
                var result = await GoogleMaps.Places.QueryAsync(request);
                AssertInconclusive.NotExceedQuota(result);
                cachedMyPlaceId = result.Results.First().PlaceId;
            }
            return cachedMyPlaceId;
        }
    }
}
