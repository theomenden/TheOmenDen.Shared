
using System.Text.Json;

namespace TheOmenDen.Shared.Enumerations.Serialization;
public sealed class EnumerationNameConverter<TKey, TValue> : JsonConverter<TKey>
    where TKey : EnumerationBase<TKey, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>, IConvertible
{
    public override TKey? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? GetKeyFromName(reader.GetString())
            : default(TKey) ?? throw new JsonException($"Unexpected token {reader.TokenType} discovered when attempting to parse {typeof(TKey).Name}");
    }

    public override void Write(Utf8JsonWriter writer, TKey value, JsonSerializerOptions options)
    {
        if(value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Name.ToString());
    }

    private static TKey GetKeyFromName(String name)
    {
        try
        {
            return EnumerationBase<TKey, TValue>.Parse(name);
        }
        catch (Exception ex)
        {
            const string message = @"Could not convert provided {0}: '{1}' to '{2}'";

            throw new JsonException(String.Format(message, nameof(name), name, typeof(TKey).Name) ,ex);
        }
    }
}

