using System;
using System.Collections.Generic;
using GoogleMapsApi.StaticMaps.Enums;

namespace GoogleMapsApi.StaticMaps.Entities
{
    /// <summary>
    /// Fluent API builder for creating Google Maps styles
    /// </summary>
    public class MapStyleBuilder
    {
        private readonly List<MapStyleRule> _styles = new List<MapStyleRule>();

        /// <summary>
        /// Creates a new MapStyleBuilder instance
        /// </summary>
        public static MapStyleBuilder Create()
        {
            return new MapStyleBuilder();
        }

        /// <summary>
        /// Adds a new style rule to the builder
        /// </summary>
        /// <param name="featureType">The feature type to style</param>
        /// <param name="elementType">The element type to style</param>
        /// <returns>A MapStyleRuleBuilder for chaining style properties</returns>
        public MapStyleRuleBuilder AddStyle(MapFeatureType featureType = MapFeatureType.All, MapElementType elementType = MapElementType.All)
        {
            var rule = new MapStyleRule
            {
                FeatureType = featureType == MapFeatureType.All ? null : ConvertFeatureTypeToString(featureType),
                ElementType = elementType == MapElementType.All ? null : ConvertElementTypeToString(elementType),
                Stylers = new List<MapStyleStyler>()
            };

            _styles.Add(rule);
            return new MapStyleRuleBuilder(rule, this);
        }

        /// <summary>
        /// Adds a new style rule with only element type (no feature type)
        /// </summary>
        /// <param name="elementType">The element type to style</param>
        /// <returns>A MapStyleRuleBuilder for chaining style properties</returns>
        public MapStyleRuleBuilder AddElementStyle(MapElementType elementType)
        {
            var rule = new MapStyleRule
            {
                ElementType = ConvertElementTypeToString(elementType),
                Stylers = new List<MapStyleStyler>()
            };

            _styles.Add(rule);
            return new MapStyleRuleBuilder(rule, this);
        }

        /// <summary>
        /// Adds a new style rule with only feature type (no element type)
        /// </summary>
        /// <param name="featureType">The feature type to style</param>
        /// <returns>A MapStyleRuleBuilder for chaining style properties</returns>
        public MapStyleRuleBuilder AddFeatureStyle(MapFeatureType featureType)
        {
            var rule = new MapStyleRule
            {
                FeatureType = ConvertFeatureTypeToString(featureType),
                Stylers = new List<MapStyleStyler>()
            };

            _styles.Add(rule);
            return new MapStyleRuleBuilder(rule, this);
        }

        /// <summary>
        /// Builds the final list of MapStyleRule objects
        /// </summary>
        /// <returns>List of MapStyleRule objects</returns>
        public List<MapStyleRule> Build()
        {
            return new List<MapStyleRule>(_styles);
        }

        /// <summary>
        /// Converts MapFeatureType enum to string representation
        /// </summary>
        private static string? ConvertFeatureTypeToString(MapFeatureType featureType)
        {
            switch (featureType)
            {
                case MapFeatureType.Administrative:
                    return "administrative";
                case MapFeatureType.AdministrativeCountry:
                    return "administrative.country";
                case MapFeatureType.AdministrativeLandParcel:
                    return "administrative.land_parcel";
                case MapFeatureType.AdministrativeLocality:
                    return "administrative.locality";
                case MapFeatureType.AdministrativeNeighborhood:
                    return "administrative.neighborhood";
                case MapFeatureType.AdministrativeProvince:
                    return "administrative.province";
                case MapFeatureType.Landscape:
                    return "landscape";
                case MapFeatureType.LandscapeManMade:
                    return "landscape.man_made";
                case MapFeatureType.LandscapeNatural:
                    return "landscape.natural";
                case MapFeatureType.LandscapeNaturalLandcover:
                    return "landscape.natural.landcover";
                case MapFeatureType.LandscapeNaturalTerrain:
                    return "landscape.natural.terrain";
                case MapFeatureType.Poi:
                    return "poi";
                case MapFeatureType.PoiAttraction:
                    return "poi.attraction";
                case MapFeatureType.PoiBusiness:
                    return "poi.business";
                case MapFeatureType.PoiGovernment:
                    return "poi.government";
                case MapFeatureType.PoiMedical:
                    return "poi.medical";
                case MapFeatureType.PoiPark:
                    return "poi.park";
                case MapFeatureType.PoiPlaceOfWorship:
                    return "poi.place_of_worship";
                case MapFeatureType.PoiSchool:
                    return "poi.school";
                case MapFeatureType.PoiSportsComplex:
                    return "poi.sports_complex";
                case MapFeatureType.Road:
                    return "road";
                case MapFeatureType.RoadArterial:
                    return "road.arterial";
                case MapFeatureType.RoadHighway:
                    return "road.highway";
                case MapFeatureType.RoadHighwayControlledAccess:
                    return "road.highway.controlled_access";
                case MapFeatureType.RoadLocal:
                    return "road.local";
                case MapFeatureType.Transit:
                    return "transit";
                case MapFeatureType.TransitLine:
                    return "transit.line";
                case MapFeatureType.TransitStation:
                    return "transit.station";
                case MapFeatureType.TransitStationAirport:
                    return "transit.station.airport";
                case MapFeatureType.TransitStationBus:
                    return "transit.station.bus";
                case MapFeatureType.TransitStationRail:
                    return "transit.station.rail";
                case MapFeatureType.Water:
                    return "water";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Converts MapElementType enum to string representation
        /// </summary>
        private static string? ConvertElementTypeToString(MapElementType elementType)
        {
            switch (elementType)
            {
                case MapElementType.Geometry:
                    return "geometry";
                case MapElementType.GeometryFill:
                    return "geometry.fill";
                case MapElementType.GeometryStroke:
                    return "geometry.stroke";
                case MapElementType.Labels:
                    return "labels";
                case MapElementType.LabelsIcon:
                    return "labels.icon";
                case MapElementType.LabelsText:
                    return "labels.text";
                case MapElementType.LabelsTextFill:
                    return "labels.text.fill";
                case MapElementType.LabelsTextStroke:
                    return "labels.text.stroke";
                default:
                    return null;
            }
        }
    }
}
