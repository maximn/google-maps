using System.Net.Http;
using System.Threading.Tasks;

namespace GoogleMapsApi.Test.IntegrationTests
{
    using System;
    using System.Linq;
    using GoogleMapsApi.Engine;
    using GoogleMapsApi.Entities.DistanceMatrix.Request;
    using GoogleMapsApi.Entities.DistanceMatrix.Response;

    using NUnit.Framework;
    using GoogleMapsApi.Test.Utils;

    [TestFixture]
    public class DistanceMatrixTests : BaseTestIntegration
    {

        [Test]
        public async Task ShouldReturnValidValueWhenOneOriginAndOneDestinationsSpeciefedAsync()
        {
            var request = new DistanceMatrixRequest
            {
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
                ApiKey = ApiKey
            };

            var result = await GoogleMaps.DistanceMatrix.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);
            CollectionAssert.AreEqual(
                new [] {"Alter Sirksfelder Weg 10, 23881 Koberg, Germany"}, 
                result.DestinationAddresses);
            CollectionAssert.AreEqual(
                new[] { "Pilsener Str. 18, 92726 Waidhaus, Germany" },
                result.OriginAddresses);
            Assert.AreEqual(DistanceMatrixElementStatusCodes.OK, result.Rows.First().Elements.First().Status);
            Assert.IsNotNull(result.Rows.First().Elements.First().Distance);
            Assert.IsNotNull(result.Rows.First().Elements.First().Duration);
        }

