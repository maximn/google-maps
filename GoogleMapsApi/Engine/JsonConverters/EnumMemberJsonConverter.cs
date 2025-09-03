using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Engine.JsonConverters
{
    /// <summary>
    /// JSON converter for enums that respects EnumMember attributes with custom values
    /// </summary>
    public class EnumMemberJsonConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<TEnum, string>> EnumToStringCache 
            = new ConcurrentDictionary<Type, Dictionary<TEnum, string>>();
        
        private static readonly ConcurrentDictionary<Type, Dictionary<string, TEnum>> StringToEnumCache 
            = new ConcurrentDictionary<Type, Dictionary<string, TEnum>>();

        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                var stringToEnum = GetStringToEnumMapping(typeToConvert);

                if (stringToEnum.TryGetValue(stringValue, out var enumValue))
                    return enumValue;

                throw new JsonException($"Unable to convert \"{stringValue}\" to {typeToConvert.Name}");
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                var numericValue = reader.GetInt32();
                if (Enum.IsDefined(typeToConvert, numericValue))
                {
                    return (TEnum)Enum.ToObject(typeToConvert, numericValue);
                }
                throw new JsonException($"Unable to convert {numericValue} to {typeToConvert.Name}");
            }
            else
            {
                throw new JsonException($"Expected String or Number, got {reader.TokenType}");
            }
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            var enumToString = GetEnumToStringMapping(typeof(TEnum));
            
            if (enumToString.TryGetValue(value, out var stringValue))
            {
                writer.WriteStringValue(stringValue);
            }
            else
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        private static Dictionary<TEnum, string> GetEnumToStringMapping(Type enumType)
        {
            return EnumToStringCache.GetOrAdd(enumType, type =>
            {
                var mapping = new Dictionary<TEnum, string>();
                var enumValues = Enum.GetValues(type).Cast<TEnum>();

                foreach (var enumValue in enumValues)
                {
                    var memberInfo = type.GetMember(enumValue.ToString()).FirstOrDefault();
                    var enumMemberAttr = memberInfo?.GetCustomAttribute<EnumMemberAttribute>();
                    
                    var stringValue = enumMemberAttr?.Value ?? enumValue.ToString();
                    mapping[enumValue] = stringValue;
                }

                return mapping;
            });
        }

        private static Dictionary<string, TEnum> GetStringToEnumMapping(Type enumType)
        {
            return StringToEnumCache.GetOrAdd(enumType, type =>
            {
                var mapping = new Dictionary<string, TEnum>();
                var enumValues = Enum.GetValues(type).Cast<TEnum>();

                foreach (var enumValue in enumValues)
                {
                    var memberInfo = type.GetMember(enumValue.ToString()).FirstOrDefault();
                    var enumMemberAttr = memberInfo?.GetCustomAttribute<EnumMemberAttribute>();
                    
                    var stringValue = enumMemberAttr?.Value ?? enumValue.ToString();
                    mapping[stringValue] = enumValue;
                }

                return mapping;
            });
        }
    }

    /// <summary>
    /// Factory for creating EnumMember JSON converters
    /// </summary>
    public class EnumMemberJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(EnumMemberJsonConverter<>).MakeGenericType(typeToConvert);
            return (JsonConverter)Activator.CreateInstance(converterType);
        }
    }
}