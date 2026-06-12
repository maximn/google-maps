using System.Linq;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Roads.Request;
using GoogleMapsApi.Entities.Roads.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class RoadsTests : BaseTestIntegration
    {
        // A short path along Sullivans Creek Rd, Canberra — the Roads API documentation sample.
        private static readonly Location[] SamplePath =
        {
            new Location(-35.27801, 149.12958),
            new Location(-35.28032, 149.12907),
            new Location(-35.28099, 149.12929),
            new Location(-35.28144, 149.12984),
            new Location(-35.28194, 149.13003),
        };

        [Test]
        public async Task SnapToRoads_ReturnsSnappedPoints()
        {
            var request = new SnapToRoadsRequest { ApiKey = ApiKey, Path = SamplePath };

            SnapToRoadsResponse result = await Maps.SnapToRoads.QueryAsync(request);

            Assert.That(result.SnappedPoints, Is.Not.Null.And.Not.Empty);
            Assert.That(result.SnappedPoints!.All(p => p.Location != null && p.PlaceId != null), Is.True);
        }

        [Test]
        public async Task SnapToRoads_Interpolate_AddsPointsWithoutOriginalIndex()
        {
            var request = new SnapToRoadsRequest { ApiKey = ApiKey, Path = SamplePath, Interpolate = true };

            SnapToRoadsResponse result = await Maps.SnapToRoads.QueryAsync(request);

            Assert.That(result.SnappedPoints, Is.Not.Null.And.Not.Empty);
            // Interpolation fills in extra points between the inputs; those have no OriginalIndex.
            Assert.That(result.SnappedPoints!.Any(p => p.OriginalIndex == null), Is.True);
        }

        [Test]
        public async Task NearestRoads_ReturnsSnappedPoints()
        {
            var request = new NearestRoadsRequest
            {
                ApiKey = ApiKey,
                Points = new[] { new Location(60.170880, 24.942795), new Location(60.170879, 24.942796) },
            };

            NearestRoadsResponse result = await Maps.NearestRoads.QueryAsync(request);

            Assert.That(result.SnappedPoints, Is.Not.Null.And.Not.Empty);
        }

        // The Speed Limits service requires a Google Asset Tracking license; without one the API
        // returns HTTP 403. Marked billable/restricted so it is skipped unless RUN_BILLABLE_TESTS=true.
        [Test]
        [BillableTest]
        public async Task SpeedLimits_ReturnsSpeedLimits()
        {
            var request = new SpeedLimitsRequest { ApiKey = ApiKey, Path = SamplePath, Units = SpeedUnits.Kph };

            SpeedLimitsResponse result = await Maps.SpeedLimits.QueryAsync(request);

            Assert.That(result.SpeedLimits, Is.Not.Null.And.Not.Empty);
        }
    }
}
