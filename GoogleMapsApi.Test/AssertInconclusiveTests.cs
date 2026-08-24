using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Test.Utils;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class AssertInconclusiveTests
    {
        [Test]
        public void HasTransitStep_NonOkDirectionsResponse_Fails()
        {
            var response = new DirectionsResponse
            {
                Status = DirectionsStatusCodes.REQUEST_DENIED,
                ErrorMessage = "API key rejected",
            };

            var ex = Assert.Throws<AssertionException>(() => AssertInconclusive.HasTransitStep(response));

            Assert.That(ex!.Message, Does.Contain("API key rejected"));
        }

        [Test]
        public void HasTransitStep_OkResponseWithoutTransitStep_IsInconclusive()
        {
            var response = new DirectionsResponse
            {
                Status = DirectionsStatusCodes.OK,
                Routes = new[]
                {
                    new Route
                    {
                        Legs = new[]
                        {
                            new Leg
                            {
                                Steps = new[] { new Step() },
                            },
                        },
                    },
                },
            };

            Assert.Throws<InconclusiveException>(() => AssertInconclusive.HasTransitStep(response));
        }

        [Test]
        public void EnforcedRouteLengthLimit_RouteRejected_Passes()
        {
            var response = new DirectionsResponse { Status = DirectionsStatusCodes.MAX_ROUTE_LENGTH_EXCEEDED };

            Assert.DoesNotThrow(() => AssertInconclusive.EnforcedRouteLengthLimit(response));
        }

        [Test]
        public void EnforcedRouteLengthLimit_GoogleAcceptedTheRoute_IsInconclusive()
        {
            var response = new DirectionsResponse { Status = DirectionsStatusCodes.OK };

            Assert.Throws<InconclusiveException>(() => AssertInconclusive.EnforcedRouteLengthLimit(response));
        }

        [Test]
        public void EnforcedRouteLengthLimit_UnrelatedFailure_Fails()
        {
            var response = new DirectionsResponse
            {
                Status = DirectionsStatusCodes.REQUEST_DENIED,
                ErrorMessage = "API key rejected",
            };

            var ex = Assert.Throws<AssertionException>(() => AssertInconclusive.EnforcedRouteLengthLimit(response));

            Assert.That(ex!.Message, Does.Contain("API key rejected"));
        }
    }
}
