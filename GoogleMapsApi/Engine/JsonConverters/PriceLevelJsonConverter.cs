using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.PlacesDetails.Response;

namespace GoogleMapsApi.Engine.JsonConverters
{
    /// <summary>
    /// JSON converter for PriceLevel enum that handles string number conversion
    /// </summary>
    public class PriceLevelJsonConverter : JsonConverter<PriceLevel?>
    {
        public override PriceLevel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (string.IsNullOrEmpty(stringValue))
                    return null;

                if (int.TryParse(stringValue, out var priceLevelInt) && Enum.IsDefined(typeof(PriceLevel), priceLevelInt))
                    return (PriceLevel)priceLevelInt;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt32(out var intValue) && Enum.IsDefined(typeof(PriceLevel), intValue))
                {
                    return (PriceLevel)intValue;
                }
                else if (reader.TryGetDouble(out var doubleValue))
                {
                    // Convert floating point to integer (truncate decimal part)
                    var truncatedValue = (int)doubleValue;
                    if (Enum.IsDefined(typeof(PriceLevel), truncatedValue))
                        return (PriceLevel)truncatedValue;
                }
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, PriceLevel? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(((int)value.Value).ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}