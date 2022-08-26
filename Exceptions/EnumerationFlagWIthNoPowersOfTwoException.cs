using System.Runtime.Serialization;

namespace TheOmenDen.Shared.Exceptions;
/// <summary>
/// An exception thrown when an <see cref="EnumerationBaseFlag{TEnumKey}"/> is attempted to be registered without having a value backing in powers of 2
/// </summary>
/// <remarks>Examples 0,2,4,8,16</remarks>
[Serializable]
public class EnumerationFlagWithNoPowersOfTwoException : Exception
{
    public EnumerationFlagWithNoPowersOfTwoException()
    {
    }

    public EnumerationFlagWithNoPowersOfTwoException(string? message) : base(message)
    {
    }

    public EnumerationFlagWithNoPowersOfTwoException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EnumerationFlagWithNoPowersOfTwoException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
