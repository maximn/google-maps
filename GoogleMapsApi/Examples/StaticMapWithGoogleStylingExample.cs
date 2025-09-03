using System;
using System.Collections.Generic;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using GoogleMapsApi.StaticMaps.Enums;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Examples
{
    /// <summary>
    /// Example demonstrating how to use Google Styling Wizard JSON with StaticMapRequest
    /// This addresses GitHub issue #141: StaticMapRequest: how to add "style"?
    /// </summary>
    public class StaticMapWithGoogleStylingExample
    {
        public static void Main()
        {
            // Example JSON from Google Styling Wizard (from GitHub issue #141)
            string googleStylingJson = @"[
                {
                    ""elementType"": ""geometry"",
                    ""stylers"": [
                        {
                            ""color"": ""#f5f5f5""
                        }
                    ]
                },
                {
                    ""elementType"": ""labels.icon"",
                    ""stylers"": [
                        {
                            ""visibility"": ""off""
                        }
                    ]
                }
            ]";

            // Create a static map request with the styling
            var request = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(800, 600))
            {
                ApiKey = "YOUR_API_KEY_HERE", // Replace with your actual API key
                Styles = MapStyleHelper.FromJson(googleStylingJson)
            };

            // Generate the static map URL
            var staticMapsEngine = new StaticMapsEngine();
            string mapUrl = staticMapsEngine.GenerateStaticMapURL(request);

            Console.WriteLine("Generated Static Map URL with Google Styling:");
            Console.WriteLine(mapUrl);
            Console.WriteLine();
            Console.WriteLine("This URL can be used in an img tag or opened in a browser to see the styled map.");
        }

        /// <summary>
        /// Alternative example showing how to create styles programmatically using raw objects
        /// </summary>
        public static void CreateStylesProgrammatically()
        {
            // Create styles programmatically instead of from JSON
            var styles = new List<MapStyleRule>
            {
                new MapStyleRule
                {
                    ElementType = "geometry",
                    Stylers = new List<MapStyleStyler>
                    {
                        new MapStyleStyler { Color = "#f5f5f5" }
                    }
                },
                new MapStyleRule
                {
                    ElementType = "labels.icon",
                    Stylers = new List<MapStyleStyler>
                    {
                        new MapStyleStyler { Visibility = "off" }
                    }
                }
            };

            var request = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(800, 600))
            {
                ApiKey = "YOUR_API_KEY_HERE", // Replace with your actual API key
                Styles = styles
            };

            var staticMapsEngine = new StaticMapsEngine();
            string mapUrl = staticMapsEngine.GenerateStaticMapURL(request);

            Console.WriteLine("Generated Static Map URL with programmatic styles:");
            Console.WriteLine(mapUrl);
        }

        /// <summary>
        /// Example showing how to use the MapStyleBuilder fluent API (recommended approach)
        /// </summary>
        public static void CreateStylesUsingBuilder()
        {
            Console.WriteLine("\n=== Example using MapStyleBuilder (Fluent API) ===");

            // Create the same styles using the fluent builder API
            var styles = MapStyleBuilder.Create()
                .AddElementStyle(MapElementType.Geometry)
                    .WithColor("#f5f5f5")
                    .And()
                .AddElementStyle(MapElementType.LabelsIcon)
                    .WithVisibility(MapVisibility.Off)
                    .Build();

            var request = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(800, 600))
            {
                ApiKey = "YOUR_API_KEY_HERE", // Replace with your actual API key
                Styles = styles
            };

            var staticMapsEngine = new StaticMapsEngine();
            string mapUrl = staticMapsEngine.GenerateStaticMapURL(request);

            Console.WriteLine("Generated Static Map URL with builder pattern:");
            Console.WriteLine(mapUrl);
            Console.WriteLine("\nThis approach provides type safety and IntelliSense support.");
        }

        /// <summary>
        /// Advanced example showing complex styling with the builder
        /// </summary>
        public static void CreateComplexStylesUsingBuilder()
        {
            Console.WriteLine("\n=== Advanced Builder Example ===");

            var styles = MapStyleBuilder.Create()
                // Style water features
                .AddStyle(MapFeatureType.Water, MapElementType.GeometryFill)
                    .WithColor("#1976d2")
                    .WithLightness(25)
                    .And()
                // Style roads 
                .AddStyle(MapFeatureType.Road, MapElementType.Geometry)
                    .WithColor("#ffffff")
                    .WithLightness(16)
                    .And()
                // Style highways with specific color
                .AddStyle(MapFeatureType.RoadHighway, MapElementType.GeometryStroke)
                    .WithColor("#ffb74d")
                    .WithWeight(3)
                    .And()
                // Hide POI icons but keep text
                .AddStyle(MapFeatureType.Poi, MapElementType.LabelsIcon)
                    .WithVisibility(MapVisibility.Off)
                    .And()
                // Style administrative areas
                .AddStyle(MapFeatureType.Administrative, MapElementType.LabelsText)
                    .WithColor("#616161")
                    .WithSaturation(-50)
                    .Build();

            var request = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(800, 600))
            {
                ApiKey = "YOUR_API_KEY_HERE", // Replace with your actual API key
                Styles = styles
            };

            var staticMapsEngine = new StaticMapsEngine();
            string mapUrl = staticMapsEngine.GenerateStaticMapURL(request);

            Console.WriteLine("Generated Static Map URL with complex builder styles:");
            Console.WriteLine(mapUrl);
        }
    }
}

