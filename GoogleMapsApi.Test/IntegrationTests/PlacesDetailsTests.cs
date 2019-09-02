using System.Linq;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.PlacesDetails.Request;
using GoogleMapsApi.Entities.PlacesDetails.Response;
using NUnit.Framework;
using GoogleMapsApi.Test.Utils;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class PlacesDetailsTests  : BaseTestIntegration
    {
        [Test]
        public async Task ReturnsPhotosAsync()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = "ChIJZ3VuVMQdLz4REP9PWpQ4SIY"
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsNotEmpty(result.Result.Photos);
        }

        [Test]
        [System.Obsolete]
        public void ReturnsPhotos()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = "ChIJZ3VuVMQdLz4REP9PWpQ4SIY"
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.IsNotEmpty(result.Result.Photos);
        }

        [Test]
        public async Task ReturnsNotFoundForWrongReferenceStringAsync()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = base.ApiKey,
                // Needs to be a correct looking reference. 1 character too short or long and google will return INVALID_REQUEST instead.
                PlaceId = "ChIJbWWgrQAVkFQReAwrXXWzlYs"
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.NOT_FOUND, result.Status);
        }

        [Test]
        [System.Obsolete]
        public void ReturnsNotFoundForWrongReferenceString()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = base.ApiKey,
                // Needs to be a correct looking reference. 1 character too short or long and google will return INVALID_REQUEST instead.
                PlaceId = "ChIJbWWgrQAVkFQReAwrXXWzlYs"
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.NOT_FOUND, result.Status);
        }

        readonly PriceLevel[] _anyPriceLevel = new PriceLevel[] { PriceLevel.Free, PriceLevel.Inexpensive, PriceLevel.Moderate, PriceLevel.Expensive, PriceLevel.VeryExpensive };

        [Test]
        public async Task ReturnsStronglyTypedPriceLevelAsync()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = await GetMyPlaceIdAsync()
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.NotNull(result.Result.PriceLevel);
            Assert.That(new PriceLevel[] {result.Result.PriceLevel.Value}, Is.SubsetOf(_anyPriceLevel));
        }

        [Test]
        [System.Obsolete]
        public void ReturnsStronglyTypedPriceLevel()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = GetMyPlaceId(),
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            Assert.NotNull(result.Result.PriceLevel);
            Assert.That(new PriceLevel[] { result.Result.PriceLevel.Value }, Is.SubsetOf(_anyPriceLevel));
        }

        [Test]
        public async Task ReturnsOpeningTimesAsync()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = await GetMyPlaceIdAsync()
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
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

        [Test]
        [System.Obsolete]
        public void ReturnsOpeningTimes()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = GetMyPlaceId(),
            };

            PlacesDetailsResponse result = GoogleMaps.PlacesDetails.Query(request);

            AssertInconclusive.NotExceedQuota(result);
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

        private string _cachedMyPlaceId;
        private async Task<string> GetMyPlaceIdAsync()
        {
            if (_cachedMyPlaceId == null)
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
                _cachedMyPlaceId = result.Results.First().PlaceId;
            }
            return _cachedMyPlaceId;
        }

        [System.Obsolete]
        private string GetMyPlaceId()
        {
            if (_cachedMyPlaceId == null)
            {
                var request = new Entities.Places.Request.PlacesRequest()
                {
                    ApiKey = ApiKey,
                    Name = "My Place Bar & Restaurant",
                    Location = new Location(-31.954453, 115.862717),
                    RankBy = Entities.Places.Request.RankBy.Distance,
                };
                var result = GoogleMaps.Places.Query(request);
                AssertInconclusive.NotExceedQuota(result);
                _cachedMyPlaceId = result.Results.First().PlaceId;
            }
            return _cachedMyPlaceId;
        }
    }
}
