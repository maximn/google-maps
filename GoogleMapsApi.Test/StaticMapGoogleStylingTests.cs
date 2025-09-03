using System;
using System.Collections.Generic;
using NUnit.Framework;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using GoogleMapsApi.StaticMaps.Enums;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Test
{
    /// <summary>
    /// Tests for Google Styling Wizard functionality (GitHub issue #141)
    /// </summary>
    [TestFixture]
    public class StaticMapGoogleStylingTests
    {
        [Test]
        public void TestGoogleStylingJsonParsing()
        {
            // Test JSON from GitHub issue #141
            string testJson = @"[
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

            // Test JSON parsing
            var styles = MapStyleHelper.FromJson(testJson);
            
            Assert.That(styles.Count, Is.EqualTo(2), "Should parse 2 style rules");

            // Verify first rule
            var firstRule = styles[0];
            Assert.That(firstRule.ElementType, Is.EqualTo("geometry"));
            Assert.That(firstRule.Stylers.Count, Is.EqualTo(1));
            Assert.That(firstRule.Stylers[0].Color, Is.EqualTo("#f5f5f5"));

            // Verify second rule
            var secondRule = styles[1];
            Assert.That(secondRule.ElementType, Is.EqualTo("labels.icon"));
            Assert.That(secondRule.Stylers.Count, Is.EqualTo(1));
            Assert.That(secondRule.Stylers[0].Visibility, Is.EqualTo("off"));
        }

        [Test]
        public void TestStaticMapRequestWithGoogleStyling()
        {
            // Test JSON from GitHub issue #141
            string testJson = @"[
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

            var styles = MapStyleHelper.FromJson(testJson);

            var request = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(800, 600))
            {
                ApiKey = "test_key",
                Styles = styles
            };

            var staticMapsEngine = new StaticMapsEngine();
            string mapUrl = staticMapsEngine.GenerateStaticMapURL(request);

            // Debug output
            Console.WriteLine($"Generated URL: {mapUrl}");

            // Verify the URL contains the expected style parameters (URL-encoded)
            Assert.That(mapUrl.Contains("style=element%3Ageometry%7Ccolor%3A%23f5f5f5"), Is.True, 
                "URL should contain geometry style");
            Assert.That(mapUrl.Contains("style=element%3Alabels.icon%7Cvisibility%3Aoff"), Is.True, 
                "URL should contain labels.icon style");
            Assert.That(mapUrl.Contains("key=test_key"), Is.True, 
                "URL should contain API key");
        }

        [Test]
        public void TestBuilderStyleCreation()
        {
            // Create styles using the fluent API
            var styles = MapStyleBuilder.Create()
                .AddElementStyle(MapElementType.Geometry)
                    .WithColor("#f5f5f5")
                    .And()
                .AddElementStyle(MapElementType.LabelsIcon)
                    .WithVisibility(MapVisibility.Off)
                    .Build();

            var request = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(800, 600))
            {
                ApiKey = "test_key",
                Styles = styles
            };

            var staticMapsEngine = new StaticMapsEngine();
            string mapUrl = staticMapsEngine.GenerateStaticMapURL(request);

            // Debug output
            Console.WriteLine($"Generated URL: {mapUrl}");

            // Verify the URL contains the expected style parameters (URL-encoded)
            Assert.That(mapUrl.Contains("style=element%3Ageometry%7Ccolor%3A%23f5f5f5"), Is.True, 
                "URL should contain geometry style");
            Assert.That(mapUrl.Contains("style=element%3Alabels.icon%7Cvisibility%3Aoff"), Is.True, 
                "URL should contain labels.icon style");
        }

        [Test]
        public void TestInvalidJsonThrowsException()
        {
            string invalidJson = "invalid json";

            Assert.Throws<ArgumentException>(() => MapStyleHelper.FromJson(invalidJson));
        }

        [Test]
        public void TestEmptyJsonThrowsException()
        {
            Assert.Throws<ArgumentException>(() => MapStyleHelper.FromJson(""));
            Assert.Throws<ArgumentException>(() => MapStyleHelper.FromJson(null));
        }

        [Test]
        public void TestColorFormatHandling()
        {
            // Test both # and 0x color formats
            var stylesWithHashColor = new List<MapStyleRule>
            {
                new MapStyleRule
                {
                    ElementType = "geometry",
                    Stylers = new List<MapStyleStyler>
                    {
                        new MapStyleStyler { Color = "#f5f5f5" }
                    }
                }
            };

            var stylesWithHexColor = new List<MapStyleRule>
            {
                new MapStyleRule
                {
                    ElementType = "geometry", 
                    Stylers = new List<MapStyleStyler>
                    {
                        new MapStyleStyler { Color = "0xf5f5f5" }
                    }
                }
            };

            var requestWithHash = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(800, 600))
            {
                ApiKey = "test_key",
                Styles = stylesWithHashColor
            };

            var requestWithHex = new StaticMapRequest(new Location(40.714728, -73.998672), 12, new ImageSize(800, 600))
            {
                ApiKey = "test_key",
                Styles = stylesWithHexColor
            };

            var staticMapsEngine = new StaticMapsEngine();
            string hashUrl = staticMapsEngine.GenerateStaticMapURL(requestWithHash);
            string hexUrl = staticMapsEngine.GenerateStaticMapURL(requestWithHex);

            Console.WriteLine($"Hash color URL: {hashUrl}");
            Console.WriteLine($"Hex color URL: {hexUrl}");

            // Both should work - verify URL contains color parameter
            Assert.That(hashUrl.Contains("color%3A%23f5f5f5") || hashUrl.Contains("color:#f5f5f5"), Is.True, 
                "Hash format color should be present in URL");
            Assert.That(hexUrl.Contains("color%3A0xf5f5f5") || hexUrl.Contains("color:0xf5f5f5"), Is.True, 
                "Hex format color should be present in URL");
        }

        [Test]
        public void TestFluentApiStyleCreation()
        {
            // Test the new fluent API
            var styles = MapStyleBuilder.Create()
                .AddStyle(MapFeatureType.Water, MapElementType.Geometry)
                    .WithColor("#e9e9e9")
                    .WithLightness(17)
                    .And()
                .AddStyle(MapFeatureType.Road, MapElementType.Geometry)
                    .WithColor("#ffffff")
                    .WithLightness(16)
                    .And()
                .AddElementStyle(MapElementType.LabelsIcon)
                    .WithVisibility(MapVisibility.Off)
                    .Build();

            Assert.That(styles.Count, Is.EqualTo(3), "Should create 3 style rules");

            // Verify first rule (water)
            var waterRule = styles[0];
            Assert.That(waterRule.FeatureType, Is.EqualTo("water"));
            Assert.That(waterRule.ElementType, Is.EqualTo("geometry"));
            Assert.That(waterRule.Stylers.Count, Is.EqualTo(2));
            Assert.That(waterRule.Stylers[0].Color, Is.EqualTo("#e9e9e9"));
            Assert.That(waterRule.Stylers[1].Lightness, Is.EqualTo(17));

            // Verify second rule (road)
            var roadRule = styles[1];
            Assert.That(roadRule.FeatureType, Is.EqualTo("road"));
            Assert.That(roadRule.ElementType, Is.EqualTo("geometry"));
            Assert.That(roadRule.Stylers.Count, Is.EqualTo(2));
            Assert.That(roadRule.Stylers[0].Color, Is.EqualTo("#ffffff"));
            Assert.That(roadRule.Stylers[1].Lightness, Is.EqualTo(16));

            // Verify third rule (labels.icon)
            var labelsRule = styles[2];
            Assert.That(labelsRule.FeatureType, Is.Null);
            Assert.That(labelsRule.ElementType, Is.EqualTo("labels.icon"));
            Assert.That(labelsRule.Stylers.Count, Is.EqualTo(1));
            Assert.That(labelsRule.Stylers[0].Visibility, Is.EqualTo("off"));
        }

        [Test]
        public void TestLandscapeFeatureTypes()
        {
            // Test the new landscape feature types
            var styles = MapStyleBuilder.Create()
                .AddStyle(MapFeatureType.Landscape, MapElementType.Geometry)
                    .WithColor("#f0f0f0")
                    .And()
                .AddStyle(MapFeatureType.LandscapeManMade, MapElementType.Geometry)
                    .WithColor("#e0e0e0")
                    .And()
                .AddStyle(MapFeatureType.LandscapeNatural, MapElementType.Geometry)
                    .WithColor("#d0d0d0")
                    .And()
                .AddStyle(MapFeatureType.LandscapeNaturalLandcover, MapElementType.GeometryFill)
                    .WithColor("#c0c0c0")
                    .And()
                .AddStyle(MapFeatureType.LandscapeNaturalTerrain, MapElementType.GeometryStroke)
                    .WithColor("#b0b0b0")
                    .Build();

            Assert.That(styles.Count, Is.EqualTo(5), "Should create 5 landscape style rules");

            // Verify landscape feature types are correctly mapped
            Assert.That(styles[0].FeatureType, Is.EqualTo("landscape"));
            Assert.That(styles[1].FeatureType, Is.EqualTo("landscape.man_made"));
            Assert.That(styles[2].FeatureType, Is.EqualTo("landscape.natural"));
            Assert.That(styles[3].FeatureType, Is.EqualTo("landscape.natural.landcover"));
            Assert.That(styles[4].FeatureType, Is.EqualTo("landscape.natural.terrain"));
        }

        [Test]
        public void TestBuilderWithMultipleStylers()
        {
            // Test adding multiple stylers to a single rule
            var styles = MapStyleBuilder.Create()
                .AddStyle(MapFeatureType.Water, MapElementType.Geometry)
                    .WithColor("#1976d2")
                    .WithLightness(25)
                    .WithSaturation(30)
                    .WithGamma(1.2f)
                    .Build();

            Assert.That(styles.Count, Is.EqualTo(1));
            
            var rule = styles[0];
            Assert.That(rule.Stylers.Count, Is.EqualTo(4), "Should have 4 stylers");
            Assert.That(rule.Stylers[0].Color, Is.EqualTo("#1976d2"));
            Assert.That(rule.Stylers[1].Lightness, Is.EqualTo(25));
            Assert.That(rule.Stylers[2].Saturation, Is.EqualTo(30));
            Assert.That(rule.Stylers[3].Gamma, Is.EqualTo(1.2f));
        }

        [Test]
        public void TestBuilderComparedToJson()
        {
            // GitHub issue #141 JSON example
            string testJson = @"[
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

            // Parse JSON styles
            var jsonStyles = MapStyleHelper.FromJson(testJson);

            // Create equivalent styles using builder
            var builderStyles = MapStyleBuilder.Create()
                .AddElementStyle(MapElementType.Geometry)
                    .WithColor("#f5f5f5")
                    .And()
                .AddElementStyle(MapElementType.LabelsIcon)
                    .WithVisibility(MapVisibility.Off)
                    .Build();

            // Compare results
            Assert.That(builderStyles.Count, Is.EqualTo(jsonStyles.Count));
            
            for (int i = 0; i < jsonStyles.Count; i++)
            {
                Assert.That(builderStyles[i].ElementType, Is.EqualTo(jsonStyles[i].ElementType));
                Assert.That(builderStyles[i].FeatureType, Is.EqualTo(jsonStyles[i].FeatureType));
                Assert.That(builderStyles[i].Stylers.Count, Is.EqualTo(jsonStyles[i].Stylers.Count));
                
                // Compare first styler (both should have only one styler each)
                if (builderStyles[i].Stylers.Count > 0 && jsonStyles[i].Stylers.Count > 0)
                {
                    Assert.That(builderStyles[i].Stylers[0].Color, Is.EqualTo(jsonStyles[i].Stylers[0].Color));
                    Assert.That(builderStyles[i].Stylers[0].Visibility, Is.EqualTo(jsonStyles[i].Stylers[0].Visibility));
                }
            }

            Console.WriteLine("Builder and JSON approaches produce equivalent results");
        }
    }
}
