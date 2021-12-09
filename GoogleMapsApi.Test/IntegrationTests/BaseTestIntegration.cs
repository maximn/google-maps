using Microsoft.Extensions.Configuration;
using System;
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
        private readonly string Key;

        public BaseTestIntegration()
        {
            Key = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
        }

        protected string ApiKey => this.Key;
    }
}
