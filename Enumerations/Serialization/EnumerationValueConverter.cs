using System.Text.Json;

namespace TheOmenDen.Shared.Enumerations.Serialization;

public class EnumerationValueConverter<TKey, TValue> : JsonConverter<TKey>
    where TKey : EnumerationBase<TKey, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>, IConvertible
{
    #region Overrides
    public override TKey? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.Null
        ? null
        : GetFromValue(ReadValue(ref reader));

    public override void Write(Utf8JsonWriter writer, TKey value, JsonSerializerOptions options)
    {
        if (!Enum.TryParse<TypeCode>(Type.GetTypeCode(typeof(TValue)).ToString(), true, out var code))
        {
            code = TypeCode.String;
        }

        _writerTypeCodeActions[code](value, writer);
    }
    #endregion
    #region Private Static Members
    private static TValue ReadValue(ref Utf8JsonReader reader)
    => (TValue)(Object)(Type.GetTypeCode(typeof(TValue)) switch
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
    });

    private static TKey? GetFromValue(TValue value)
    {
        try
        {
            return EnumerationBase<TKey, TValue>.ParseFromValue(value);
        }
        catch (Exception ex)
        {
            const string message = @"Could not convert {0} {1}: {2} to {3}";

            throw new JsonException(String.Format(message, typeof(TValue).Name, nameof(value), value.ToString(), typeof(TKey).Name), ex);
        }
    }


    private static readonly Dictionary<TypeCode, Action<TKey, Utf8JsonWriter>> _writerTypeCodeActions = new()
    {
        {TypeCode.Empty, (_ , writer) => writer.WriteNullValue() },
        {TypeCode.Boolean, (value, writer) => writer.WriteBooleanValue(Convert.ToBoolean(value.Value)) },
        {TypeCode.Int16, (value, writer) => writer.WriteNumberValue(Convert.ToInt16(value.Value))},
        {TypeCode.UInt16, (value, writer) => writer.WriteNumberValue(Convert.ToUInt16(value.Value))},
        {TypeCode.Int32, (value, writer) => writer.WriteNumberValue(Convert.ToInt32(value.Value)) },
        {TypeCode.UInt32, (value, writer) => writer.WriteNumberValue(Convert.ToUInt32(value.Value))},
        {TypeCode.Int64, (value, writer) => writer.WriteNumberValue(Convert.ToInt64(value.Value))},
        {TypeCode.UInt64, (value, writer) => writer.WriteNumberValue(Convert.ToUInt64(value.Value))},
        {TypeCode.Single, (value, writer) => writer.WriteNumberValue(Convert.ToSingle(value.Value))},
        {TypeCode.Double, (value, writer) => writer.WriteNumberValue(Convert.ToDouble(value.Value))},
        {TypeCode.Decimal, (value, writer) => writer.WriteNumberValue(Convert.ToDecimal(value.Value))},
        {TypeCode.String, (value, writer) => writer.WriteStringValue(value.Value.ToString())}
    };
    #endregion
}
