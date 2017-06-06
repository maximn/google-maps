using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using NUnit.Framework;
using Status = GoogleMapsApi.Entities.Geocoding.Response.Status;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class GeocodingTests : BaseTestIntegration
    {
        [Test]
        public void Geocoding_ReturnsCorrectLocation()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                Address = "285 Bedford Ave, Brooklyn, NY 11211, USA"
            };

            var result = GoogleMaps.Geocode.Query(request).Result;

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Status.OK, result.Status);
            // 40.{*}, -73.{*}
            StringAssert.IsMatch("40\\.\\d*,-73\\.\\d*", result.Results.First().Geometry.Location.LocationString);
        }


        [Test]
        public void Geocoding_InvalidClientCredentials_Throws()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA", ClientID = "gme-ThisIsAUnitTest", SigningKey = "AAECAwQFBgcICQoLDA0ODxAREhM=" };
            var task = GoogleMaps.Geocode.Query(request);

            Assert.Throws(Is.TypeOf<AggregateException>().And.InnerException.TypeOf<UnauthorizedAccessException>(),() => task.Wait());
        }

        [Test]
        public void Geocoding_Cancel_Throws()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };

            var tokeSource = new CancellationTokenSource();
            tokeSource.Cancel();

            var task = GoogleMaps.Geocode.Query(request);

            Assert.Throws(Is.TypeOf<OperationCanceledException>(),
                () => task.Wait(tokeSource.Token));
        }

        [Test]
        public void Geocoding_WithPreCanceledToken_Cancels()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var task = GoogleMaps.Geocode.Query(request);

            //Tasks support cancellation tokens in their API, so lets use that for simplicity
            Assert.Throws(Is.TypeOf<OperationCanceledException>(),
                            () => task.Wait(cts.Token));
        }

        [Test]
        public void ReverseGeocoding_ReturnsCorrectAddress()
        {
            var request = new GeocodingRequest { Location = new Location(40.7141289, -73.9614074) };

            var result = GoogleMaps.Geocode.Query(request).Result;

            if (result.Status == Status.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(Status.OK, result.Status);
            StringAssert.Contains("Bedford Ave, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
        }
    }
}
