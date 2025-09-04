using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsApi.Engine.JsonConverters
{
    /// <summary>
    /// JSON converter for Duration entities that handles TimeSpan to seconds conversion
    /// </summary>
    public class DurationJsonConverter<T> : JsonConverter<T> where T : class, new()
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException($"Expected StartObject, got {reader.TokenType}");

            var duration = new T();
            var valueProperty = typeToConvert.GetProperty("Value");
            var textProperty = typeToConvert.GetProperty("Text");

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "value":
                            if (reader.TokenType == JsonTokenType.Number && valueProperty != null)
                            {
                                var seconds = reader.GetInt32();
                                valueProperty.SetValue(duration, TimeSpan.FromSeconds(seconds));
                            }
                            break;
                        case "text":
                            if (reader.TokenType == JsonTokenType.String && textProperty != null)
                            {
                                textProperty.SetValue(duration, reader.GetString());
                            }
                            break;
                    }
                }
            }

            return duration;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();

            var valueProperty = typeof(T).GetProperty("Value");
            var textProperty = typeof(T).GetProperty("Text");

            if (valueProperty != null)
            {
                var timeSpan = (TimeSpan?)valueProperty.GetValue(value);
                if (timeSpan.HasValue)
                {
                    writer.WriteNumber("value", (int)Math.Round(timeSpan.Value.TotalSeconds));
                }
            }

            if (textProperty != null)
            {
                var text = (string)textProperty.GetValue(value);
                if (text != null)
                {
                    writer.WriteString("text", text);
                }
            }

            writer.WriteEndObject();
        }
    }
}