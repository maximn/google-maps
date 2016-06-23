﻿using System.Configuration;
using NUnit.Framework;
using System;

namespace GoogleMapsApi.Test.IntegrationTests
{
    //  Note:	The integration tests run against the real Google API web
    //			servers and count towards your query limit. Also, the tests
    //			require a working internet connection in order to pass.
    //			Their run time may vary depending on your connection,
    //			network congestion and the current load on Google's servers.

    public class BaseTestIntegration
    {
        private string apiKey;

        protected string ApiKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    Assert.Inconclusive("API key not specified, please set it in the 'app.config' file");
                }

                return apiKey;
            }
            private set { apiKey = value; }
        }


        protected BaseTestIntegration()
        {
           var fromConfigManager = ConfigurationManager.AppSettings["ApiKey"];
            var fromEnvironmentVariables = Environment.GetEnvironmentVariable("ApiKey");

            if (!string.IsNullOrEmpty(fromConfigManager))
            {
              ApiKey = fromConfigManager;
            } else if (!string.IsNullOrEmpty(fromEnvironmentVariables))
            {
              ApiKey = fromEnvironmentVariables;
            }
            else 
            {
                ApiKey = string.Empty;
            }
        }
    }
}
