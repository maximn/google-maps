using System;
using GoogleMapsApi.StaticMaps.Enums;

namespace GoogleMapsApi.StaticMaps.Entities
{
    /// <summary>
    /// Builder for individual style rules with method chaining
    /// </summary>
    public class MapStyleRuleBuilder
    {
        private readonly MapStyleRule _rule;
        private readonly MapStyleBuilder _parentBuilder;

        internal MapStyleRuleBuilder(MapStyleRule rule, MapStyleBuilder parentBuilder)
        {
            _rule = rule;
            _parentBuilder = parentBuilder;
        }

        /// <summary>
        /// Sets the color for this style rule
        /// </summary>
        /// <param name="color">Color in hex format (e.g., "#ff0000")</param>
        /// <returns>This builder for method chaining</returns>
        public MapStyleRuleBuilder WithColor(string color)
        {
            _rule.Stylers.Add(new MapStyleStyler { Color = color });
            return this;
        }

        /// <summary>
        /// Sets the visibility for this style rule
        /// </summary>
        /// <param name="visibility">Visibility setting</param>
        /// <returns>This builder for method chaining</returns>
        public MapStyleRuleBuilder WithVisibility(MapVisibility visibility)
        {
            string visibilityString = ConvertVisibilityToString(visibility);
            _rule.Stylers.Add(new MapStyleStyler { Visibility = visibilityString });
            return this;
        }

        /// <summary>
        /// Sets the lightness for this style rule
        /// </summary>
        /// <param name="lightness">Lightness value (-100 to 100)</param>
        /// <returns>This builder for method chaining</returns>
        public MapStyleRuleBuilder WithLightness(float lightness)
        {
            _rule.Stylers.Add(new MapStyleStyler { Lightness = lightness });
            return this;
        }

        /// <summary>
        /// Sets the saturation for this style rule
        /// </summary>
        /// <param name="saturation">Saturation value (-100 to 100)</param>
        /// <returns>This builder for method chaining</returns>
        public MapStyleRuleBuilder WithSaturation(float saturation)
        {
            _rule.Stylers.Add(new MapStyleStyler { Saturation = saturation });
            return this;
        }

        /// <summary>
        /// Sets the gamma for this style rule
        /// </summary>
        /// <param name="gamma">Gamma value (0.01 to 10.0)</param>
        /// <returns>This builder for method chaining</returns>
        public MapStyleRuleBuilder WithGamma(float gamma)
        {
            _rule.Stylers.Add(new MapStyleStyler { Gamma = gamma });
            return this;
        }

        /// <summary>
        /// Sets the hue for this style rule
        /// </summary>
        /// <param name="hue">Hue value in hex format (e.g., "#ff0000")</param>
        /// <returns>This builder for method chaining</returns>
        public MapStyleRuleBuilder WithHue(string hue)
        {
            _rule.Stylers.Add(new MapStyleStyler { Hue = hue });
            return this;
        }

        /// <summary>
        /// Sets the weight for this style rule
        /// </summary>
        /// <param name="weight">Weight value (0 to 10)</param>
        /// <returns>This builder for method chaining</returns>
        public MapStyleRuleBuilder WithWeight(int weight)
        {
            _rule.Stylers.Add(new MapStyleStyler { Weight = weight });
            return this;
        }

        /// <summary>
        /// Adds multiple stylers to this rule
        /// </summary>
        /// <param name="stylers">Array of styler actions</param>
        /// <returns>This builder for method chaining</returns>
        public MapStyleRuleBuilder WithStylers(params Action<MapStyleStyler>[] stylers)
        {
            foreach (var stylerAction in stylers)
            {
                var styler = new MapStyleStyler();
                stylerAction(styler);
                _rule.Stylers.Add(styler);
            }
            return this;
        }

        /// <summary>
        /// Completes this style rule and returns to the parent builder
        /// </summary>
        /// <returns>The parent MapStyleBuilder for adding more styles</returns>
        public MapStyleBuilder And()
        {
            return _parentBuilder;
        }

        /// <summary>
        /// Completes this style rule and builds the final result
        /// </summary>
        /// <returns>List of MapStyleRule objects</returns>
        public System.Collections.Generic.List<MapStyleRule> Build()
        {
            return _parentBuilder.Build();
        }

        /// <summary>
        /// Converts MapVisibility enum to string representation
        /// </summary>
        private static string ConvertVisibilityToString(MapVisibility visibility)
        {
            switch (visibility)
            {
                case MapVisibility.On:
                    return "on";
                case MapVisibility.Off:
                    return "off";
                case MapVisibility.Simplified:
                    return "simplified";
                default:
                    return "on";
            }
        }
    }
}
