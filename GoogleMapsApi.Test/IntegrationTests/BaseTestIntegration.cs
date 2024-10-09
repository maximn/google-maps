﻿using Microsoft.Extensions.Configuration;
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
        private readonly IConfigurationRoot Configuration;

        public BaseTestIntegration()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();
        }

        protected string ApiKey => Configuration.GetValue<string>(ApiKeyEnvironmentVariable) ?? throw new InvalidOperationException($"API key is not configured. Please set the {ApiKeyEnvironmentVariable} environment variable.");
    }
}
