using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetCore60.Controllers
{

    public class DateOnlyConverter : JsonConverter<System.DateOnly>
    {
        public override System.DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (System.DateOnly.TryParse(reader.GetString(), out var date))
                {
                    return date;
                }
            }

            throw new JsonException("Unable to parse DateOnly value.");
        }

        public override void Write(Utf8JsonWriter writer, System.DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }


}
