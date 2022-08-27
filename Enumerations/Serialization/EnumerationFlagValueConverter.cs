using TheOmenDen.Shared.Exceptions.Templates;

namespace TheOmenDen.Shared.Enumerations.Serialization;
/// <summary>
/// <inheritdoc cref="JsonConverter{T}"/>
/// </summary>
/// <typeparam name="TKey">The key/name of the <see cref="EnumerationBaseFlag{TEnumKey, TEnumValue}"/></typeparam>
/// <typeparam name="TValue">The value for the Enumeration</typeparam>
public sealed class EnumerationFlagValueConverter<TKey, TValue> : JsonConverter<TKey>
where TKey : EnumerationBaseFlag<TKey, TValue>
where TValue : struct, IEquatable<TValue>, IComparable<TValue>
{
    public override bool HandleNull => true;

    public override TKey? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    => reader.TokenType == JsonTokenType.Null
        ? null
        : GetFromValue(ReadValue(ref reader));

    public override void Write(Utf8JsonWriter writer, TKey value, JsonSerializerOptions options)
    {
        if (!Enum.TryParse<TypeCode>(Type.GetTypeCode(typeof(TValue)).ToString(), true, out var code))
        {
            code = TypeCode.String;
        }

        WriterTypeCodeActions[code](value, writer);
    }
    #region Private Static Members
    private static TKey GetFromValue(TValue value)
    {
        try
        {
            return EnumerationBaseFlag<TKey, TValue>.DeserializeValue(value);
        }
        catch (Exception innerException)
        {
            var message = String.Format(Messages.EnumerationNotFoundByValue, typeof(TKey).Name, value);

            throw new JsonException(message, innerException);
        }
    }

    private static readonly Dictionary<TypeCode, Action<TKey, Utf8JsonWriter>> WriterTypeCodeActions = new()
    {
        {TypeCode.Empty, (_ , writer) => writer.WriteNullValue() },
        {TypeCode.Boolean, (value, writer) => writer.WriteBooleanValue(Convert.ToBoolean(value.Value.ToString())) },
        {TypeCode.Int16, (value, writer) => writer.WriteNumberValue(Convert.ToInt16(value.Value.ToString()))},
        {TypeCode.UInt16, (value, writer) => writer.WriteNumberValue(Convert.ToUInt16(value.Value.ToString()))},
        {TypeCode.Int32, (value, writer) => writer.WriteNumberValue(Convert.ToInt32(value.Value.ToString())) },
        {TypeCode.UInt32, (value, writer) => writer.WriteNumberValue(Convert.ToUInt32(value.Value.ToString()))},
        {TypeCode.Int64, (value, writer) => writer.WriteNumberValue(Convert.ToInt64(value.Value.ToString()))},
        {TypeCode.UInt64, (value, writer) => writer.WriteNumberValue(Convert.ToUInt64(value.Value.ToString()))},
        {TypeCode.Single, (value, writer) => writer.WriteNumberValue(Convert.ToSingle(value.Value.ToString()))},
        {TypeCode.Double, (value, writer) => writer.WriteNumberValue(Convert.ToDouble(value.Value.ToString()))},
        {TypeCode.Decimal, (value, writer) => writer.WriteNumberValue(Convert.ToDecimal(value.Value.ToString()))},
        {TypeCode.String, (value, writer) => writer.WriteStringValue(value.Value.ToString())}
    };

    private static TValue ReadValue(ref Utf8JsonReader reader)
        => (TValue)((object)(Type.GetTypeCode(typeof(TValue))  switch 
        {
            TypeCode.Boolean => reader.GetBoolean(),
            TypeCode.SByte => reader.GetSByte(),
            TypeCode.Byte => reader.GetByte(),
            TypeCode.Int16 => reader.GetInt16(),
            TypeCode.UInt16 => reader.GetUInt16(),
            TypeCode.Int32 => reader.GetInt32(),
            TypeCode.UInt32 => reader.GetUInt32(),
            TypeCode.Int64 => reader.GetInt64(),
            TypeCode.UInt64 => reader.GetUInt64(),
            TypeCode.Single => reader.GetSingle(),
            TypeCode.Double => reader.GetDouble(),
            TypeCode.Decimal => reader.GetDecimal(),
            TypeCode.String => reader.GetString(),
            _ => throw new ArgumentOutOfRangeException(typeof(TValue).ToString(), $"{typeof(TValue).Name} is not supported.")
        }))!;
    #endregion
}

