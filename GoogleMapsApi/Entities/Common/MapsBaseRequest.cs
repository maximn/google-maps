using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Linq;

namespace GoogleMapsApi.Entities.Common
{
    public abstract class MapsBaseRequest
    {
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

        public virtual Uri GetUri()
        {
            string scheme = IsSSL ? "https://" : "http://";

            var queryString = GetQueryStringParameters().GetQueryStringPostfix();
            return new Uri(scheme + BaseUrl + "json?" + queryString);
        }
    }
}
