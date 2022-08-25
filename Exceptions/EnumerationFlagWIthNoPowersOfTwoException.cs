using System.Runtime.Serialization;

namespace TheOmenDen.Shared.Exceptions;

[Serializable]
public class EnumerationFlagWIthNoPowersOfTwoException : Exception
{
    public EnumerationFlagWIthNoPowersOfTwoException()
    {
    }

    public EnumerationFlagWIthNoPowersOfTwoException(string? message) : base(message)
    {
    }

    public EnumerationFlagWIthNoPowersOfTwoException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EnumerationFlagWIthNoPowersOfTwoException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
