﻿using System.Runtime.Serialization;

namespace TheOmenDen.Shared.Exceptions;
[Serializable]
public sealed class GuardException : Exception
{
    public GuardException()
    {
    }

    public GuardException(string? message) : base(message)
    {
    }

    private GuardException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public GuardException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
