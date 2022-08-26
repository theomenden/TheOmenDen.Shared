using TheOmenDen.Shared.Enumerations.Attributes;
using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    /// <summary>
    /// Protects entity creation from invalid inputs
    /// </summary>
    /// <typeparam name="T">The type of entity we're checking</typeparam>
    /// <param name="parameter">The parameter we're checking</param>
    /// <param name="parameterName">The name of the <paramref name="parameter"/></param>
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

    /// <summary>
    /// Protects entity creation from invalid inputs
    /// </summary>
    /// <typeparam name="T">The type of entity we're checking</typeparam>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="parameter">The parameter we're checking</param>
    /// <param name="message">The formatted message we want to return to the caller</param>
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

    /// <summary>
    /// Protects entity creation from invalid inputs
    /// </summary>
    /// <typeparam name="T">The type of entity we're checking</typeparam>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="parameter">The parameter we're checking</param>
    /// <param name="exception">The exception we aim to throw</param>
    /// <exception cref="ArgumentNullException">If the <paramref name="exception"/> is null</exception>
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

