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
    [BillableTest]
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
            Assert.That(result.Status, Is.EqualTo(Status.NOT_FOUND));
        }

        static readonly PriceLevel[] anyPriceLevel = { PriceLevel.Free, PriceLevel.Inexpensive, PriceLevel.Moderate, PriceLevel.Expensive, PriceLevel.VeryExpensive };

        // Single Details fetch covers what used to be two separate tests (price level + opening hours)
        // to keep Places API call volume within free-tier limits.
        [Test]
        public async Task ReturnsStronglyTypedPriceLevelAndOpeningHours()
        {
            var request = new PlacesDetailsRequest
            {
                ApiKey = ApiKey,
                PlaceId = await GetMyPlaceId(),
            };

            PlacesDetailsResponse result = await GoogleMaps.PlacesDetails.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));

            Assert.That(result.Result.PriceLevel, Is.Not.Null);
            Assert.That(new[] { result.Result.PriceLevel!.Value }, Is.SubsetOf(anyPriceLevel));

            // Opening-hours shape: Google sometimes omits hours for this place, so only assert
            // structural validity when present.
            if (result.Result.OpeningHours?.Periods != null)
            {
                foreach (var period in result.Result.OpeningHours.Periods)
                {
                    Assert.That(int.Parse(period.OpenTime.Time), Is.InRange(0, 2359));
                    if (period.CloseTime != null)
                        Assert.That(int.Parse(period.CloseTime.Time), Is.InRange(0, 2359));
                }
            }
        }

        private string? cachedMyPlaceId;
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
                Assert.That(result.Results, Is.Not.Null.And.Not.Empty, "Results should not be null or empty");
                cachedMyPlaceId = result.Results!.First().PlaceId;
            }
            return cachedMyPlaceId;
        }
    }
}
