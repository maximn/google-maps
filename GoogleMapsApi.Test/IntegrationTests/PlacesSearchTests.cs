﻿using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.Entities.Places.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class PlacesSearchTests : BaseTestIntegration
    {
        [Test]
        public async Task ReturnsNearbySearchRequest()
        {
            var request = new PlacesRequest
            {
                ApiKey = ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
            };

            PlacesResponse result = await GoogleMaps.Places.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            Assert.That(result.Results.Count(), Is.GreaterThan(5));
        }

        [Test]
        [Ignore("Need to fix it")]
        public async Task TestNearbySearchType()
        {
            var request = new PlacesRequest
            {
                ApiKey = ApiKey,
                Radius = 10000,
                Location = new Location(40.6782552, -73.8671761), // New York
                Type = "airport"
            };

            PlacesResponse result = await GoogleMaps.Places.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            Assert.That(result.Results.Any(), Is.True);
            Assert.That(result.Results.Any(t => t.Name.Contains("John F. Kennedy")), Is.True);
        }

        [Test]
        public async Task TestNearbySearchPagination()
        {
            var request = new PlacesRequest
            {
                ApiKey = ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
            };

            PlacesResponse result = await GoogleMaps.Places.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            //we should have more than one page of pizza results from the NearBy Search
            Assert.That(!String.IsNullOrEmpty(result.NextPage), Is.True);
            //a full page of results is always 20
            Assert.That(result.Results.Count(), Is.EqualTo(20));
            var resultFromFirstPage = result.Results.FirstOrDefault(); //hold onto this

            //get the second page of results. Delay request by 2 seconds
            //Google API requires a short processing window to develop the second page. See Google API docs for more info on delay.
            
            Thread.Sleep(2000);
            request = new PlacesRequest
            {
                ApiKey = ApiKey,
                Keyword = "pizza",
                Radius = 10000,
                Location = new Location(47.611162, -122.337644), //Seattle, Washington, USA
                PageToken = result.NextPage
            };
            result = await GoogleMaps.Places.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(Status.OK));
            //make sure the second page has some results
            Assert.That(result.Results != null && result.Results.Any(), Is.True);
            //make sure the result from the first page isn't on the second page to confirm we actually got a second page with new results
            Assert.That(result.Results, Is.Not.Null);
            Assert.That(resultFromFirstPage, Is.Not.Null);
            Assert.That(result.Results.Any(t => t.PlaceId == resultFromFirstPage.PlaceId), Is.False);
        }
    }
}
