using System.Collections.Generic;

namespace GoogleMapsApi.Entities.Geocoding.Request
{
    public class GeocodingComponents : Dictionary<string, string>
    {
        /// <summary>
        /// matches long or short name of a route.
        /// </summary>
        public string Route { get { return this[route]; } set { this[route] = value; } }
        /// <summary>
        /// matches against both locality and sublocality types.
        /// </summary>
        public string Locality { get { return this[locality]; } set { this[locality] = value; } }
        /// <summary>
        /// matches all the administrative_area levels.
        /// </summary>
        public string AdministrativeArea { get { return this[administrative_area]; } set { this[administrative_area] = value; } }
        /// <summary>
        /// matches postal_code and postal_code_prefix.
        /// </summary>
        public string PostalCode { get { return this[postal_code]; } set { this[postal_code] = value; } }
        /// <summary>
        /// matches a country name or a two letter ISO 3166-1 country code.
        /// </summary>
        public string Country { get { return this[country]; } set { this[country] = value; } }
        public bool Exists { get { return Count > 0; } }
        public override string ToString()
        {
            return Build();
        }
        public string Build()
        {
            if (Exists)
            {
                string components = "";
                foreach (var keyValue in this)
                {
                    components += $"{keyValue.Key}:{keyValue.Value}|";
                }
                return components;
            }
            return null;
        }

        public const string route = "route";
        public const string locality = "locality";
        public const string administrative_area = "administrative_area";
        public const string postal_code = "postal_code";
        public const string country = "country";
    }
}
