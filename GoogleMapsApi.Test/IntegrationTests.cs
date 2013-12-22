using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.TimeZone.Request;
using GoogleMapsApi.Entities.TimeZone.Response;
using NUnit.Framework;
using Status = GoogleMapsApi.Entities.Geocoding.Response.Status;

/////////////////////////////////////////////////////////////////////////
//
//  Warning:	The tests below run against the real Google API web
//				servers and count towards your query limit. Also, the tests
//				require a working internet connection in order to pass.
//				Their run time may vary depending on your connection,
//				network congestion and the current load on Google's servers.
//
/////////////////////////////////////////////////////////////////////////


/////////////////////////////////////////////////////////////////////////
//
//  Note:	Tests are written in the Arrange-Act-Assert pattern, where each
//			section is separated by a blank line.
//
/////////////////////////////////////////////////////////////////////////


namespace GoogleMapsApi.Test
{
	[TestFixture]
	public class IntegrationTests
	{
		[Test]
		public void Geocoding_ReturnsCorrectLocation()
		{
			var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };

			var result = GoogleMaps.Geocode.Query(request);

			if (result.Status == Status.OVER_QUERY_LIMIT)
				Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
			Assert.AreEqual(Status.OK, result.Status);
            Assert.AreEqual("40.7140289,-73.961305", result.Results.First().Geometry.Location.LocationString);
		}

		[Test]
		public void GeocodingAsync_ReturnsCorrectLocation()
		{
			var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };

			var result = GoogleMaps.Geocode.QueryAsync(request).Result;

