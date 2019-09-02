using System.Configuration;
using NUnit.Framework;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace GoogleMapsApi.Test.IntegrationTests
{
    //  Note:	The integration tests run against the real Google API web
    //			servers and count towards your query limit. Also, the tests
    //			require a working internet connection in order to pass.
    //			Their run time may vary depending on your connection,
    //			network congestion and the current load on Google's servers.

    public class BaseTestIntegration
    {
        private string _apiKey;

        protected string ApiKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_apiKey))
                {
                    Assert.Inconclusive("API key not specified, please set it in the 'app.config' file");
                }

                return _apiKey;
            }
            private set => _apiKey = value;
        }


        protected BaseTestIntegration()
        {
            var fromConfigManager = ConfigurationManager.AppSettings["ApiKey"];
            var fromEnvironmentVariables = Environment.GetEnvironmentVariable("ApiKey");

            if (!string.IsNullOrEmpty(fromConfigManager))
            {
                ApiKey = fromConfigManager;
            }
            else if (!string.IsNullOrEmpty(fromEnvironmentVariables))
            {
                ApiKey = fromEnvironmentVariables;
            }
            else
            {
                // TODO: Add default api key
                ApiKey = "";
            }
        }
    }
}