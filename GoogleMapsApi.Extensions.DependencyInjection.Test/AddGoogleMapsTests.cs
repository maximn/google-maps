using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GoogleMapsApi.Extensions.DependencyInjection.Test
{
    /// <summary>
    /// Hermetic tests for <c>services.AddGoogleMaps(...)</c>. No live network calls — the primary
    /// HTTP handler is replaced with <see cref="CapturingHandler"/> so the outgoing request URI
    /// can be inspected.
    /// </summary>
    [TestFixture]
    public class AddGoogleMapsTests
    {
        private const string OkGeocodingJson = "{\"status\":\"OK\",\"results\":[]}";

        [Test]
        public void AddGoogleMaps_RegistersResolvableClient()
        {
            var services = new ServiceCollection();

            services.AddGoogleMaps();

            using var provider = services.BuildServiceProvider();
            var client = provider.GetRequiredService<IGoogleMapsClient>();

            Assert.That(client, Is.Not.Null);
            Assert.That(client, Is.InstanceOf<GoogleMapsClient>());
        }

        [Test]
        public void AddGoogleMaps_ReturnsHttpClientBuilder()
        {
            var services = new ServiceCollection();

            IHttpClientBuilder builder = services.AddGoogleMaps(o => o.ApiKey = "k");

            Assert.That(builder, Is.Not.Null);
        }

        [Test]
        public async Task AddGoogleMaps_WithActionOptions_FlowsApiKeyToRequest()
        {
            var handler = new CapturingHandler(OkGeocodingJson);
            var services = new ServiceCollection();
            services.AddGoogleMaps(o => o.ApiKey = "action-key")
                    .ConfigurePrimaryHttpMessageHandler(() => handler);

            using var provider = services.BuildServiceProvider();
            var client = provider.GetRequiredService<IGoogleMapsClient>();

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "Pier 39" });

            Assert.That(handler.LastRequestUri!.Query, Does.Contain("key=action-key"));
        }

        [Test]
        public async Task AddGoogleMaps_WithConfiguration_FlowsApiKeyToRequest()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ApiKey"] = "config-key" })
                .Build();

            var handler = new CapturingHandler(OkGeocodingJson);
            var services = new ServiceCollection();
            services.AddGoogleMaps(configuration)
                    .ConfigurePrimaryHttpMessageHandler(() => handler);

            using var provider = services.BuildServiceProvider();
            var client = provider.GetRequiredService<IGoogleMapsClient>();

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "Pier 39" });

            Assert.That(handler.LastRequestUri!.Query, Does.Contain("key=config-key"));
        }

        [Test]
        public async Task AddGoogleMaps_NoOptions_SendsRequestWithoutKey()
        {
            var handler = new CapturingHandler(OkGeocodingJson);
            var services = new ServiceCollection();
            services.AddGoogleMaps()
                    .ConfigurePrimaryHttpMessageHandler(() => handler);

            using var provider = services.BuildServiceProvider();
            var client = provider.GetRequiredService<IGoogleMapsClient>();

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "Pier 39" });

            Assert.That(handler.LastRequestUri!.Query, Does.Not.Contain("key="));
        }

        [Test]
        public void AddGoogleMaps_NullServices_Throws()
        {
            IServiceCollection services = null!;

            Assert.Throws<ArgumentNullException>(() => services.AddGoogleMaps());
            Assert.Throws<ArgumentNullException>(() => services.AddGoogleMaps(_ => { }));
            Assert.Throws<ArgumentNullException>(() => services.AddGoogleMaps((IConfiguration)null!));
        }

        [Test]
        public void AddGoogleMaps_NullConfigureArgument_Throws()
        {
            var services = new ServiceCollection();

            Assert.Throws<ArgumentNullException>(() => services.AddGoogleMaps((Action<GoogleMapsClientOptions>)null!));
            Assert.Throws<ArgumentNullException>(() => services.AddGoogleMaps((IConfiguration)null!));
        }

        private sealed class CapturingHandler : HttpMessageHandler
        {
            private readonly string _responseBody;

            public CapturingHandler(string responseBody)
            {
                _responseBody = responseBody;
            }

            public Uri? LastRequestUri { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                LastRequestUri = request.RequestUri;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_responseBody)
                });
            }
        }
    }
}
