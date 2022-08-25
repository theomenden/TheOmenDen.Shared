using System.Runtime.Serialization;

namespace TheOmenDen.Shared.Exceptions;
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
