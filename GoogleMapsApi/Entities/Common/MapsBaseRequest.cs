using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Linq;

namespace GoogleMapsApi.Entities.Common
{
    /// <summary>
    /// Base class for all Google Maps Web Service requests. Provides API key, SSL, and URL composition.
    /// </summary>
    public abstract class MapsBaseRequest
    {
        /// <summary>
        /// Initializes a new request with SSL enabled and no API key.
        /// </summary>
        public MapsBaseRequest()
        {
            this.isSsl = true;
            ApiKey = null;
        }


        /// <summary>
        /// True to indicate that request comes from a device with a location sensor, otherwise false. 
        /// </summary>
        /// <remarks>
        /// The Google Maps API previously required that you include the sensor parameter to indicate whether your application used a sensor to determine the user's location.
        /// This parameter is no longer required.
        /// See the geocoding API reference at https://developers.google.com/maps/documentation/geocoding/.
        /// </remarks>
        [Obsolete]
        public bool Sensor { get; set; }

        /// <summary>
        /// True to use use the https protocol; false to use http. The default is false.
        /// </summary>
        public virtual bool IsSSL
        {
            get { return isSsl; }
            set { isSsl = value; }
        }
        private bool isSsl;


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
                if (!this.IsSSL)
                {
                    throw new ArgumentException("When using an ApiKey MUST send the request over SSL [IsSSL = true]");
                }
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
            string scheme = IsSSL ? "https://" : "http://";

            var queryString = GetQueryStringParameters().GetQueryStringPostfix();
            return new Uri(scheme + BaseUrl + "json?" + queryString);
        }

        internal MapsBaseRequest CloneShallow() => (MapsBaseRequest)MemberwiseClone();
    }
}
