using GoogleMapsApi.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.Roads
{
    public class BaseRoadsRequest : SignableRequest
    {
        public override Uri GetUri()
        {
            string scheme = IsSSL ? "https://" : "http://";

            var queryString = GetQueryStringParameters().GetQueryStringPostfix();
            return new Uri(scheme + BaseUrl + "?" + queryString);
        }
    }
}
