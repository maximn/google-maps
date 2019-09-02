using System.Collections.Generic;
using System.Linq;

namespace GoogleMapsApi.Entities.Geocoding.Request
{
    public class GeocodingComponents
    {
        private readonly Dictionary<string, string> components = new Dictionary<string, string>();

        /// <summary>
        /// matches long or short name of a route.
        /// </summary>
        public string Route { get => components[RouteComponent];
            set => components[RouteComponent] = value;
        }
        /// <summary>
        /// matches against both locality and sublocality types.
        /// </summary>
        public string Locality { get => components[LocalityComponent];
            set => components[LocalityComponent] = value;
        }
        /// <summary>
        /// matches all the administrative_area levels.
        /// </summary>
        public string AdministrativeArea { get => components[AdministrativeAreaComponent];
            set => components[AdministrativeAreaComponent] = value;
        }
        /// <summary>
        /// matches postal_code and postal_code_prefix.
        /// </summary>
        public string PostalCode { get => components[PostalCodeComponent];
            set => components[PostalCodeComponent] = value;
        }
        /// <summary>
        /// matches a country name or a two letter ISO 3166-1 country code.
        /// </summary>
        public string Country { get => components[CountryComponent];
            set => components[CountryComponent] = value;
        }
        public bool Exists => components.Count > 0;

        public override string ToString()
        {
            return Build();
        }
        public string Build()
        {
            return string.Join("|", components.Select(x => x.Key + ":" + x.Value));
        }

        private const string RouteComponent = "route";
        private const string LocalityComponent = "locality";
        private const string AdministrativeAreaComponent = "administrative_area";
        private const string PostalCodeComponent = "postal_code";
        private const string CountryComponent = "country";
    }
}
