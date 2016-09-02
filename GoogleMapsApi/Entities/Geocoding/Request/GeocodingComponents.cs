using System.Collections.Generic;

namespace GoogleMapsApi.Entities.Geocoding.Request
{
    public class GeocodingComponents
    {
        readonly Dictionary<string, string> components = new Dictionary<string, string>();

        /// <summary>
        /// matches long or short name of a route.
        /// </summary>
        public string Route { get { return components[route]; } set { components[route] = value; } }
        /// <summary>
        /// matches against both locality and sublocality types.
        /// </summary>
        public string Locality { get { return components[locality]; } set { components[locality] = value; } }
        /// <summary>
        /// matches all the administrative_area levels.
        /// </summary>
        public string AdministrativeArea { get { return components[administrative_area]; } set { components[administrative_area] = value; } }
        /// <summary>
        /// matches postal_code and postal_code_prefix.
        /// </summary>
        public string PostalCode { get { return components[postal_code]; } set { components[postal_code] = value; } }
        /// <summary>
        /// matches a country name or a two letter ISO 3166-1 country code.
        /// </summary>
        public string Country { get { return components[country]; } set { components[country] = value; } }
        public bool Exists { get { return components.Count > 0; } }
        public override string ToString()
        {
            return Build();
        }
        public string Build()
        {
            if (Exists)
            {
                string _components = "";
                foreach (var keyValue in this.components)
                {
                    _components += $"{keyValue.Key}:{keyValue.Value}|";
                }
                return _components;
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
