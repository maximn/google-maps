using System;
using System.Collections.Generic;
using System.Linq;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using NUnit.Framework;
using GoogleMapsApi.Test.Utils;
using System.Threading.Tasks;
using GoogleMapsApi.StaticMaps;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class DirectionsTests : BaseTestIntegration
    {
        [Test]
        public async Task Directions_SumOfStepDistancesCorrect()
        {
            var request = new DirectionsRequest { Origin = "285 Bedford Ave, Brooklyn, NY, USA", Destination = "185 Broadway Ave, Manhattan, NY, USA" };
            request.ApiKey = "AIzaSyDikeBAymgSWrWz-9Y7Danr2mNewZV_MwI";
            var result = await GoogleMaps.Directions.QueryAsync(request);

            //var map = new StaticMapsEngine().GenerateStaticMapURL(request);
            AssertInconclusive.NotExceedQuota(result);

            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status, result.ErrorMessage);
            Assert.Greater(result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), 100);

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
            Assert.AreEqual(DirectionsStatusCodes.REQUEST_DENIED, result.Status);
			Assert.IsNotNull (result.ErrorMessage);
			Assert.IsNotEmpty (result.ErrorMessage);
		}

        [Test]
        public async Task Directions_WithWayPoints()
        {
            var request = new DirectionsRequest { Origin = "NYC, USA", Destination = "Miami, USA", Waypoints = new string[] { "Philadelphia, USA" }, OptimizeWaypoints = true, ApiKey = ApiKey };
            var result = await GoogleMaps.Directions.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status, result.ErrorMessage);
            Assert.AreEqual(156097, result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), 10 * 1000);

            StringAssert.Contains("Philadelphia", result.Routes.First().Legs.First().EndAddress);
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
            Assert.AreEqual(DirectionsStatusCodes.MAX_ROUTE_LENGTH_EXCEEDED, result.Status, result.ErrorMessage);
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

            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status, result.ErrorMessage);
            Assert.AreEqual(122, overviewPath.Points.Count(), 30);
            Assert.Greater(polyline.Points.Count(), 1);
        }

        [Test]
        public void DirectionsAsync_SumOfStepDistancesCorrect()
        {
            var request = new DirectionsRequest { Origin = "285 Bedford Ave, Brooklyn, NY, USA", Destination = "185 Broadway Ave, Manhattan, NY, USA", ApiKey = ApiKey };

            var result = GoogleMaps.Directions.QueryAsync(request).Result;

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status, result.ErrorMessage);
            Assert.Greater(result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), 100);
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

            Assert.NotNull(step);
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

            Assert.NotNull(route);
            Assert.NotNull(route.Bounds);
            Assert.Greater(route.Bounds.NorthEast.Latitude, 50);
            Assert.Greater(route.Bounds.NorthEast.Longitude, 3);
            Assert.Greater(route.Bounds.SouthWest.Latitude, 50);
            Assert.Greater(route.Bounds.SouthWest.Longitude, 3);
            Assert.Greater(route.Bounds.Center.Latitude, 50);
            Assert.Greater(route.Bounds.Center.Longitude, 3);
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

            Assert.IsNotEmpty(steps.Where(s =>
                s.TransitDetails?
                .Lines?
                .Vehicle?
                .Icon != null));
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
            Assert.IsNotEmpty(result.Routes);
            Assert.True(result.Status.Equals(DirectionsStatusCodes.OK));
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
            Assert.IsTrue(result.Routes.First().Legs.All(l => l.DurationInTraffic != null));

            //Duration with traffic is usually longer but is not guaranteed
            Assert.AreNotEqual(result.Routes.First().Legs.Sum(s => s.Duration.Value.TotalSeconds), result.Routes.First().Legs.Sum(s => s.DurationInTraffic.Value.TotalSeconds));
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
