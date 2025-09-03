using System;
using System.Collections.Generic;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using GoogleMapsApi.StaticMaps.Enums;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Examples
{
    /// <summary>
    /// Example demonstrating how to use the MapStyleBuilder fluent API
    /// This provides a type-safe, class-based approach to creating Google Maps styles
    /// </summary>
    public class MapStyleBuilderExample
    {
        public static void Main()
        {
            // Example 1: Simple styling using the fluent API
            CreateSimpleStyle();

            // Example 2: Complex styling with multiple rules
            CreateComplexStyle();

            // Example 3: Recreating the GitHub issue #141 example
            CreateGitHubIssueExample();

            // Example 4: Creating a dark theme map
            CreateDarkThemeStyle();
        }

        /// <summary>
        /// Example 1: Simple styling - hide labels and change geometry color
        /// </summary>
        public static void CreateSimpleStyle()
        {
            Console.WriteLine("=== Example 1: Simple Style ===");

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

            Console.WriteLine("Generated URL:");
            Console.WriteLine(mapUrl);
            Console.WriteLine();
        }

        /// <summary>
        /// Example 2: Complex styling with multiple feature types and elements
        /// </summary>
        public static void CreateComplexStyle()
        {
            Console.WriteLine("=== Example 2: Complex Style ===");

            var styles = MapStyleBuilder.Create()
                // Style water features
                .AddStyle(MapFeatureType.Water, MapElementType.Geometry)
                    .WithColor("#e9e9e9")
                    .WithLightness(17)
                    .And()
                // Style landscape features
                .AddStyle(MapFeatureType.Road, MapElementType.Geometry)
                    .WithColor("#ffffff")
                    .WithLightness(16)
                    .And()
                // Style highway roads
                .AddStyle(MapFeatureType.RoadHighway, MapElementType.GeometryStroke)
                    .WithColor("#ffffff")
                    .WithLightness(16)
                    .And()
                // Hide POI labels
                .AddStyle(MapFeatureType.Poi, MapElementType.Labels)
                    .WithVisibility(MapVisibility.Off)
                    .And()
                // Style label text
                .AddElementStyle(MapElementType.LabelsTextFill)
                    .WithSaturation(36)
                    .WithColor("#333333")
                    .WithLightness(40)
                    .Build();

            var request = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(800, 600))
            {
                ApiKey = "YOUR_API_KEY_HERE", // Replace with your actual API key
                Styles = styles
            };

            var staticMapsEngine = new StaticMapsEngine();
            string mapUrl = staticMapsEngine.GenerateStaticMapURL(request);

            Console.WriteLine("Generated URL:");
            Console.WriteLine(mapUrl);
            Console.WriteLine();
        }

        /// <summary>
        /// Example 3: Recreating the exact example from GitHub issue #141
        /// </summary>
        public static void CreateGitHubIssueExample()
        {
            Console.WriteLine("=== Example 3: GitHub Issue #141 Example ===");

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

            Console.WriteLine("Generated URL (GitHub Issue #141 example):");
            Console.WriteLine(mapUrl);
            Console.WriteLine();
        }

        /// <summary>
        /// Example 4: Creating a dark theme map
        /// </summary>
        public static void CreateDarkThemeStyle()
        {
            Console.WriteLine("=== Example 4: Dark Theme Style ===");

            var styles = MapStyleBuilder.Create()
                // Dark background
                .AddElementStyle(MapElementType.Geometry)
                    .WithColor("#212121")
                    .And()
                // Style water
                .AddStyle(MapFeatureType.Water, MapElementType.Geometry)
                    .WithColor("#000000")
                    .And()
                // Style roads
                .AddStyle(MapFeatureType.Road, MapElementType.Geometry)
                    .WithColor("#2b2b2b")
                    .And()
                .AddStyle(MapFeatureType.Road, MapElementType.Labels)
                    .WithColor("#757575")
                    .And()
                // Style highways
                .AddStyle(MapFeatureType.RoadHighway, MapElementType.Geometry)
                    .WithColor("#616161")
                    .And()
                .AddStyle(MapFeatureType.RoadHighway, MapElementType.Labels)
                    .WithColor("#ffffff")
                    .And()
                // Style local roads
                .AddStyle(MapFeatureType.RoadLocal, MapElementType.Geometry)
                    .WithColor("#424242")
                    .And()
                // Style POI
                .AddStyle(MapFeatureType.Poi, MapElementType.Geometry)
                    .WithColor("#2b2b2b")
                    .And()
                .AddStyle(MapFeatureType.Poi, MapElementType.Labels)
                    .WithColor("#757575")
                    .And()
                // Style parks
                .AddStyle(MapFeatureType.PoiPark, MapElementType.Geometry)
                    .WithColor("#1b5e20")
                    .And()
                // Style transit
                .AddStyle(MapFeatureType.Transit, MapElementType.Geometry)
                    .WithColor("#2b2b2b")
                    .And()
                .AddStyle(MapFeatureType.Transit, MapElementType.Labels)
                    .WithColor("#757575")
                    .And()
                // Style administrative areas
                .AddStyle(MapFeatureType.Administrative, MapElementType.Geometry)
                    .WithColor("#2b2b2b")
                    .And()
                // Style label text
                .AddElementStyle(MapElementType.LabelsTextFill)
                    .WithColor("#ffffff")
                    .And()
                .AddElementStyle(MapElementType.LabelsTextStroke)
                    .WithColor("#000000")
                    .WithWeight(1)
                    .Build();

            var request = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(800, 600))
            {
                ApiKey = "YOUR_API_KEY_HERE", // Replace with your actual API key
                Styles = styles
            };

            var staticMapsEngine = new StaticMapsEngine();
            string mapUrl = staticMapsEngine.GenerateStaticMapURL(request);

            Console.WriteLine("Generated URL (Dark Theme):");
            Console.WriteLine(mapUrl);
            Console.WriteLine();
        }

        /// <summary>
        /// Example showing how to create styles programmatically with custom logic
        /// </summary>
        public static void CreateProgrammaticStyle()
        {
            Console.WriteLine("=== Example 5: Programmatic Style Creation ===");

            var builder = MapStyleBuilder.Create();

            // Add styles based on some business logic
            var featureTypes = new[] { MapFeatureType.Road, MapFeatureType.Water, MapFeatureType.Poi };
            var colors = new[] { "#ff0000", "#0000ff", "#00ff00" };

            for (int i = 0; i < featureTypes.Length; i++)
            {
                builder.AddStyle(featureTypes[i], MapElementType.Geometry)
                    .WithColor(colors[i])
                    .And();
            }

            // Add a common style for all labels
            builder.AddElementStyle(MapElementType.Labels)
                .WithVisibility(MapVisibility.Simplified);

            var styles = builder.Build();

            var request = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(800, 600))
            {
                ApiKey = "YOUR_API_KEY_HERE", // Replace with your actual API key
                Styles = styles
            };

            var staticMapsEngine = new StaticMapsEngine();
            string mapUrl = staticMapsEngine.GenerateStaticMapURL(request);

            Console.WriteLine("Generated URL (Programmatic):");
            Console.WriteLine(mapUrl);
            Console.WriteLine();
        }
    }
}
