using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Geocoding.Request;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Polly;

namespace GoogleMapsApi.Extensions.DependencyInjection.Test
{
    /// <summary>
    /// Proves that the <see cref="IHttpClientBuilder"/> returned by <c>AddGoogleMaps</c> composes with
    /// <c>AddStandardResilienceHandler()</c> from <c>Microsoft.Extensions.Http.Resilience</c>, and that
    /// the standard handler retries Google's HTTP 429 (throttling) response. Hermetic — no live calls.
    /// </summary>
    [TestFixture]
    public class ResilienceTests
    {
        private const string OkGeocodingJson = "{\"status\":\"OK\",\"results\":[]}";

        [Test]
        public async Task AddStandardResilienceHandler_RetriesAfter429_ThenSucceeds()
        {
            var handler = new SequencedHandler(HttpStatusCode.TooManyRequests, HttpStatusCode.OK);
            var services = new ServiceCollection();
            services.AddGoogleMaps(o => o.ApiKey = "k")
                    .ConfigurePrimaryHttpMessageHandler(() => handler)
                    .AddStandardResilienceHandler(options =>
                    {
                        options.Retry.Delay = TimeSpan.Zero;
                        options.Retry.UseJitter = false;
                        options.Retry.BackoffType = DelayBackoffType.Constant;
                    });

            using var provider = services.BuildServiceProvider();
            var client = provider.GetRequiredService<IGoogleMapsClient>();

            await client.Geocode.QueryAsync(new GeocodingRequest { Address = "Pier 39" });

            Assert.That(handler.SendCount, Is.EqualTo(2), "The 429 should be retried, producing a second request.");
        }

        [Test]
        public void AddStandardResilienceHandler_ChainsOffEveryOverload()
        {
            var configuration = new ConfigurationBuilder().Build();

            Assert.Multiple(() =>
            {
                Assert.That(new ServiceCollection().AddGoogleMaps().AddStandardResilienceHandler(), Is.Not.Null);
                Assert.That(new ServiceCollection().AddGoogleMaps(o => o.ApiKey = "k").AddStandardResilienceHandler(), Is.Not.Null);
                Assert.That(new ServiceCollection().AddGoogleMaps(configuration).AddStandardResilienceHandler(), Is.Not.Null);
            });
        }

        /// <summary>
        /// Returns each supplied status code on successive calls (the last one repeats), so a test can
        /// model "429 then 200". <see cref="HttpStatusCode.OK"/> responses carry a valid geocoding body.
        /// </summary>
        private sealed class SequencedHandler : HttpMessageHandler
        {
            private readonly HttpStatusCode[] _statuses;

            public SequencedHandler(params HttpStatusCode[] statuses)
            {
                _statuses = statuses;
            }

            public int SendCount { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var status = _statuses[Math.Min(SendCount, _statuses.Length - 1)];
                SendCount++;

                var response = new HttpResponseMessage(status);
                if (status == HttpStatusCode.OK)
                {
                    response.Content = new StringContent(OkGeocodingJson);
                }

                return Task.FromResult(response);
            }
        }
    }
}
