using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Linq;

namespace GoogleMapsApi.Entities.Common
{
    /// <summary>
    /// Base class for all Google Maps Web Service requests. Provides API key, SSL, and URL composition.
    /// </summary>
    public abstract class MapsBaseRequest
    {
        /// <summary>
        /// Initializes a new request with no API key.
        /// </summary>
        public MapsBaseRequest()
        {
            ApiKey = null;
        }


        /// <summary>
        /// Always <c>true</c>. The Google Maps Web Services are served over HTTPS only, so this is no
        /// longer a choice. Assigning <c>false</c> throws rather than silently downgrading the request.
        /// </summary>
        [Obsolete("All Google Maps Web Services requests use HTTPS. This property is ignored and will be removed in 3.0.")]
        public virtual bool IsSSL
        {
            get { return true; }
            set
            {
                if (!value)
                    throw new NotSupportedException("Plain HTTP is not supported; all requests use HTTPS.");
            }
        }


        /// <summary>
        /// Base URL (without scheme) for the Google Maps Web Service endpoint this request targets.
        /// Derived requests append their service-specific path.
        /// </summary>
        protected internal virtual string BaseUrl
        {
            get
            {
                return "maps.googleapis.com/maps/api/";
            }
        }

        /// <summary>
        /// Your application's API key. This key identifies your application for purposes of quota management and so that Places 
        /// added from your application are made immediately available to your app. Visit the APIs Console to create an API Project and obtain your key.
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// Builds the list of query-string parameters that will be sent with the request.
        /// Derived classes override to contribute their service-specific parameters.
        /// </summary>
        /// <returns>The parameters to include in the request URL.</returns>
        protected virtual QueryStringParametersList GetQueryStringParameters()
        {
            QueryStringParametersList parametersList = new QueryStringParametersList();

            if (!string.IsNullOrWhiteSpace(ApiKey))
            {
                parametersList.Add("key", ApiKey);
            }

            return parametersList;
        }

        /// <summary>
        /// Builds the absolute request URI, including scheme, base URL, and serialized query-string parameters.
        /// </summary>
        /// <returns>The fully composed URI to send to the Google Maps API.</returns>
        public virtual Uri GetUri()
        {
            var queryString = GetQueryStringParameters().GetQueryStringPostfix();
            return new Uri("https://" + BaseUrl + "json?" + queryString);
        }

        /// <summary>
        /// Builds the body to send with this request, or <c>null</c> for endpoints that use GET with
        /// query-string parameters only. Derived requests that target POST-based endpoints (for example,
        /// Address Validation) override this to return a JSON <see cref="HttpContent"/>; when the engine
        /// sees a non-null body it issues a POST instead of a GET.
        /// </summary>
        /// <returns>The HTTP content to send, or <c>null</c> for a GET request.</returns>
        protected internal virtual HttpContent? GetRequestBody() => null;

        internal MapsBaseRequest CloneShallow() => (MapsBaseRequest)MemberwiseClone();
    }
}
