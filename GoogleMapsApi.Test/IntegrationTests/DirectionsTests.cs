﻿using System;
using System.Collections.Generic;
using System.Linq;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using NUnit.Framework;
using GoogleMapsApi.Test.Utils;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class DirectionsTests : BaseTestIntegration
    {
        [Test]
        public async Task Directions_SumOfStepDistancesCorrect()
        {
            var request = new DirectionsRequest { Origin = "285 Bedford Ave, Brooklyn, NY, USA", Destination = "185 Broadway Ave, Manhattan, NY, USA" };
            request.ApiKey = ApiKey;
            var result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(DirectionsStatusCodes.OK), result.ErrorMessage);
            Assert.That(result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), Is.GreaterThan(100));
        }

		[Test]
		public async Task Directions_ErrorMessage()
		{
			var request = new DirectionsRequest
			{
				ApiKey = "ABCDEF", // Wrong API Key
				Origin = "285 Bedford Ave, Brooklyn, NY, USA",
				Destination = "185 Broadway Ave, Manhattan, NY, USA"
			};
			var result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(DirectionsStatusCodes.REQUEST_DENIED));
			Assert.That(result.ErrorMessage, Is.Not.Null.And.Not.Empty);
		}

        [Test]
        public async Task Directions_WithWayPoints()
        {
            var request = new DirectionsRequest { Origin = "NYC, USA", Destination = "Miami, USA", Waypoints = new string[] { "Philadelphia, USA" }, OptimizeWaypoints = true, ApiKey = ApiKey };
            var result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(DirectionsStatusCodes.OK), result.ErrorMessage);
            Assert.That(result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), Is.EqualTo(156097).Within(10 * 1000));

            Assert.That(result.Routes.First().Legs.First().EndAddress, Does.Contain("Philadelphia"));
        }

        [Test]
        public async Task Directions_ExceedingRouteLength()
        {
            var request = new DirectionsRequest
            {
                Origin = "NYC, USA", Destination = "Miami, USA", Waypoints = new string[]
                {
                    "Seattle, USA",
                    "Dallas, USA",
                    "Naginey, USA",
                    "Edmonton, Canada",
                    "Seattle, USA",
                    "Dallas, USA",
                    "Naginey, USA",
                    "Edmonton, Canada",
                    "Seattle, USA",
                    "Dallas, USA",
                    "Naginey, USA",
                    "Edmonton, Canada"
                },
                ApiKey = ApiKey
            };
            var result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(DirectionsStatusCodes.MAX_ROUTE_LENGTH_EXCEEDED), result.ErrorMessage);
        }

        [Test]
        public async Task Directions_Correct_OverviewPath()
        {
            var request = new DirectionsRequest
            {
                Destination = "maleva 10, Ahtme, Kohtla-Järve, 31025 Ida-Viru County, Estonia",
                Origin = "veski 2, Jõhvi Parish, 41532 Ida-Viru County, Estonia",
                ApiKey = ApiKey
            };

            DirectionsResponse result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);

            OverviewPolyline overviewPath = result.Routes.First().OverviewPath;
            OverviewPolyline polyline = result.Routes.First().Legs.First().Steps.First().PolyLine;

            Assert.That(result.Status, Is.EqualTo(DirectionsStatusCodes.OK), result.ErrorMessage);
            Assert.That(overviewPath.Points.Count(), Is.EqualTo(122).Within(30));
            Assert.That(polyline.Points.Count(), Is.GreaterThan(1));
        }

        [Test]
        public void DirectionsAsync_SumOfStepDistancesCorrect()
        {
            var request = new DirectionsRequest { Origin = "285 Bedford Ave, Brooklyn, NY, USA", Destination = "185 Broadway Ave, Manhattan, NY, USA", ApiKey = ApiKey };

            var result = GoogleMaps.Directions.QueryAsync(request).Result;

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Status, Is.EqualTo(DirectionsStatusCodes.OK));
            Assert.That(result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), Is.GreaterThan(100));
        }

        //The sub_steps differes between google docs documentation and implementation. We use it as google implemented, so we have test to make sure it's not broken.
        [Test]
        public async Task Directions_VerifysubSteps()
        {
            var request = new DirectionsRequest
            {
                Origin = "75 9th Ave, New York, NY",
                Destination = "MetLife Stadium Dr East Rutherford, NJ 07073",
                TravelMode = TravelMode.Driving,
                ApiKey = ApiKey
            };

            DirectionsResponse result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);

            var route = result.Routes.First();
            var leg = route.Legs.First();
            var step = leg.Steps.First();

            Assert.That(step, Is.Not.Null);
        }

        [Test]
        public async Task Directions_VerifyBounds()
        {
            var request = new DirectionsRequest
            {
                Origin = "Genk, Belgium",
                Destination = "Brussels, Belgium",
                TravelMode = TravelMode.Driving,
                ApiKey = ApiKey
            };

            DirectionsResponse result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);

            var route = result.Routes.First();

            Assert.That(route, Is.Not.Null);
            Assert.That(route.Bounds, Is.Not.Null);
            Assert.That(route.Bounds.NorthEast.Latitude, Is.GreaterThan(50));
            Assert.That(route.Bounds.NorthEast.Longitude, Is.GreaterThan(3));
            Assert.That(route.Bounds.SouthWest.Latitude, Is.GreaterThan(50));
            Assert.That(route.Bounds.SouthWest.Longitude, Is.GreaterThan(3));
            Assert.That(route.Bounds.Center.Latitude, Is.GreaterThan(50));
            Assert.That(route.Bounds.Center.Longitude, Is.GreaterThan(3));
        }

        [Test]
        public async Task Directions_WithIcons()
        {
            var dep_time = DateTime.Today
                            .AddDays(1)
                            .AddHours(13);
            
            var request = new DirectionsRequest
            {
                Origin = "T-centralen, Stockholm, Sverige",
                Destination = "Kungsträdgården, Stockholm, Sverige",
                TravelMode = TravelMode.Transit,
                DepartureTime = dep_time,
                Language = "sv",
                ApiKey = ApiKey

            };

            DirectionsResponse result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);

            var route = result.Routes.First();
            var leg = route.Legs.First();
            var steps = leg.Steps;

            Assert.That(steps.Where(s =>
                s.TransitDetails?
                .Lines?
                .Vehicle?
                .Icon != null), Is.Not.Empty);
        }

        [Test]
        public async Task Directions_WithRegionSearch()
        {
            var dep_time = DateTime.Today
                            .AddDays(1)
                            .AddHours(13);

            var request = new DirectionsRequest
            {
                Origin = "Mt Albert",
                Destination = "Parnell",
                TravelMode = TravelMode.Transit,
                DepartureTime = dep_time,
                Region = "nz",
                ApiKey = ApiKey
            };

            DirectionsResponse result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.That(result.Routes, Is.Not.Empty);
            Assert.That(result.Status.Equals(DirectionsStatusCodes.OK), Is.True);
        }

        [Test]
        public async Task Directions_CanGetDurationWithTraffic()
        {
            var request = new DirectionsRequest
            {
                Origin = "285 Bedford Ave, Brooklyn, NY, USA",
                Destination = "185 Broadway Ave, Manhattan, NY, USA",
                DepartureTime = DateTime.Now.Date.AddDays(1).AddHours(8),
                ApiKey = ApiKey //Duration in traffic requires an API key
            };
            var result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);

            //All legs have duration
            Assert.That(result.Routes.First().Legs.All(l => l.DurationInTraffic != null), Is.True);

            //Duration with traffic is usually longer but is not guaranteed
            Assert.That(result.Routes.First().Legs.Sum(s => s.Duration.Value.TotalSeconds), Is.Not.EqualTo(result.Routes.First().Legs.Sum(s => s.DurationInTraffic.Value.TotalSeconds)));
        }

        [Test]
        public void Directions_CanGetLongDistanceTrain()
        {
            var request = new DirectionsRequest
            {
                Origin = "zurich airport",
                Destination = "brig",
                TravelMode = TravelMode.Transit,
                DepartureTime = new DateTime(2018, 08, 18, 15, 30, 00)
            };

            Assert.DoesNotThrowAsync(async () => await GoogleMaps.Directions.QueryAsync(request));
        }
    }
}
