using GoogleMapsApi.Test.Utils;
using System.IO;

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

        public BaseTestIntegration()
        {
        }

        protected string ApiKey => AppSettings.Load()?.GoogleApiKey
            ?? Environment.GetEnvironmentVariable(ApiKeyEnvironmentVariable) 
            ?? throw new InvalidOperationException($"API key is not configured. Please set the {ApiKeyEnvironmentVariable} environment variable.");
    }
}
