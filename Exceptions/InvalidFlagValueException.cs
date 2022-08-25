using System.Runtime.Serialization;
namespace TheOmenDen.Shared.Exceptions;

[Serializable]
public class InvalidFlagValueException : Exception
{
    public InvalidFlagValueException()
    {
    }

    public InvalidFlagValueException(string? message) : base(message)
    {
    }

    public InvalidFlagValueException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidFlagValueException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
