using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Engine.JsonConverters
{
    /// <summary>
    /// Factory for creating EnumMember JSON converters
    /// </summary>
    public class EnumMemberJsonConverterFactory : JsonConverterFactory
    {
#if NET5_0_OR_GREATER
        // Dictionary to hold converters for each enum type, necessary for AOT compatibility
        private static readonly Dictionary<Type, Func<JsonConverter>> Converters = new()
        {
            { typeof(GoogleMapsApi.Entities.Common.LocationType), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.Common.LocationType>() },

            { typeof(GoogleMapsApi.Entities.Directions.Request.AvoidWay), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.Directions.Request.AvoidWay>() },
            { typeof(GoogleMapsApi.Entities.Directions.Request.TravelMode), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.Directions.Request.TravelMode>() },
            { typeof(GoogleMapsApi.Entities.Directions.Response.DirectionsStatusCodes), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.Directions.Response.DirectionsStatusCodes>() },
            { typeof(GoogleMapsApi.Entities.Directions.Response.VehicleType), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.Directions.Response.VehicleType>() },

            { typeof(GoogleMapsApi.Entities.DistanceMatrix.Request.DistanceMatrixRestrictions), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.DistanceMatrix.Request.DistanceMatrixRestrictions>() },
            { typeof(GoogleMapsApi.Entities.DistanceMatrix.Request.DistanceMatrixTrafficModels), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.DistanceMatrix.Request.DistanceMatrixTrafficModels>() },
            { typeof(GoogleMapsApi.Entities.DistanceMatrix.Request.DistanceMatrixTransitModes), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.DistanceMatrix.Request.DistanceMatrixTransitModes>() },
            { typeof(GoogleMapsApi.Entities.DistanceMatrix.Request.DistanceMatrixTransitRoutingPreferences), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.DistanceMatrix.Request.DistanceMatrixTransitRoutingPreferences>() },
            { typeof(GoogleMapsApi.Entities.DistanceMatrix.Request.DistanceMatrixTravelModes), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.DistanceMatrix.Request.DistanceMatrixTravelModes>() },
            { typeof(GoogleMapsApi.Entities.DistanceMatrix.Request.DistanceMatrixUnitSystems), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.DistanceMatrix.Request.DistanceMatrixUnitSystems>() },
            { typeof(GoogleMapsApi.Entities.DistanceMatrix.Response.DistanceMatrixElementStatusCodes), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.DistanceMatrix.Response.DistanceMatrixElementStatusCodes>() },
            { typeof(GoogleMapsApi.Entities.DistanceMatrix.Response.DistanceMatrixStatusCodes), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.DistanceMatrix.Response.DistanceMatrixStatusCodes>() },

            { typeof(GoogleMapsApi.Entities.Elevation.Response.Status), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.Elevation.Response.Status>() },

            { typeof(GoogleMapsApi.Entities.Geocoding.Response.GeocodeLocationType), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.Geocoding.Response.GeocodeLocationType>() },
            { typeof(GoogleMapsApi.Entities.Geocoding.Response.Status), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.Geocoding.Response.Status>() },

            { typeof(GoogleMapsApi.Entities.PlaceAutocomplete.Response.Status), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.PlaceAutocomplete.Response.Status>() },

            { typeof(GoogleMapsApi.Entities.PlacesDetails.Response.BusinessStatus), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.PlacesDetails.Response.BusinessStatus>() },
            { typeof(GoogleMapsApi.Entities.PlacesDetails.Response.PriceLevel), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.PlacesDetails.Response.PriceLevel>() },
            { typeof(GoogleMapsApi.Entities.PlacesDetails.Response.Status), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.PlacesDetails.Response.Status>() },

            { typeof(GoogleMapsApi.Entities.PlacesFind.Request.InputType), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.PlacesFind.Request.InputType>() },
            { typeof(GoogleMapsApi.Entities.PlacesFind.Response.Status), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.PlacesFind.Response.Status>() },

            { typeof(GoogleMapsApi.Entities.PlacesNearBy.Request.RankBy), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.PlacesNearBy.Request.RankBy>() },
            { typeof(GoogleMapsApi.Entities.PlacesNearBy.Response.Status), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.PlacesNearBy.Response.Status>() },

            { typeof(GoogleMapsApi.Entities.PlacesRadar.Response.Status), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.PlacesRadar.Response.Status>() },
            { typeof(GoogleMapsApi.Entities.PlacesText.Response.Status), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.PlacesText.Response.Status>() },

            { typeof(GoogleMapsApi.Entities.Places.Request.RankBy), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.Places.Request.RankBy>() },
            { typeof(GoogleMapsApi.Entities.Places.Response.Status), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.Places.Response.Status>() },

            { typeof(GoogleMapsApi.Entities.TimeZone.Response.Status), static () => new EnumMemberJsonConverter<GoogleMapsApi.Entities.TimeZone.Response.Status>() },

            { typeof(GoogleMapsApi.StaticMaps.Enums.ImageFormat), static () => new EnumMemberJsonConverter<GoogleMapsApi.StaticMaps.Enums.ImageFormat>() },
            { typeof(GoogleMapsApi.StaticMaps.Enums.MapElement), static () => new EnumMemberJsonConverter<GoogleMapsApi.StaticMaps.Enums.MapElement>() },
            { typeof(GoogleMapsApi.StaticMaps.Enums.MapElementType), static () => new EnumMemberJsonConverter<GoogleMapsApi.StaticMaps.Enums.MapElementType>() },
            { typeof(GoogleMapsApi.StaticMaps.Enums.MapFeature), static () => new EnumMemberJsonConverter<GoogleMapsApi.StaticMaps.Enums.MapFeature>() },
            { typeof(GoogleMapsApi.StaticMaps.Enums.MapFeatureType), static () => new EnumMemberJsonConverter<GoogleMapsApi.StaticMaps.Enums.MapFeatureType>() },
            { typeof(GoogleMapsApi.StaticMaps.Enums.MapType), static () => new EnumMemberJsonConverter<GoogleMapsApi.StaticMaps.Enums.MapType>() },
            { typeof(GoogleMapsApi.StaticMaps.Enums.MapVisibility), static () => new EnumMemberJsonConverter<GoogleMapsApi.StaticMaps.Enums.MapVisibility>() },
            { typeof(GoogleMapsApi.StaticMaps.Enums.MarkerSize), static () => new EnumMemberJsonConverter<GoogleMapsApi.StaticMaps.Enums.MarkerSize>() },
        };
#endif

        public override bool CanConvert(Type typeToConvert)
        {
#if NET5_0_OR_GREATER
            return Converters.ContainsKey(typeToConvert);
#else
            return typeToConvert.IsEnum;
#endif
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
#if NET5_0_OR_GREATER
            if (Converters.TryGetValue(typeToConvert, out var factory))
            {
                return factory();
            }

            throw new NotSupportedException(
                $"AOT does not support reflection-based dynamic converter creation for '{typeToConvert}'. " +
                "Register this enum explicitly in EnumMemberJsonConverterFactory.");
#else
            var converterType = typeof(EnumMemberJsonConverter<>).MakeGenericType(typeToConvert);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
#endif
        }
    }
}