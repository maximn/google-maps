using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Solar.Request;
using GoogleMapsApi.Entities.Solar.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    /// <summary>
    /// Hermetic tests for the Solar API request/response plumbing. No live network calls — URLs are
    /// asserted directly off <c>GetUri()</c> and HTTP exchanges are intercepted by a stub handler.
    /// </summary>
    [TestFixture]
    public class SolarUnitTests
    {
        // --- URL generation ---------------------------------------------------------------

        [Test]
        public void BuildingInsights_GetUri_BuildsExpectedUrl()
        {
            var uri = new BuildingInsightsRequest
            {
                ApiKey = "k",
                Latitude = 37.4,
                Longitude = -122.1,
                RequiredQuality = ImageryQuality.High,
            }.GetUri();

            Assert.That(uri.Scheme, Is.EqualTo("https"));
            Assert.That(uri.Host, Is.EqualTo("solar.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/buildingInsights:findClosest"));
            Assert.That(uri.Query, Does.Contain("location.latitude=37.4"));
            Assert.That(uri.Query, Does.Contain("location.longitude=-122.1"));
            Assert.That(uri.Query, Does.Contain("requiredQuality=HIGH"));
            Assert.That(uri.Query, Does.Contain("key=k"));
        }

        [Test]
        public void BuildingInsights_GetUri_OmitsRequiredQuality_WhenUnset()
        {
            var uri = new BuildingInsightsRequest { ApiKey = "k", Latitude = 1, Longitude = 2 }.GetUri();
            Assert.That(uri.Query, Does.Not.Contain("requiredQuality"));
        }

        [Test]
        public void DataLayers_GetUri_BuildsExpectedUrl()
        {
            var uri = new DataLayersRequest
            {
                ApiKey = "k",
                Latitude = 37.4,
                Longitude = -122.1,
                RadiusMeters = 50,
                View = DataLayerView.FullLayers,
                RequiredQuality = ImageryQuality.Medium,
                PixelSizeMeters = 0.5,
                ExactQualityRequired = true,
            }.GetUri();

            Assert.That(uri.Host, Is.EqualTo("solar.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/dataLayers:get"));
            Assert.That(uri.Query, Does.Contain("radiusMeters=50"));
            Assert.That(uri.Query, Does.Contain("view=FULL_LAYERS"));
            Assert.That(uri.Query, Does.Contain("requiredQuality=MEDIUM"));
            Assert.That(uri.Query, Does.Contain("pixelSizeMeters=0.5"));
            Assert.That(uri.Query, Does.Contain("exactQualityRequired=true"));
            Assert.That(uri.Query, Does.Contain("key=k"));
        }

        [Test]
        public void GeoTiff_GetUri_AppendsKeyToLayerUrl()
        {
            var uri = new GeoTiffRequest
            {
                ApiKey = "k",
                Url = "https://solar.googleapis.com/v1/geoTiff:get?id=abc123",
            }.GetUri();

            Assert.That(uri.Host, Is.EqualTo("solar.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1/geoTiff:get"));
            Assert.That(uri.Query, Does.Contain("id=abc123"));
            Assert.That(uri.Query, Does.Contain("key=k"));
        }

        // --- Guard clauses ----------------------------------------------------------------

        [Test]
        public void BuildingInsights_GetUri_MissingApiKey_Throws()
        {
            Assert.Throws<InvalidOperationException>(
                () => new BuildingInsightsRequest { Latitude = 1, Longitude = 2 }.GetUri());
        }

        [Test]
        public void BuildingInsights_RequestingNonSsl_Throws()
        {
#pragma warning disable CS0618 // deliberately exercising the obsolete opt-out
            Assert.Throws<NotSupportedException>(
                () => new BuildingInsightsRequest { ApiKey = "k", IsSSL = false, Latitude = 1, Longitude = 2 }.GetUri());
#pragma warning restore CS0618
        }

        [Test]
        public void DataLayers_GetUri_NonPositiveRadius_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new DataLayersRequest { ApiKey = "k", Latitude = 1, Longitude = 2, RadiusMeters = 0 }.GetUri());
        }

        [Test]
        public void GeoTiff_GetUri_EmptyUrl_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new GeoTiffRequest { ApiKey = "k", Url = "" }.GetUri());
        }

        [Test]
        public void GeoTiff_GetUri_ForeignHost_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new GeoTiffRequest { ApiKey = "k", Url = "https://evil.example.com/geoTiff:get?id=x" }.GetUri());
        }

        // --- Binary engine path -----------------------------------------------------------

        [Test]
        public async Task GeoTiff_QueryAsync_ReturnsRawBytesAndContentType()
        {
            var payload = new byte[] { 0x49, 0x49, 0x2A, 0x00, 0x10, 0x20, 0x30 };
            using var handler = new StubHandler(payload, "image/tiff");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            var response = await client.SolarGeoTiff.QueryAsync(new GeoTiffRequest
            {
                Url = "https://solar.googleapis.com/v1/geoTiff:get?id=abc",
            });

            Assert.That(response.Content, Is.EqualTo(payload));
            Assert.That(response.ContentType, Is.EqualTo("image/tiff"));
        }

        // --- JSON deserialization ---------------------------------------------------------

        [Test]
        public async Task BuildingInsights_QueryAsync_DeserializesNestedFields()
        {
            const string json = """
            {
              "name": "buildings/abc",
              "center": { "latitude": 37.4, "longitude": -122.1 },
              "imageryQuality": "HIGH",
              "solarPotential": {
                "maxArrayPanelsCount": 42,
                "panelCapacityWatts": 250,
                "solarPanels": [
                  { "center": { "latitude": 37.4, "longitude": -122.1 }, "orientation": "LANDSCAPE", "yearlyEnergyDcKwh": 410.5, "segmentIndex": 0 }
                ],
                "financialAnalyses": [
                  {
                    "monthlyBill": { "currencyCode": "USD", "units": "35" },
                    "panelConfigIndex": 3,
                    "cashPurchaseSavings": {
                      "paybackYears": 8.5,
                      "savings": { "savingsLifetime": { "currencyCode": "USD", "units": "42000", "nanos": 500000000 } }
                    }
                  }
                ]
              }
            }
            """;
            using var handler = new StubHandler(Encoding.UTF8.GetBytes(json), "application/json");
            using var http = new HttpClient(handler);
            var client = new GoogleMapsClient(http, new GoogleMapsClientOptions { ApiKey = "k" });

            var response = await client.SolarBuildingInsights.QueryAsync(new BuildingInsightsRequest { Latitude = 37.4, Longitude = -122.1 });

            Assert.That(response.Name, Is.EqualTo("buildings/abc"));
            Assert.That(response.ImageryQuality, Is.EqualTo(ImageryQuality.High));
            Assert.That(response.SolarPotential, Is.Not.Null);
            Assert.That(response.SolarPotential!.MaxArrayPanelsCount, Is.EqualTo(42));
            Assert.That(response.SolarPotential.SolarPanels![0].Orientation, Is.EqualTo(SolarPanelOrientation.Landscape));

            var analysis = response.SolarPotential.FinancialAnalyses![0];
            Assert.That(analysis.MonthlyBill!.Units, Is.EqualTo(35L), "int64 encoded as JSON string should parse");
            var lifetime = analysis.CashPurchaseSavings!.Savings!.SavingsLifetime!;
            Assert.That(lifetime.Units, Is.EqualTo(42000L));
            Assert.That(lifetime.Nanos, Is.EqualTo(500000000));
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
