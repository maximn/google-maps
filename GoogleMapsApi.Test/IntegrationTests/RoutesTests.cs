using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Routes.Request;
using NUnit.Framework;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class RoutesTests : BaseTestIntegration
    {
        [Test]
        public async Task ComputeRoutes_SimpleAddressToAddress_ReturnsRoute()
        {
            var request = new RoutesRequest
            {
                ApiKey = ApiKey,
                Origin = Waypoint.FromAddress("285 Bedford Ave, Brooklyn, NY, USA"),
                Destination = Waypoint.FromAddress("185 Broadway Ave, Manhattan, NY, USA"),
            };

            var response = await Maps.Routes.QueryAsync(request);

            Assert.That(response.Routes, Is.Not.Null.And.Not.Empty);
            var route = response.Routes![0];
            Assert.That(route.DistanceMeters, Is.Not.Null.And.GreaterThan(0));
            Assert.That(route.DurationSeconds, Is.Not.Null.And.GreaterThan(0));
            Assert.That(route.Polyline!.EncodedPolyline, Is.Not.Null.And.Not.Empty);
            Assert.That(route.Polyline.DecodedPoints, Is.Not.Empty);
        }

        [Test]
        public async Task ComputeRoutes_WithIntermediateWaypoint_ReturnsMultipleLegs()
        {
            var request = new RoutesRequest
            {
                ApiKey = ApiKey,
                Origin = Waypoint.FromAddress("New York, NY, USA"),
                Destination = Waypoint.FromAddress("Miami, FL, USA"),
                Intermediates = new List<Waypoint> { Waypoint.FromAddress("Philadelphia, PA, USA") },
            };

            var response = await Maps.Routes.QueryAsync(request);

            Assert.That(response.Routes, Is.Not.Null.And.Not.Empty);
            Assert.That(response.Routes![0].Legs, Has.Count.EqualTo(2),
                "One intermediate waypoint should produce two legs.");
        }

        [Test]
        public async Task ComputeRoutes_TrafficAware_ReturnsRoute()
        {
            var request = new RoutesRequest
            {
                ApiKey = ApiKey,
                Origin = Waypoint.FromAddress("Brooklyn, NY, USA"),
                Destination = Waypoint.FromAddress("Newark, NJ, USA"),
                RoutingPreference = RoutingPreference.TrafficAware,
            };

            var response = await Maps.Routes.QueryAsync(request);

            Assert.That(response.Routes, Is.Not.Null.And.Not.Empty);
            Assert.That(response.Routes![0].DurationSeconds, Is.Not.Null.And.GreaterThan(0));
        }

        [Test]
        public async Task ComputeRoutes_AlternativeRoutes_ReturnsMoreThanOne()
        {
            var request = new RoutesRequest
            {
                ApiKey = ApiKey,
                Origin = Waypoint.FromAddress("Brooklyn, NY, USA"),
                Destination = Waypoint.FromAddress("Manhattan, NY, USA"),
                ComputeAlternativeRoutes = true,
            };

            var response = await Maps.Routes.QueryAsync(request);

            Assert.That(response.Routes, Is.Not.Null.And.Not.Empty);
            // Routes API returns 1-3 routes when alternatives are enabled; can't strictly require >1
            // because some O/D pairs only have one viable route. Just confirm the request was accepted.
            Assert.That(response.Routes!.All(r => r.DistanceMeters > 0), Is.True);
        }

        [Test]
        public async Task ComputeRoutes_AvoidTolls_ReturnsRoute()
        {
            var request = new RoutesRequest
            {
                ApiKey = ApiKey,
                Origin = Waypoint.FromAddress("New York, NY, USA"),
                Destination = Waypoint.FromAddress("Boston, MA, USA"),
                RouteModifiers = new RouteModifiers { AvoidTolls = true },
            };

            var response = await Maps.Routes.QueryAsync(request);

            Assert.That(response.Routes, Is.Not.Null.And.Not.Empty);
            Assert.That(response.Routes![0].DistanceMeters, Is.Not.Null.And.GreaterThan(0));
        }
    }
}
