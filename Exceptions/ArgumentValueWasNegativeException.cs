using System.Runtime.Serialization;

namespace TheOmenDen.Shared.Exceptions;

[Serializable]
public class ArgumentValueWasNegativeException : Exception
{
    public ArgumentValueWasNegativeException()
    {
    }

    public ArgumentValueWasNegativeException(string? message) : base(message)
    {
    }

    public ArgumentValueWasNegativeException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ArgumentValueWasNegativeException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
