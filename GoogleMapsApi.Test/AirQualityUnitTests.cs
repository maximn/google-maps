using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.AirQuality.Request;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    /// <summary>
    /// Hermetic tests for the Air Quality API request/response plumbing. No live network calls — URLs are
    /// asserted off <c>GetUri()</c>, request bodies are captured by a recording handler, and canned JSON
    /// is fed back through a stub handler.
    /// </summary>
    [TestFixture]
    public class AirQualityUnitTests
    {
        private const double Latitude = 37.4;
        private const double Longitude = -122.1;

        // --- URL generation ---------------------------------------------------------------

        [Test]
        public void CurrentConditions_GetUri_BuildsExpectedUrl()
        {
            var uri = new CurrentConditionsRequest { ApiKey = "k", Latitude = Latitude, Longitude = Longitude }.GetUri();

            Assert.That(uri.Scheme, Is.EqualTo("https"));
            Assert.That(uri.Host, Is.EqualTo("airquality.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/currentConditions:lookup"));
            Assert.That(uri.Query, Does.Contain("key=k"));
        }

        [Test]
        public void Forecast_GetUri_UsesForecastEndpoint()
        {
            var uri = new ForecastRequest { ApiKey = "k", Latitude = Latitude, Longitude = Longitude }.GetUri();
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/forecast:lookup"));
        }

        [Test]
        public void History_GetUri_UsesHistoryEndpoint()
        {
            var uri = new HistoryRequest { ApiKey = "k", Latitude = Latitude, Longitude = Longitude }.GetUri();
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/history:lookup"));
        }

        [Test]
        public void HeatmapTile_GetUri_BuildsTilePathWithMapType()
        {
            var uri = new HeatmapTileRequest
            {
                ApiKey = "k",
                MapType = AirQualityMapType.UsAqi,
                Zoom = 4,
                X = 3,
                Y = 6,
            }.GetUri();

            Assert.That(uri.Host, Is.EqualTo("airquality.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/mapTypes/US_AQI/heatmapTiles/4/3/6"));
            Assert.That(uri.Query, Does.Contain("key=k"));
        }

        // --- Guard clauses ----------------------------------------------------------------

        [Test]
        public void CurrentConditions_GetUri_MissingApiKey_Throws()
        {
            Assert.Throws<InvalidOperationException>(
                () => new CurrentConditionsRequest { Latitude = Latitude, Longitude = Longitude }.GetUri());
        }

        [Test]
        public void CurrentConditions_GetUri_NonSsl_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new CurrentConditionsRequest { ApiKey = "k", IsSSL = false, Latitude = Latitude, Longitude = Longitude }.GetUri());
        }

        [Test]
        public void HeatmapTile_GetUri_ZoomOutOfRange_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new HeatmapTileRequest { ApiKey = "k", MapType = AirQualityMapType.UsAqi, Zoom = 17, X = 1, Y = 1 }.GetUri());
        }

        // --- POST body serialization ------------------------------------------------------

        [Test]
        public async Task CurrentConditions_QueryAsync_PostsExpectedBody()
        {
            var handler = new RecordingHandler("{}");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            await client.AirQualityCurrentConditions.QueryAsync(new CurrentConditionsRequest
            {
                Latitude = Latitude,
                Longitude = Longitude,
                UniversalAqi = false,
                ExtraComputations = new() { ExtraComputation.HealthRecommendations, ExtraComputation.PollutantConcentration },
                LanguageCode = "en",
            });

            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastContentType, Is.EqualTo("application/json"));
            Assert.That(handler.LastRequestBody, Does.Contain("\"latitude\":37.4"));
            Assert.That(handler.LastRequestBody, Does.Contain("\"longitude\":-122.1"));
            Assert.That(handler.LastRequestBody, Does.Contain("\"universalAqi\":false"));
            Assert.That(handler.LastRequestBody, Does.Contain("\"HEALTH_RECOMMENDATIONS\""));
            Assert.That(handler.LastRequestBody, Does.Contain("\"POLLUTANT_CONCENTRATION\""));
        }

        [Test]
        public async Task CurrentConditions_QueryAsync_OmitsUnsetOptionalFields()
        {
            var handler = new RecordingHandler("{}");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            await client.AirQualityCurrentConditions.QueryAsync(
                new CurrentConditionsRequest { Latitude = Latitude, Longitude = Longitude });

            Assert.That(handler.LastRequestBody, Does.Not.Contain("extraComputations"));
            Assert.That(handler.LastRequestBody, Does.Not.Contain("languageCode"));
            Assert.That(handler.LastRequestBody, Does.Not.Contain("universalAqi"));
        }

        [Test]
        public async Task Forecast_QueryAsync_SerializesPeriodAndPaging()
        {
            var handler = new RecordingHandler("{}");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            await client.AirQualityForecast.QueryAsync(new ForecastRequest
            {
                Latitude = Latitude,
                Longitude = Longitude,
                Period = new Interval
                {
                    StartTime = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    EndTime = new DateTimeOffset(2024, 1, 2, 0, 0, 0, TimeSpan.Zero),
                },
                PageSize = 12,
                PageToken = "tok",
            });

            Assert.That(handler.LastRequestBody, Does.Contain("\"startTime\":\"2024-01-01T00:00:00Z\""));
            Assert.That(handler.LastRequestBody, Does.Contain("\"endTime\":\"2024-01-02T00:00:00Z\""));
            Assert.That(handler.LastRequestBody, Does.Contain("\"pageSize\":12"));
            Assert.That(handler.LastRequestBody, Does.Contain("\"pageToken\":\"tok\""));
        }

        [Test]
        public void Forecast_QueryAsync_DateTimeAndPeriodTogether_Throws()
        {
            var handler = new RecordingHandler("{}");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await client.AirQualityForecast.QueryAsync(new ForecastRequest
                {
                    Latitude = Latitude,
                    Longitude = Longitude,
                    DateTime = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    Period = new Interval { StartTime = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                }));
        }

        [Test]
        public async Task History_QueryAsync_SerializesHours()
        {
            var handler = new RecordingHandler("{}");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            await client.AirQualityHistory.QueryAsync(new HistoryRequest
            {
                Latitude = Latitude,
                Longitude = Longitude,
                Hours = 24,
            });

            Assert.That(handler.LastRequestBody, Does.Contain("\"hours\":24"));
        }

        [Test]
        public void History_QueryAsync_MultipleRanges_Throws()
        {
            var handler = new RecordingHandler("{}");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await client.AirQualityHistory.QueryAsync(new HistoryRequest
                {
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Hours = 24,
                    DateTime = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero),
                }));
        }

        // --- JSON deserialization ---------------------------------------------------------

        [Test]
        public async Task CurrentConditions_QueryAsync_DeserializesNestedFields()
        {
            const string json = """
            {
              "dateTime": "2024-01-01T10:00:00Z",
              "regionCode": "us",
              "indexes": [
                { "code": "uaqi", "displayName": "Universal AQI", "aqi": 63, "aqiDisplay": "63",
                  "color": { "red": 0.2, "green": 0.8, "blue": 0.1 },
                  "category": "Good air quality", "dominantPollutant": "pm25" }
              ],
              "pollutants": [
                { "code": "pm25", "displayName": "PM2.5", "fullName": "Fine particulate matter",
                  "concentration": { "units": "MICROGRAMS_PER_CUBIC_METER", "value": 12.5 },
                  "additionalInfo": { "sources": "Combustion", "effects": "Respiratory" } }
              ],
              "healthRecommendations": { "generalPopulation": "Enjoy outdoor activities." }
            }
            """;
            using var handler = new StubHandler(Encoding.UTF8.GetBytes(json), "application/json");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            var response = await client.AirQualityCurrentConditions.QueryAsync(
                new CurrentConditionsRequest { Latitude = Latitude, Longitude = Longitude });

            Assert.That(response.RegionCode, Is.EqualTo("us"));
            Assert.That(response.Indexes, Is.Not.Null.And.Count.EqualTo(1));
            Assert.That(response.Indexes![0].Aqi, Is.EqualTo(63));
            Assert.That(response.Indexes[0].Color!.Green, Is.EqualTo(0.8f));
            Assert.That(response.Pollutants![0].Concentration!.Value, Is.EqualTo(12.5));
            Assert.That(response.Pollutants[0].AdditionalInfo!.Sources, Is.EqualTo("Combustion"));
            Assert.That(response.HealthRecommendations!.GeneralPopulation, Is.EqualTo("Enjoy outdoor activities."));
        }

        [Test]
        public async Task Forecast_QueryAsync_DeserializesHourlyForecasts()
        {
            const string json = """
            {
              "hourlyForecasts": [
                { "dateTime": "2024-01-01T10:00:00Z", "indexes": [ { "code": "uaqi", "aqi": 70 } ] }
              ],
              "regionCode": "us",
              "nextPageToken": "next"
            }
            """;
            using var handler = new StubHandler(Encoding.UTF8.GetBytes(json), "application/json");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            var response = await client.AirQualityForecast.QueryAsync(
                new ForecastRequest { Latitude = Latitude, Longitude = Longitude });

            Assert.That(response.HourlyForecasts, Is.Not.Null.And.Count.EqualTo(1));
            Assert.That(response.HourlyForecasts![0].Indexes![0].Aqi, Is.EqualTo(70));
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

            var response = await client.AirQualityHeatmapTile.QueryAsync(new HeatmapTileRequest
            {
                MapType = AirQualityMapType.UaqiRedGreen,
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

        private sealed class RecordingHandler : HttpMessageHandler
        {
            private readonly string _responseJson;

            public RecordingHandler(string responseJson) => _responseJson = responseJson;

            public HttpMethod? LastMethod { get; private set; }
            public Uri? LastUri { get; private set; }
            public string? LastRequestBody { get; private set; }
            public string? LastContentType { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastMethod = request.Method;
                LastUri = request.RequestUri;
                if (request.Content != null)
                {
                    LastRequestBody = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                    LastContentType = request.Content.Headers.ContentType?.MediaType;
                }

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_responseJson, Encoding.UTF8, "application/json"),
                };
            }
        }
    }
}
