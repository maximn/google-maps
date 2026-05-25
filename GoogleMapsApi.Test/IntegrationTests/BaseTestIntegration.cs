using GoogleMapsApi.Test.Utils;
using System.Net.Http;

namespace GoogleMapsApi.Test.IntegrationTests
{
    //  Note:	The integration tests run against the real Google API web
    //			servers and count towards your query limit. Also, the tests
    //			require a working internet connection in order to pass.
    //			Their run time may vary depending on your connection,
    //			network congestion and the current load on Google's servers.

    public class BaseTestIntegration
    {
        const string ApiKeyEnvironmentVariable = "GOOGLE_API_KEY";

        // Shared across all integration tests in the process. Using a single HttpClient is the
        // documented best practice; the GoogleMapsClient itself is thread-safe.
        private static readonly HttpClient SharedHttpClient = new HttpClient();

        protected string ApiKey => AppSettings.Load()?.GoogleApiKey
            ?? Environment.GetEnvironmentVariable(ApiKeyEnvironmentVariable)
            ?? throw new InvalidOperationException($"API key is not configured. Please set the {ApiKeyEnvironmentVariable} environment variable.");

        private IGoogleMapsClient? _client;

        /// <summary>
        /// Shared <see cref="IGoogleMapsClient"/> for integration tests. Replaces the previously-removed
        /// static <c>GoogleMaps</c> facade. The ambient <c>ApiKey</c> is filled into requests that don't
        /// set one themselves; tests that explicitly set <c>request.ApiKey</c> continue to work unchanged.
        /// </summary>
        protected IGoogleMapsClient Client
            => _client ??= new GoogleMapsClient(SharedHttpClient, new GoogleMapsClientOptions { ApiKey = ApiKey });
    }
}
