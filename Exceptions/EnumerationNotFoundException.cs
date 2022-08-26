using System.Runtime.Serialization;

namespace TheOmenDen.Shared.Exceptions;
/// <summary>
/// Exception thrown when an <see cref="EnumerationBase{TEnumKey, TEnumValue}"/> cannot be found
/// </summary>
[Serializable]
public class EnumerationNotFoundException : Exception
{
    public EnumerationNotFoundException()
    {
    }

    public EnumerationNotFoundException(string? message) : base(message)
    {
    }

    public EnumerationNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EnumerationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
