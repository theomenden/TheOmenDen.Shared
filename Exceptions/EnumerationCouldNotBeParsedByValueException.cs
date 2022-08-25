
using System.Runtime.Serialization;

namespace TheOmenDen.Shared.Exceptions;
[Serializable]
public class EnumerationCouldNotBeParsedByValueException : Exception
{
    public EnumerationCouldNotBeParsedByValueException()
    {
    }

    public EnumerationCouldNotBeParsedByValueException(string? message) : base(message)
    {
    }

    public EnumerationCouldNotBeParsedByValueException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EnumerationCouldNotBeParsedByValueException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
