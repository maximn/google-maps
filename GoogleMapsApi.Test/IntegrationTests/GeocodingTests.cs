using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Geocoding.Request;
using NUnit.Framework;
using Status = GoogleMapsApi.Entities.Geocoding.Response.Status;
using GoogleMapsApi.Test.Utils;

namespace GoogleMapsApi.Test.IntegrationTests
{
    [TestFixture]
    public class GeocodingTests : BaseTestIntegration
    {
        [Test]
        public async Task Geocoding_ReturnsCorrectLocation()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                Address = "285 Bedford Ave, Brooklyn, NY 11211, USA"
            };

            var result = await GoogleMaps.Geocode.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            // 40.{*}, -73.{*}
            StringAssert.IsMatch("40\\.\\d*,-73\\.\\d*", result.Results.First().Geometry.Location.LocationString);
        }


        [Test]
        public void GeocodingAsync_ReturnsCorrectLocation()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                Address = "285 Bedford Ave, Brooklyn, NY 11211, USA"
            };

            var result = GoogleMaps.Geocode.QueryAsync(request).Result;

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            // 40.{*}, -73.{*}
            StringAssert.IsMatch("40\\.\\d*,-73\\.\\d*", result.Results.First().Geometry.Location.LocationString);
        }

        [Test]
        public void Geocoding_InvalidClientCredentials_Throws()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA", ApiKey = ApiKey, ClientID = "gme-ThisIsAUnitTest", SigningKey = "AAECAwQFBgcICQoLDA0ODxAREhM=" };

            Assert.ThrowsAsync<AuthenticationException>(() => GoogleMaps.Geocode.QueryAsync(request));
        }

        [Test]
        public void GeocodingAsync_InvalidClientCredentials_Throws()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA", ClientID = "gme-ThisIsAUnitTest", SigningKey = "AAECAwQFBgcICQoLDA0ODxAREhM=" };

            Assert.Throws(Is.TypeOf<AggregateException>().And.InnerException.TypeOf<AuthenticationException>(),
                          () => GoogleMaps.Geocode.QueryAsync(request).Wait());
        }

        [Test]
        public void GeocodingAsync_Cancel_Throws()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };

            var tokeSource = new CancellationTokenSource();
            var task = GoogleMaps.Geocode.QueryAsync(request, tokeSource.Token);
            tokeSource.Cancel();

            Assert.Throws(Is.TypeOf<AggregateException>().And.InnerException.TypeOf<TaskCanceledException>(),
                () => task.Wait());
        }

        [Test]
        public void GeocodingAsync_WithPreCanceledToken_Cancels()
        {
            var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var task = GoogleMaps.Geocode.QueryAsync(request, cts.Token);

            Assert.Throws(Is.TypeOf<AggregateException>().And.InnerException.TypeOf<TaskCanceledException>(),
                            () => task.Wait());
        }

        [Test]
        public async Task ReverseGeocoding_ReturnsCorrectAddress()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                Location = new Location(40.7141289, -73.9614074)
            };

            var result = await GoogleMaps.Geocode.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            StringAssert.Contains("Bedford Ave, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
        }

        [Test]
        public void ReverseGeocodingAsync_ReturnsCorrectAddress()
        {
            var request = new GeocodingRequest
            {
                ApiKey = ApiKey,
                Location = new Location(40.7141289, -73.9614074)
            };

            var result = GoogleMaps.Geocode.QueryAsync(request).Result;

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(Status.OK, result.Status);
            StringAssert.Contains("Bedford Ave, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
        }
    }
}
