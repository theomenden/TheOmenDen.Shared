using System.Text.Json;
using TheOmenDen.Shared.Exceptions.Templates;

namespace TheOmenDen.Shared.Enumerations.Serialization;
/// <summary>
/// <inheritdoc cref="JsonConverter{T}"/>
/// </summary>
/// <typeparam name="TKey">The key/name for the <see cref="EnumerationBase{TEnumKey, TEnumValue}"/></typeparam>
/// <typeparam name="TValue">The value for the <see cref="EnumerationBase{TEnumKey, TEnumValue}"/></typeparam>
public sealed class EnumerationNameConverter<TKey, TValue> : JsonConverter<TKey>
    where TKey : EnumerationBase<TKey, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>, IConvertible
{
    public override TKey? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    => reader.TokenType == JsonTokenType.String
            ? GetKeyFromName(reader.GetString() ?? throw new JsonException($"Unexpected token {reader.TokenType} discovered when attempting to parse {typeof(TKey).Name}"))
            : default;

    public override void Write(Utf8JsonWriter writer, TKey? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Name);
    }

    private static TKey GetKeyFromName(String name)
    {
        var (result, enumeration) = EnumerationBase<TKey, TValue>.TryParse(name, false);

        if (result)
        {
            return enumeration;
        }

        var message = String.Format(Messages.CouldNotConvertProvidedType, nameof(name), name, typeof(TKey).Name);

        throw new JsonException(message);
    }
}

