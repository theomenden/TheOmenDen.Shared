using TheOmenDen.Shared.Enumerations.Attributes;
using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    public static void FromInvalidInput<T>(T parameter, String parameterName)
    {
        var attribute = (AllowNegativeEnumerationKeysAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(AllowNegativeEnumerationKeysAttribute));

        if (attribute is not null)
        {
            return;
        }

        if (String.IsNullOrWhiteSpace(parameterName))
        {
            parameterName = Messages.BaseParameterName;
        }

        var message = String.Format(Messages.NegativeValueTemplate, parameterName);

        var argumentOutOfRangeException = new ArgumentOutOfRangeException(parameterName, message);

        FromInvalidInput(parameter, argumentOutOfRangeException);
    }

    public static void FromInvalidInput<T, TException>(T parameter, String message)
    where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.InvalidValueTemplate, Messages.BaseParameterName);
        }

        var exception = CreateException<TException>(message);

        FromInvalidInput(parameter, exception);
    }

    public static void FromInvalidInput<T, TException>(T parameter, TException exception)
    where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => Int32.TryParse(parameter?.ToString(), out _), exception);
    }
}

