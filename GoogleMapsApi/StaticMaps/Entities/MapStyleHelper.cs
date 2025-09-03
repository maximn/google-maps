using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoogleMapsApi.StaticMaps.Entities
{
    /// <summary>
    /// Helper class for working with Google Styling Wizard JSON and MapStyleRule objects
    /// </summary>
    public static class MapStyleHelper
    {
        /// <summary>
        /// Creates a list of MapStyleRule objects from Google Styling Wizard JSON
        /// </summary>
        /// <param name="json">The JSON string from Google Styling Wizard</param>
        /// <returns>List of MapStyleRule objects</returns>
        public static List<MapStyleRule> FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("JSON cannot be null or empty", nameof(json));

            try
            {
                var jsonArray = JArray.Parse(json);
                return FromJsonArray(jsonArray);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("Invalid JSON format", nameof(json), ex);
            }
        }

        /// <summary>
        /// Creates a list of MapStyleRule objects from Google Styling Wizard JSON array
        /// </summary>
        /// <param name="jsonArray">The JSON array from Google Styling Wizard</param>
        /// <returns>List of MapStyleRule objects</returns>
        public static List<MapStyleRule> FromJsonArray(JArray jsonArray)
        {
            var rules = new List<MapStyleRule>();

            foreach (var element in jsonArray)
            {
                var rule = new MapStyleRule();

                // Parse elementType
                if (element["elementType"] != null)
                {
                    rule.ElementType = element["elementType"].Value<string>();
                }

                // Parse featureType
                if (element["featureType"] != null)
                {
                    rule.FeatureType = element["featureType"].Value<string>();
                }

                // Parse stylers
                if (element["stylers"] is JArray stylersArray)
                {
                    foreach (var stylerElement in stylersArray)
                    {
                        var styler = new MapStyleStyler();

                        if (stylerElement["color"] != null)
                        {
                            styler.Color = stylerElement["color"].Value<string>();
                        }

                        if (stylerElement["visibility"] != null)
                        {
                            styler.Visibility = stylerElement["visibility"].Value<string>();
                        }

                        if (stylerElement["lightness"] != null)
                        {
                            styler.Lightness = stylerElement["lightness"].Value<float?>();
                        }

                        if (stylerElement["saturation"] != null)
                        {
                            styler.Saturation = stylerElement["saturation"].Value<float?>();
                        }

                        if (stylerElement["gamma"] != null)
                        {
                            styler.Gamma = stylerElement["gamma"].Value<float?>();
                        }

                        if (stylerElement["hue"] != null)
                        {
                            styler.Hue = stylerElement["hue"].Value<string>();
                        }

                        if (stylerElement["weight"] != null)
                        {
                            styler.Weight = stylerElement["weight"].Value<int?>();
                        }

                        rule.Stylers.Add(styler);
                    }
                }

                rules.Add(rule);
            }

            return rules;
        }
    }
}
