using System.Threading.Tasks;
using GoogleMapsApi.Entities.AerialView.Request;
using GoogleMapsApi.Entities.AerialView.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    [BillableTest]
    public class AerialViewTests : BaseTestIntegration
    {
        // A stable US address that has 3D imagery available.
        private const string Address = "500 W 2nd St, Austin, TX 78701";

        [Test]
        public async Task RenderVideo_ReturnsProcessingOrActive_WithVideoId()
        {
            var response = await Maps.AerialView.RenderVideo.QueryAsync(
                new RenderVideoRequest { ApiKey = ApiKey, Address = Address });

            Assert.That(response.State, Is.AnyOf(VideoState.Processing, VideoState.Active));
            Assert.That(response.Metadata!.VideoId, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task LookupVideo_ByVideoId_ReturnsNonFailedVideo()
        {
            var render = await Maps.AerialView.RenderVideo.QueryAsync(
                new RenderVideoRequest { ApiKey = ApiKey, Address = Address });
            var videoId = render.Metadata!.VideoId!;

            var lookup = await Maps.AerialView.LookupVideo.QueryAsync(
                new LookupVideoRequest { ApiKey = ApiKey, VideoId = videoId });

            Assert.That(lookup.State, Is.Not.EqualTo(VideoState.Failed));
            Assert.That(lookup.Metadata!.VideoId, Is.EqualTo(videoId));
        }
    }
}
