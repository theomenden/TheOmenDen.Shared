using System.Runtime.Serialization;

namespace TheOmenDen.Shared.Exceptions;

public sealed class GuardException : Exception
{
    public GuardException()
    {
    }

    public GuardException(string? message) : base(message)
    {
    }

    public GuardException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public GuardException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
