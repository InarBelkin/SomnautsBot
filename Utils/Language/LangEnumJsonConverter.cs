using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utils.Language;

public class LangEnumJsonConverter : JsonConverter<LangEnum>
{
    public override LangEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                var s = reader.GetString()!;
                return LangEnum.List.FirstOrDefault(l => l.Aliases.Contains(s)) ??
                       throw new JsonException($"Can't recognize language {s}");
            default:
                throw new JsonException($"Unexpected token {reader.TokenType} when parsing a smart enum.");
        }
    }

    public override void Write(Utf8JsonWriter writer, LangEnum? value, JsonSerializerOptions options)
    {
        if (value == null)
            writer.WriteNullValue();
        else
            writer.WriteStringValue(value.Aliases.First());
    }
}