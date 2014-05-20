using System.Configuration;
using NUnit.Framework;

namespace GoogleMapsApi.Test.IntegrationTests
{
    //  Note:	The tests below run against the real Google API web
    //			servers and count towards your query limit. Also, the tests
    //			require a working internet connection in order to pass.
    //			Their run time may vary depending on your connection,
    //			network congestion and the current load on Google's servers.

    public class BaseTestIntegration
    {
        protected readonly string ApiKey;

        public BaseTestIntegration()
        {
            ApiKey = ConfigurationManager.AppSettings["ApiKey"];
            
        }


    }


}