        [Test]
        [Obsolete]
        public void ShouldReturnValidValueWhenOneOriginAndOneDestinationsSpeciefed()
        {
            var request = new DistanceMatrixRequest
            {
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
                ApiKey = ApiKey
            };

            var result = GoogleMaps.DistanceMatrix.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);
            CollectionAssert.AreEqual(
                new[] { "Alter Sirksfelder Weg 10, 23881 Koberg, Germany" },
                result.DestinationAddresses);
            CollectionAssert.AreEqual(
                new[] { "Pilsener Str. 18, 92726 Waidhaus, Germany" },
                result.OriginAddresses);
            Assert.AreEqual(DistanceMatrixElementStatusCodes.OK, result.Rows.First().Elements.First().Status);
            Assert.IsNotNull(result.Rows.First().Elements.First().Distance);
            Assert.IsNotNull(result.Rows.First().Elements.First().Duration);
        }

        [Test]
        public async Task ShouldReturnValidValueWhenTwoOriginsSpecifiedAsync()
        {
            var request = new DistanceMatrixRequest
            {
                Origins = new[] { "49.64265,12.50088", "49.17395,12.87028" },
                Destinations = new[] { "53.64308,10.52726" },
                ApiKey = ApiKey
            };

            var result = await GoogleMaps.DistanceMatrix.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);
            CollectionAssert.AreEqual(
                new[] { "Alter Sirksfelder Weg 10, 23881 Koberg, Germany" },
                result.DestinationAddresses);
            CollectionAssert.AreEqual(
                new[] { "Pilsener Str. 18, 92726 Waidhaus, Germany", "Böhmerwaldstraße 19, 93444 Bad Kötzting, Germany" },
                result.OriginAddresses);
            Assert.AreEqual(2, result.Rows.Count());
            Assert.AreEqual(DistanceMatrixElementStatusCodes.OK, result.Rows.First().Elements.First().Status);
            Assert.AreEqual(DistanceMatrixElementStatusCodes.OK, result.Rows.Last().Elements.First().Status);
        }

        [Test]
        [Obsolete]
        public void ShouldReturnValidValueWhenTwoOriginsSpecified()
        {
            var request = new DistanceMatrixRequest
            {
                Origins = new[] { "49.64265,12.50088", "49.17395,12.87028" },
                Destinations = new[] { "53.64308,10.52726" },
                ApiKey = ApiKey
            };

            var result = GoogleMaps.DistanceMatrix.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);
            CollectionAssert.AreEqual(
                new[] { "Alter Sirksfelder Weg 10, 23881 Koberg, Germany" },
                result.DestinationAddresses);
            CollectionAssert.AreEqual(
                new[] { "Pilsener Str. 18, 92726 Waidhaus, Germany", "Böhmerwaldstraße 19, 93444 Bad Kötzting, Germany" },
                result.OriginAddresses);
            Assert.AreEqual(2, result.Rows.Count());
            Assert.AreEqual(DistanceMatrixElementStatusCodes.OK, result.Rows.First().Elements.First().Status);
            Assert.AreEqual(DistanceMatrixElementStatusCodes.OK, result.Rows.Last().Elements.First().Status);
        }

        [Test]
        public async Task ShouldReturnDurationInTrafficWhenDepartureTimeAndApiKeySpecifiedAsync()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                DepartureTime = new Time(),
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            var result = await GoogleMaps.DistanceMatrix.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);

            Assert.IsNotNull(result.Rows.First().Elements.First().DurationInTraffic);
        }

        [Test]
        [Obsolete]
        public void ShouldReturnDurationInTrafficWhenDepartureTimeAndApiKeySpecified()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                DepartureTime = new Time(),
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            var result = GoogleMaps.DistanceMatrix.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);

            Assert.IsNotNull(result.Rows.First().Elements.First().DurationInTraffic);
        }

        [Test]
        public void ShouldThrowExceptionWhenDepartureTimeAndArrivalTimeSpecifiedAsync()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                DepartureTime = new Time(),
                ArrivalTime = new Time(),
                Mode = DistanceMatrixTravelModes.transit,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await GoogleMaps.DistanceMatrix.QueryAsync(request));
        }

        [Test]
        [Obsolete]
        public void ShouldThrowExceptionWhenDepartureTimeAndArrivalTimeSpecified()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                DepartureTime = new Time(),
                ArrivalTime = new Time(),
                Mode = DistanceMatrixTravelModes.transit,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.Throws<ArgumentException>(() => GoogleMaps.DistanceMatrix.Query(request));
        }

        [Test]
        public void ShouldThrowExceptionWhenArrivalTimeSpecifiedForNonTransitModesAsync()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                ArrivalTime = new Time(),
                Mode = DistanceMatrixTravelModes.driving,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await GoogleMaps.DistanceMatrix.QueryAsync(request));
        }

        [Test]
        [Obsolete]
        public void ShouldThrowExceptionWhenArrivalTimeSpecifiedForNonTransitModes()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                ArrivalTime = new Time(),
                Mode = DistanceMatrixTravelModes.driving,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.Throws<ArgumentException>(() => GoogleMaps.DistanceMatrix.Query(request));
        }

        [Test]
        public void ShouldThrowExceptionWheTransitRoutingPreferenceSpecifiedForNonTransitModesAsync()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Mode = DistanceMatrixTravelModes.driving,
                TransitRoutingPreference = DistanceMatrixTransitRoutingPreferences.less_walking,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await GoogleMaps.DistanceMatrix.QueryAsync(request));
        }

        [Test]
        [Obsolete]
        public void ShouldThrowExceptionWheTransitRoutingPreferenceSpecifiedForNonTransitModes()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Mode = DistanceMatrixTravelModes.driving,
                TransitRoutingPreference = DistanceMatrixTransitRoutingPreferences.less_walking,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.Throws<ArgumentException>(() => GoogleMaps.DistanceMatrix.Query(request));
        }

        [Test]
        public void ShouldThrowExceptionWhenTrafficModelSuppliedForNonDrivingModeAsync()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Mode = DistanceMatrixTravelModes.transit,
                DepartureTime = new Time(),
                TrafficModel = DistanceMatrixTrafficModels.optimistic,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await GoogleMaps.DistanceMatrix.QueryAsync(request));
        }

        [Test]
        [Obsolete]
        public void ShouldThrowExceptionWhenTrafficModelSuppliedForNonDrivingMode()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Mode = DistanceMatrixTravelModes.transit,
                DepartureTime = new Time(),
                TrafficModel = DistanceMatrixTrafficModels.optimistic,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.Throws<ArgumentException>(() => GoogleMaps.DistanceMatrix.Query(request));
        }

        [Test]
        public void ShouldThrowExceptionWhenTrafficModelSuppliedWithoutDepartureTimeAsync()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Mode = DistanceMatrixTravelModes.driving,
                TrafficModel = DistanceMatrixTrafficModels.optimistic,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await GoogleMaps.DistanceMatrix.QueryAsync(request));
        }

        [Test]
        [Obsolete]
        public void ShouldThrowExceptionWhenTrafficModelSuppliedWithoutDepartureTime()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Mode = DistanceMatrixTravelModes.driving,
                TrafficModel = DistanceMatrixTrafficModels.optimistic,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.Throws<ArgumentException>(() => GoogleMaps.DistanceMatrix.Query(request));
        }

        [Test]
        public void ShouldThrowExceptionWhenTransitModesSuppliedForNonTransitModeAsync()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Mode = DistanceMatrixTravelModes.driving,
                TransitModes = new DistanceMatrixTransitModes[] { DistanceMatrixTransitModes.bus, DistanceMatrixTransitModes.subway},
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await GoogleMaps.DistanceMatrix.QueryAsync(request));
        }

        [Test]
        [Obsolete]
        public void ShouldThrowExceptionWhenTransitModesSuppliedForNonTransitMode()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Mode = DistanceMatrixTravelModes.driving,
                TransitModes = new DistanceMatrixTransitModes[] { DistanceMatrixTransitModes.bus, DistanceMatrixTransitModes.subway },
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            Assert.Throws<ArgumentException>(() => GoogleMaps.DistanceMatrix.Query(request));
        }

        [Test]
        public async Task ShouldReturnImperialUnitsIfImperialPassedAsParameterAsync()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Units = DistanceMatrixUnitSystems.imperial,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            var result = await GoogleMaps.DistanceMatrix.QueryAsync(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.True(result.Rows.First().Elements.First().Distance.Text.Contains("mi"));
        }

        [Test]
        [Obsolete]
        public void ShouldReturnImperialUnitsIfImperialPassedAsParameter()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Units = DistanceMatrixUnitSystems.imperial,
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" },
            };

            var result = GoogleMaps.DistanceMatrix.Query(request);

            AssertInconclusive.NotExceedQuota(result);
            Assert.True(result.Rows.First().Elements.First().Distance.Text.Contains("mi"));
        }

        [Test]
        public async Task ShouldReplaceUriViaOnUriCreatedAsync()
        {
            var request = new DistanceMatrixRequest
            {
                Origins = new[] { "placeholder" },
                Destinations = new[] { "3,4" },
                ApiKey = ApiKey
            };

            Uri OnUriCreated(Uri uri)
            {
                var builder = new UriBuilder(uri);
                var tmp = builder.Query.Replace("placeholder", "1,2").Remove(0, 1);
                builder.Query = tmp;
                return builder.Uri;
            }

            GoogleMaps.DistanceMatrix.OnUriCreated += OnUriCreated;

            try
            {
                var result = await GoogleMaps.DistanceMatrix.QueryAsync(request);

                AssertInconclusive.NotExceedQuota(result);
                Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);
                Assert.AreEqual("1,2", result.OriginAddresses.First());
            }
            finally
            {
                GoogleMaps.DistanceMatrix.OnUriCreated -= OnUriCreated;
            }
        }

        [Test]
        [Obsolete]
        public void ShouldReplaceUriViaOnUriCreated()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Origins = new[] { "placeholder" },
                Destinations = new[] { "3,4" },
            };

            Uri OnUriCreated(Uri uri)
            {
                var builder = new UriBuilder(uri);
                var tmp = builder.Query.Replace("placeholder", "1,2").Remove(0, 1);
                builder.Query = tmp;
                return builder.Uri;
            }

            GoogleMaps.DistanceMatrix.OnUriCreated += OnUriCreated;

            try
            {
                var result = GoogleMaps.DistanceMatrix.Query(request);

                AssertInconclusive.NotExceedQuota(result);
                Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);
                Assert.AreEqual("1,2", result.OriginAddresses.First());
            }
            finally
            {
                GoogleMaps.DistanceMatrix.OnUriCreated -= OnUriCreated;
            }
        }

        [Test]
        public async Task ShouldPassRawDataToOnRawResponseReceivedAsync()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Origins = new[] { "placeholder" },
                Destinations = new[] { "3,4" },
            };

            HttpContent rawData = null;

            void OnRawResponseReceived(HttpResponseMessage data) => rawData = data.Content;
            GoogleMaps.DistanceMatrix.OnRawResponseReceived += OnRawResponseReceived;

            try
            {
                var result = await GoogleMaps.DistanceMatrix.QueryAsync(request);

                AssertInconclusive.NotExceedQuota(result);
                Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);
                Assert.IsNotNull(rawData);
            }
            finally
            {
                GoogleMaps.DistanceMatrix.OnRawResponseReceived -= OnRawResponseReceived;
            }
        }

        [Test]
        [Obsolete]
        public void ShouldPassRawDataToOnRawResponseReceived()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Origins = new[] { "placeholder" },
                Destinations = new[] { "3,4" },
            };

            HttpContent rawData = null;

            void OnRawResponseReceived(HttpResponseMessage data) => rawData = data.Content;
            GoogleMaps.DistanceMatrix.OnRawResponseReceived += OnRawResponseReceived;

            try
            {
                var result = GoogleMaps.DistanceMatrix.Query(request);

                AssertInconclusive.NotExceedQuota(result);
                Assert.AreEqual(DistanceMatrixStatusCodes.OK, result.Status, result.ErrorMessage);
                Assert.IsNotNull(rawData);
            }
            finally
            {
                GoogleMaps.DistanceMatrix.OnRawResponseReceived -= OnRawResponseReceived;
            }
        }
    }
}
