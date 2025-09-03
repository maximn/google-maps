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

                if (int.TryParse(stringValue, out var priceLevelInt))
                    return (PriceLevel)priceLevelInt;
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                var intValue = reader.GetInt32();
                return (PriceLevel)intValue;
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