			if (result.Status == Status.OVER_QUERY_LIMIT)
				Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
			Assert.AreEqual(Status.OK, result.Status);
            Assert.AreEqual("40.7140289,-73.961305", result.Results.First().Geometry.Location.LocationString);
		}

		[Test]
		[ExpectedException(typeof(AuthenticationException))]
		public void Geocoding_InvalidClientCredentials_Throws()
		{
			var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA", ClientID = "gme-ThisIsAUnitTest", SigningKey = "AAECAwQFBgcICQoLDA0ODxAREhM=" };

			Utils.ThrowInnerException(() => GoogleMaps.Geocode.Query(request));
		}

		[Test]
		[ExpectedException(typeof(AuthenticationException))]
		public void GeocodingAsync_InvalidClientCredentials_Throws()
		{
			var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA", ClientID = "gme-ThisIsAUnitTest", SigningKey = "AAECAwQFBgcICQoLDA0ODxAREhM=" };

			Utils.ThrowInnerException(() => GoogleMaps.Geocode.QueryAsync(request).Wait());
		}

		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void Geocoding_TimeoutTooShort_Throws()
		{
			var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };

			Utils.ThrowInnerException(() => GoogleMaps.Geocode.Query(request, TimeSpan.FromMilliseconds(1)));
		}

		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void GeocodingAsync_TimeoutTooShort_Throws()
		{
			var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };

			Utils.ThrowInnerException(() => GoogleMaps.Geocode.QueryAsync(request, TimeSpan.FromMilliseconds(1)).Wait());
		}

		[Test]
		[ExpectedException(typeof(TaskCanceledException))]
		public void GeocodingAsync_Cancel_Throws()
		{
			var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };

			var tokeSource = new CancellationTokenSource();
			var task = GoogleMaps.Geocode.QueryAsync(request, tokeSource.Token);
			tokeSource.Cancel();
			task.ThrowInnerException();
		}

		[Test]
		[ExpectedException(typeof(TaskCanceledException))]
		public void GeocodingAsync_WithPreCanceledToken_Cancels()
		{
			var request = new GeocodingRequest { Address = "285 Bedford Ave, Brooklyn, NY 11211, USA" };
			var cts = new CancellationTokenSource();
			cts.Cancel();

			var task = GoogleMaps.Geocode.QueryAsync(request, cts.Token);
			task.ThrowInnerException();
		}

		[Test]
		public void ReverseGeocoding_ReturnsCorrectAddress()
		{
			var request = new GeocodingRequest { Location = new Location(40.7141289, -73.9614074) };

			var result = GoogleMaps.Geocode.Query(request);

			if (result.Status == Status.OVER_QUERY_LIMIT)
				Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
			Assert.AreEqual(Status.OK, result.Status);
            Assert.AreEqual("287 Bedford Avenue, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
		}

		[Test]
		public void ReverseGeocodingAsync_ReturnsCorrectAddress()
		{
			var request = new GeocodingRequest { Location = new Location(40.7141289, -73.9614074) };

			var result = GoogleMaps.Geocode.QueryAsync(request).Result;

			if (result.Status == Status.OVER_QUERY_LIMIT)
				Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
			Assert.AreEqual(Status.OK, result.Status);
            Assert.AreEqual("287 Bedford Avenue, Brooklyn, NY 11211, USA", result.Results.First().FormattedAddress);
		}

		[Test]
		public void Directions_SumOfStepDistancesCorrect()
		{
			var request = new DirectionsRequest { Origin = "285 Bedford Ave, Brooklyn, NY, USA", Destination = "185 Broadway Ave, Manhattan, NY, USA" };

			var result = GoogleMaps.Directions.Query(request);

			if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
				Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
			Assert.AreEqual(DirectionsStatusCodes.OK, result.Status);
            Assert.AreEqual(7284, result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), 2000);
		}

		[Test]
		public void Directions_WithWayPoints()
		{
			var request = new DirectionsRequest { Origin = "NYC, USA", Destination = "Miami, USA", Waypoints = new string[]{"Philadelphia, USA"}, OptimizeWaypoints = true};

			var result = GoogleMaps.Directions.Query(request);

			if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
				Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
			Assert.AreEqual(DirectionsStatusCodes.OK, result.Status);
            Assert.AreEqual(156097, result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), 10 * 1000);

			StringAssert.Contains("Philadelphia", result.Routes.First().Legs.First().EndAddress);
		}

		[Test]
		public void Directions_Correct_OverviewPath()
		{
			DirectionsRequest request = new DirectionsRequest();
			request.Destination = "maleva 10, Ahtme, Kohtla-Järve, 31025 Ida-Viru County, Estonia";
			request.Origin = "veski 2, Jõhvi Parish, 41532 Ida-Viru County, Estonia";

			DirectionsResponse result = GoogleMaps.Directions.Query(request);

			OverviewPolyline overviewPath = result.Routes.First().OverviewPath;

			OverviewPolyline polyline = result.Routes.First().Legs.First().Steps.First().PolyLine;

			Assert.AreEqual(DirectionsStatusCodes.OK, result.Status);
			Assert.AreEqual(122, overviewPath.Points.Count(), 30);
			Assert.AreEqual(2, polyline.Points.Count());
		}

		[Test]
		public void DirectionsAsync_SumOfStepDistancesCorrect()
		{
			var request = new DirectionsRequest { Origin = "285 Bedford Ave, Brooklyn, NY, USA", Destination = "185 Broadway Ave, Manhattan, NY, USA" };

			var result = GoogleMaps.Directions.QueryAsync(request).Result;

			if (result.Status == DirectionsStatusCodes.OVER_QUERY_LIMIT)
				Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
			Assert.AreEqual(DirectionsStatusCodes.OK, result.Status);
            Assert.AreEqual(7284, result.Routes.First().Legs.First().Steps.Sum(s => s.Distance.Value), 2000);
		}

		[Test]
		public void Elevation_ReturnsCorrectElevation()
		{
			var request = new ElevationRequest { Locations = new[] { new Location(40.7141289, -73.9614074) } };

			var result = GoogleMaps.Elevation.Query(request);

			if (result.Status == Entities.Elevation.Response.Status.OVER_QUERY_LIMIT)
				Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
			Assert.AreEqual(Entities.Elevation.Response.Status.OK, result.Status);
			Assert.AreEqual(14.782454490661619, result.Results.First().Elevation);
		}

		[Test]
		public void ElevationAsync_ReturnsCorrectElevation()
		{
			var request = new ElevationRequest { Locations = new[] { new Location(40.7141289, -73.9614074) } };

			var result = GoogleMaps.Elevation.QueryAsync(request).Result;

			if (result.Status == Entities.Elevation.Response.Status.OVER_QUERY_LIMIT)
				Assert.Inconclusive("Cannot run test since you have exceeded your Google API query limit.");
			Assert.AreEqual(Entities.Elevation.Response.Status.OK, result.Status);
			Assert.AreEqual(14.782454490661619, result.Results.First().Elevation);
		}

        [Test]
        public void TimeZone_Correct_OverviewPath()
        {
            TimeZoneRequest request = new TimeZoneRequest();
            request.Location = new Location(55.866413, 12.501063);
            request.Language = "en";

            TimeZoneResponse result = GoogleMaps.TimeZone.Query(request);

            Assert.AreEqual(GoogleMapsApi.Entities.TimeZone.Response.Status.OK, result.Status);
        }
    }

	static class Utils
	{
		public static void ThrowInnerException(this Task task)
		{
			try
			{
				task.Wait();
			}
			catch (AggregateException ex)
			{
				throw ex.Flatten().InnerException;
			}
		}

		public static void ThrowInnerException(Action action)
		{
			try
			{
				action();
			}
			catch (AggregateException ex)
			{
				throw ex.InnerException;
			}
		}
	}
}
