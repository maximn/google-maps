using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Pollen.Request;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    /// <summary>
    /// Hermetic tests for the Pollen API request/response plumbing. No live network calls — URLs are
    /// asserted off <c>GetUri()</c> and canned JSON is fed back through a stub handler.
    /// </summary>
    [TestFixture]
    public class PollenUnitTests
    {
        private const double Latitude = 37.4;
        private const double Longitude = -122.1;

        // --- URL generation ---------------------------------------------------------------

        [Test]
        public void Forecast_GetUri_BuildsExpectedUrl()
        {
            var uri = new PollenForecastRequest
            {
                ApiKey = "k",
                Latitude = Latitude,
                Longitude = Longitude,
                Days = 3,
            }.GetUri();

            Assert.That(uri.Scheme, Is.EqualTo("https"));
            Assert.That(uri.Host, Is.EqualTo("pollen.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/forecast:lookup"));
            Assert.That(uri.Query, Does.Contain("location.latitude=37.4"));
            Assert.That(uri.Query, Does.Contain("location.longitude=-122.1"));
            Assert.That(uri.Query, Does.Contain("days=3"));
            Assert.That(uri.Query, Does.Contain("key=k"));
        }

        [Test]
        public void Forecast_GetUri_AppendsOptionalParameters()
        {
            var uri = new PollenForecastRequest
            {
                ApiKey = "k",
                Latitude = Latitude,
                Longitude = Longitude,
                Days = 5,
                PageSize = 2,
                PageToken = "tok",
                LanguageCode = "fr",
                PlantsDescription = false,
            }.GetUri();

            Assert.That(uri.Query, Does.Contain("pageSize=2"));
            Assert.That(uri.Query, Does.Contain("pageToken=tok"));
            Assert.That(uri.Query, Does.Contain("languageCode=fr"));
            Assert.That(uri.Query, Does.Contain("plantsDescription=false"));
        }

        [Test]
        public void Forecast_GetUri_OmitsOptionalParameters_WhenUnset()
        {
            var uri = new PollenForecastRequest { ApiKey = "k", Latitude = Latitude, Longitude = Longitude, Days = 1 }.GetUri();

            Assert.That(uri.Query, Does.Not.Contain("pageSize"));
            Assert.That(uri.Query, Does.Not.Contain("pageToken"));
            Assert.That(uri.Query, Does.Not.Contain("languageCode"));
            Assert.That(uri.Query, Does.Not.Contain("plantsDescription"));
        }

        [Test]
        public void HeatmapTile_GetUri_BuildsTilePathWithMapType()
        {
            var uri = new PollenHeatmapTileRequest
            {
                ApiKey = "k",
                MapType = PollenMapType.GrassUpi,
                Zoom = 5,
                X = 9,
                Y = 12,
            }.GetUri();

            Assert.That(uri.Host, Is.EqualTo("pollen.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/mapTypes/GRASS_UPI/heatmapTiles/5/9/12"));
            Assert.That(uri.Query, Does.Contain("key=k"));
        }

        // --- Guard clauses ----------------------------------------------------------------

        [Test]
        public void Forecast_GetUri_MissingApiKey_Throws()
        {
            Assert.Throws<InvalidOperationException>(
                () => new PollenForecastRequest { Latitude = Latitude, Longitude = Longitude, Days = 1 }.GetUri());
        }

        [Test]
        public void Forecast_RequestingNonSsl_Throws()
        {
#pragma warning disable CS0618 // deliberately exercising the obsolete opt-out
            Assert.Throws<NotSupportedException>(
                () => new PollenForecastRequest { ApiKey = "k", IsSSL = false, Latitude = Latitude, Longitude = Longitude, Days = 1 }.GetUri());
#pragma warning restore CS0618
        }

        [Test]
        public void Forecast_GetUri_DaysOutOfRange_Throws([Values(0, 6)] int days)
        {
            Assert.Throws<ArgumentException>(
                () => new PollenForecastRequest { ApiKey = "k", Latitude = Latitude, Longitude = Longitude, Days = days }.GetUri());
        }

        [Test]
        public void HeatmapTile_GetUri_ZoomOutOfRange_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new PollenHeatmapTileRequest { ApiKey = "k", MapType = PollenMapType.TreeUpi, Zoom = 17, X = 1, Y = 1 }.GetUri());
        }

        // --- JSON deserialization ---------------------------------------------------------

        [Test]
        public async Task Forecast_QueryAsync_DeserializesNestedFields()
        {
            const string json = """
            {
              "regionCode": "US",
              "dailyInfo": [
                {
                  "date": { "year": 2024, "month": 4, "day": 1 },
                  "pollenTypeInfo": [
                    { "code": "TREE", "displayName": "Tree", "inSeason": true,
                      "indexInfo": { "code": "UPI", "displayName": "Universal Pollen Index", "value": 3,
                                     "category": "Moderate", "color": { "red": 1.0, "green": 0.8 } },
                      "healthRecommendations": [ "Keep windows closed." ] }
                  ],
                  "plantInfo": [
                    { "code": "BIRCH", "displayName": "Birch", "inSeason": true,
                      "indexInfo": { "code": "UPI", "value": 4, "category": "High" },
                      "plantDescription": { "type": "TREE", "family": "Betulaceae", "season": "Spring",
                                            "picture": "https://example/birch.png" } }
                  ]
                }
              ],
              "nextPageToken": "next"
            }
            """;
            using var handler = new StubHandler(Encoding.UTF8.GetBytes(json), "application/json");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            var response = await client.PollenForecast.QueryAsync(
                new PollenForecastRequest { Latitude = Latitude, Longitude = Longitude, Days = 1 });

            Assert.That(response.RegionCode, Is.EqualTo("US"));
            Assert.That(response.DailyInfo, Is.Not.Null.And.Count.EqualTo(1));

            var day = response.DailyInfo![0];
            Assert.That(day.Date!.Year, Is.EqualTo(2024));
            Assert.That(day.PollenTypeInfo![0].Code, Is.EqualTo("TREE"));
            Assert.That(day.PollenTypeInfo[0].IndexInfo!.Value, Is.EqualTo(3));
            Assert.That(day.PollenTypeInfo[0].IndexInfo!.Color!.Red, Is.EqualTo(1.0f));
            Assert.That(day.PollenTypeInfo[0].HealthRecommendations![0], Is.EqualTo("Keep windows closed."));
            Assert.That(day.PlantInfo![0].Code, Is.EqualTo("BIRCH"));
            Assert.That(day.PlantInfo[0].PlantDescription!.Family, Is.EqualTo("Betulaceae"));
            Assert.That(response.NextPageToken, Is.EqualTo("next"));
        }

        // --- Binary engine path -----------------------------------------------------------

        [Test]
        public async Task HeatmapTile_QueryAsync_ReturnsRawBytesAndContentType()
        {
            var payload = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            using var handler = new StubHandler(payload, "image/png");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            var response = await client.PollenHeatmapTile.QueryAsync(new PollenHeatmapTileRequest
            {
                MapType = PollenMapType.WeedUpi,
                Zoom = 4,
                X = 1,
                Y = 2,
            });

            Assert.That(response.Content, Is.EqualTo(payload));
            Assert.That(response.ContentType, Is.EqualTo("image/png"));
        }

        private sealed class StubHandler : HttpMessageHandler
        {
            private readonly byte[] _body;
            private readonly string _mediaType;

            public StubHandler(byte[] body, string mediaType)
            {
                _body = body;
                _mediaType = mediaType;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var content = new ByteArrayContent(_body);
                content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = content });
            }
        }
    }
}
