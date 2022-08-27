using TheOmenDen.Shared.Exceptions.Templates;

namespace TheOmenDen.Shared.Enumerations.Serialization;
/// <summary>
/// <inheritdoc cref="JsonConverter{T}"/>
/// </summary>
/// <typeparam name="TKey">The key/name of the <see cref="EnumerationBaseFlag{TEnumKey, TEnumValue}"/></typeparam>
/// <typeparam name="TValue">The value for the Enumeration</typeparam>
public sealed class EnumerationFlagNameConverter<TKey, TValue> : JsonConverter<TKey>
where TKey : EnumerationBaseFlag<TKey, TValue>
where TValue : struct, IComparable<TValue>, IEquatable<TValue>
{
    public override bool HandleNull => true;

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
        var (result, enumeration) = EnumerationBaseFlag<TKey, TValue>.TryParse(name);

        if (result)
        {
            return enumeration;
        }

        var message = String.Format(Messages.CouldNotConvertProvidedType, nameof(name), name, typeof(TKey).Name);

        throw new JsonException(message);
    }
}
