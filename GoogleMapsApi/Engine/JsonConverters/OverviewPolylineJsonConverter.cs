using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Directions.Response;

namespace GoogleMapsApi.Engine.JsonConverters
{
    /// <summary>
    /// JSON converter for OverviewPolyline that handles encoded points and lazy initialization
    /// </summary>
    public class OverviewPolylineJsonConverter : JsonConverter<OverviewPolyline>
    {
        public override OverviewPolyline Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException($"Expected StartObject, got {reader.TokenType}");

            var polyline = new OverviewPolyline();
            var encodedPointsProperty = typeToConvert.GetProperty("EncodedPoints", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();

                    if (propertyName == "points" && reader.TokenType == JsonTokenType.String)
                    {
                        var encodedPoints = reader.GetString();
                        encodedPointsProperty?.SetValue(polyline, encodedPoints);
                    }
                }
            }

            // Initialize lazy points after deserialization
            polyline.OnDeserialized();
            return polyline;
        }

        public override void Write(Utf8JsonWriter writer, OverviewPolyline value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartObject();

            var encodedPointsProperty = typeof(OverviewPolyline).GetProperty("EncodedPoints",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var encodedPoints = (string?)encodedPointsProperty?.GetValue(value);
            if (encodedPoints != null)
            {
                writer.WriteString("points", encodedPoints);
            }

            writer.WriteEndObject();
        }
    }
}