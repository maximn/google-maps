using GoogleMapsApi.Test.Utils;
using GoogleMapsApi.Test.Vcr;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace GoogleMapsApi.Test.IntegrationTests
{
    //  Note:	By default (VCR_MODE=replay) these tests serve committed cassettes — no key, no network,
    //			no charge. Set VCR_MODE=record (or auto/live) with a real GOOGLE_API_KEY to talk to the
    //			live Google APIs; that counts towards your quota and, for billable APIs, your bill.
    //			See .agents/testing.md for the record/replay workflow.

    public abstract class BaseTestIntegration
    {
        const string ApiKeyEnvironmentVariable = "GOOGLE_API_KEY";

        // Replay needs no real key, so contributors without one can run the whole suite offline.
        private const string ReplayApiKey = "REPLAY";

        private HttpClient? _httpClient;

        protected IGoogleMapsClient Maps { get; private set; } = null!;

        [SetUp]
        public void SetUpVcr()
        {
            var test = TestContext.CurrentContext.Test;
            var cassettePath = CassetteLocator.ForTest(test.ClassName!, test.Name);

            var handler = new VcrDelegatingHandler(VcrModes.Current, cassettePath, new HttpClientHandler());
            _httpClient = new HttpClient(handler);
            Maps = new GoogleMapsClient(_httpClient);
        }

        [TearDown]
        public void TearDownVcr()
        {
            _httpClient?.Dispose();
            _httpClient = null;
        }

        protected string ApiKey => VcrModes.Current == VcrMode.Replay
            ? ReplayApiKey
            : AppSettings.Load()?.GoogleApiKey
              ?? Environment.GetEnvironmentVariable(ApiKeyEnvironmentVariable)
              ?? throw new InvalidOperationException($"API key is not configured. Please set the {ApiKeyEnvironmentVariable} environment variable.");
    }
}
