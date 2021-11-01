﻿using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.TimeZone.Request;
using GoogleMapsApi.Entities.TimeZone.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class TimeZoneTests : BaseTestIntegration
    {
        [Test]
        public async Task TimeZone_Correct_OverviewPath()
        {
            var request = new TimeZoneRequest
            {
                Location = new Location(55.866413, 12.501063),
                Language = "en"
            };

            TimeZoneResponse result = await GoogleMaps.TimeZone.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
        }
    }
}