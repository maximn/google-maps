namespace GoogleMapsApi.Test.IntegrationTests
{
    using System;
    using System.Linq;
    using GoogleMapsApi.Engine;
    using GoogleMapsApi.Entities.Directions.Response;
    using GoogleMapsApi.Entities.DistanceMatrix.Request;

    using NUnit.Framework;

    [TestFixture]
    public class DistanceMatrixTests : BaseTestIntegration
    {

        [Test]
        public void ShouldReturnValidValueWhenOneOriginAndOneDestinationsSpeciefed()
        {
            var request = new DistanceMatrixRequest
            {
                Origins = new[] { "49.64265,12.50088" },
                Destinations = new[] { "53.64308,10.52726" }
            };

            var result = GoogleMaps.DistanceMatrix.Query(request);

            if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status);
            CollectionAssert.AreEqual(
                new [] {"Alter Sirksfelder Weg 7, 23881 Koberg, Germany"}, 
                result.DestinationAddresses);
            CollectionAssert.AreEqual(
                new[] { "Pilsener Str. 18, 92726 Waidhaus, Germany" },
                result.OriginAddresses);
            Assert.AreEqual(DirectionsStatusCodes.OK, result.Rows.First().Elements.First().Status);
            Assert.IsNotNull(result.Rows.First().Elements.First().Distance);
            Assert.IsNotNull(result.Rows.First().Elements.First().Duration);
        }

        [Test]
        public void ShouldReturnValidValueWhenTwoOriginsSpecified()
        {
            var request = new DistanceMatrixRequest
            {
                Origins = new[] { "49.64265,12.50088", "49.17395,12.87028" },
                Destinations = new[] { "53.64308,10.52726" }
            };

            var result = GoogleMaps.DistanceMatrix.Query(request);

            if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status);
            CollectionAssert.AreEqual(
                new[] { "Alter Sirksfelder Weg 7, 23881 Koberg, Germany" },
                result.DestinationAddresses);
            CollectionAssert.AreEqual(
                new[] { "Pilsener Str. 18, 92726 Waidhaus, Germany", "Böhmerwaldstraße 19, 93444 Bad Kötzting, Germany" },
                result.OriginAddresses);
            Assert.AreEqual(2, result.Rows.Count());
            Assert.AreEqual(DirectionsStatusCodes.OK, result.Rows.First().Elements.First().Status);
            Assert.AreEqual(DirectionsStatusCodes.OK, result.Rows.Last().Elements.First().Status);
        }

        [Test]
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

            if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.AreEqual(DirectionsStatusCodes.OK, result.Status);

            Assert.IsNotNull(result.Rows.First().Elements.First().DurationInTraffic);
        }

        [Test]
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

            if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
            Assert.True(result.Rows.First().Elements.First().Distance.Text.Contains("mi"));
        }

        [Test]
        public void ShouldReplaceUriViaOnUriCreated()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Origins = new[] { "placeholder" },
                Destinations = new[] { "3,4" },
            };

            UriCreatedDelegate onUriCreated = delegate (Uri uri)
                {
                    var builder = new UriBuilder(uri);
                    builder.Query = builder.Query.Replace("placeholder", "1,2");
                    return builder.Uri;
                };

            GoogleMaps.DistanceMatrix.OnUriCreated += onUriCreated;

            try
            {
                var result = GoogleMaps.DistanceMatrix.Query(request);
                if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                    Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
                Assert.AreEqual(DirectionsStatusCodes.OK, result.Status);
                Assert.AreEqual("1,2", result.OriginAddresses.First());
            }
            finally
            {
                GoogleMaps.DistanceMatrix.OnUriCreated -= onUriCreated;
            }
        }

        [Test]
        public void ShouldPassRawDataToOnRawResponseRecivied()
        {
            var request = new DistanceMatrixRequest
            {
                ApiKey = ApiKey,
                Origins = new[] { "placeholder" },
                Destinations = new[] { "3,4" },
            };

            var rawData = new byte[0];

            RawResponseReciviedDelegate onRawResponseRecivied = data => rawData = data;
            GoogleMaps.DistanceMatrix.OnRawResponseRecivied += onRawResponseRecivied;

            try
            {
                var result = GoogleMaps.DistanceMatrix.Query(request);
                if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
                    Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
                Assert.AreEqual(DirectionsStatusCodes.OK, result.Status);
                CollectionAssert.IsNotEmpty(rawData);
            }
            finally
            {
                GoogleMaps.DistanceMatrix.OnRawResponseRecivied -= onRawResponseRecivied;
            }
        }
    }
}
