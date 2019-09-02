using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.TimeZone.Request;
using GoogleMapsApi.Entities.TimeZone.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class TimeZoneTests : BaseTestIntegration
    {
        [Test]
        public async Task TimeZone_Correct_OverviewPathAsync()
        {
            TimeZoneRequest request = new TimeZoneRequest
            {
                Location = new Location(55.866413, 12.501063),
                Language = "en",
                ApiKey = ApiKey
            };

            TimeZoneResponse result = await GoogleMaps.TimeZone.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
        }

        [Test]
        [System.Obsolete]
        public void TimeZone_Correct_OverviewPath()
        {
            TimeZoneRequest request = new TimeZoneRequest
            {
                Location = new Location(55.866413, 12.501063),
                Language = "en",
                ApiKey = ApiKey
            };

            TimeZoneResponse result = GoogleMaps.TimeZone.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
        }
    }
}