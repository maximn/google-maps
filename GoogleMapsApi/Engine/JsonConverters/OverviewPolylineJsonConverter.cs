using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoogleMapsApi.Entities.Directions.Response;

namespace GoogleMapsApi.Engine.JsonConverters
{
    /// <summary>
    /// JSON converter for OverviewPolyline that handles encoded points and lazy initialization
    /// </summary>
    internal class OverviewPolylineJsonConverter : JsonConverter<OverviewPolyline>
    {
        public override OverviewPolyline Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException($"Expected StartObject, got {reader.TokenType}");

            var polyline = new OverviewPolyline();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    continue;

                var propertyName = reader.GetString();
                reader.Read();

                if (propertyName == "points" && reader.TokenType == JsonTokenType.String)
                    polyline.EncodedPoints = reader.GetString();
                else
                    reader.Skip();
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

            if (value.EncodedPoints != null)
                writer.WriteString("points", value.EncodedPoints);

            writer.WriteEndObject();
        }
    }
}
