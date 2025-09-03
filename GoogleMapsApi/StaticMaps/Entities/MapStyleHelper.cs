using System;
using System.Collections.Generic;
using System.Text.Json;

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
                using var document = JsonDocument.Parse(json);
                if (document.RootElement.ValueKind != JsonValueKind.Array)
                    throw new ArgumentException("JSON must be an array", nameof(json));
                
                return FromJsonArray(document.RootElement);
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
        public static List<MapStyleRule> FromJsonArray(JsonElement jsonArray)
        {
            var rules = new List<MapStyleRule>();

            foreach (var element in jsonArray.EnumerateArray())
            {
                var rule = new MapStyleRule();

                // Parse elementType
                if (element.TryGetProperty("elementType", out var elementTypeProperty))
                {
                    rule.ElementType = elementTypeProperty.GetString();
                }

                // Parse featureType
                if (element.TryGetProperty("featureType", out var featureTypeProperty))
                {
                    rule.FeatureType = featureTypeProperty.GetString();
                }

                // Parse stylers
                if (element.TryGetProperty("stylers", out var stylersProperty) && stylersProperty.ValueKind == JsonValueKind.Array)
                {
                    foreach (var stylerElement in stylersProperty.EnumerateArray())
                    {
                        var styler = new MapStyleStyler();

                        if (stylerElement.TryGetProperty("color", out var colorProperty))
                        {
                            styler.Color = colorProperty.GetString();
                        }

                        if (stylerElement.TryGetProperty("visibility", out var visibilityProperty))
                        {
                            styler.Visibility = visibilityProperty.GetString();
                        }

                        if (stylerElement.TryGetProperty("lightness", out var lightnessProperty))
                        {
                            styler.Lightness = lightnessProperty.ValueKind == JsonValueKind.Number ? lightnessProperty.GetSingle() : null;
                        }

                        if (stylerElement.TryGetProperty("saturation", out var saturationProperty))
                        {
                            styler.Saturation = saturationProperty.ValueKind == JsonValueKind.Number ? saturationProperty.GetSingle() : null;
                        }

                        if (stylerElement.TryGetProperty("gamma", out var gammaProperty))
                        {
                            styler.Gamma = gammaProperty.ValueKind == JsonValueKind.Number ? gammaProperty.GetSingle() : null;
                        }

                        if (stylerElement.TryGetProperty("hue", out var hueProperty))
                        {
                            styler.Hue = hueProperty.GetString();
                        }

                        if (stylerElement.TryGetProperty("weight", out var weightProperty))
                        {
                            styler.Weight = weightProperty.ValueKind == JsonValueKind.Number ? weightProperty.GetInt32() : null;
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
