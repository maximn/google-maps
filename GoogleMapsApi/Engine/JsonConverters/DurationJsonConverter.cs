using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using DirectionsDuration = GoogleMapsApi.Entities.Directions.Response.Duration;
using DistanceMatrixDuration = GoogleMapsApi.Entities.DistanceMatrix.Response.Duration;

namespace GoogleMapsApi.Engine.JsonConverters
{
    /// <summary>
    /// Base JSON converter for Duration entities that maps the API's seconds-based "value" field to a
    /// <see cref="TimeSpan"/>. Member access is delegated to typed subclasses so the conversion is
    /// compile-time safe (no reflection).
    /// </summary>
    internal abstract class DurationJsonConverterBase<T> : JsonConverter<T> where T : class, new()
    {
        protected abstract void SetValue(T duration, TimeSpan value);
        protected abstract void SetText(T duration, string text);
        protected abstract TimeSpan GetValue(T duration);
        protected abstract string? GetText(T duration);

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException($"Expected StartObject, got {reader.TokenType}");

            var duration = new T();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    continue;

                var propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "value" when reader.TokenType == JsonTokenType.Number:
                        SetValue(duration, TimeSpan.FromSeconds(reader.GetInt32()));
                        break;
                    case "text" when reader.TokenType == JsonTokenType.String:
                        var text = reader.GetString();
                        if (text != null)
                            SetText(duration, text);
                        break;
                    default:
                        reader.Skip();
                        break;
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

            writer.WriteNumber("value", (int)Math.Round(GetValue(value).TotalSeconds));

            var text = GetText(value);
            if (text != null)
                writer.WriteString("text", text);

            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// JSON converter for the Directions API <see cref="DirectionsDuration"/> entity.
    /// </summary>
    internal sealed class DirectionsDurationJsonConverter : DurationJsonConverterBase<DirectionsDuration>
    {
        protected override void SetValue(DirectionsDuration duration, TimeSpan value) => duration.Value = value;
        protected override void SetText(DirectionsDuration duration, string text) => duration.Text = text;
        protected override TimeSpan GetValue(DirectionsDuration duration) => duration.Value;
        protected override string? GetText(DirectionsDuration duration) => duration.Text;
    }

    /// <summary>
    /// JSON converter for the Distance Matrix API <see cref="DistanceMatrixDuration"/> entity.
    /// </summary>
    internal sealed class DistanceMatrixDurationJsonConverter : DurationJsonConverterBase<DistanceMatrixDuration>
    {
        protected override void SetValue(DistanceMatrixDuration duration, TimeSpan value) => duration.Value = value;
        protected override void SetText(DistanceMatrixDuration duration, string text) => duration.Text = text;
        protected override TimeSpan GetValue(DistanceMatrixDuration duration) => duration.Value;
        protected override string? GetText(DistanceMatrixDuration duration) => duration.Text;
    }
}
