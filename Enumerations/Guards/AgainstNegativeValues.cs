using TheOmenDen.Shared.Enumerations.Attributes;
using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;
public partial class Guard
{

    /// <summary>
    /// Checks the provided <paramref name="parameter"/> for negative values
    /// </summary>
    /// <typeparam name="T">The parameter type</typeparam>
    /// <param name="parameter">The parameter we are trying to check</param>
    /// <param name="parameterName">The name of the <paramref name="parameter"/></param>
    public static void FromNegativeValues<T>(T parameter, String parameterName)
    where T : IComparable<T>
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

        FromNegativeValues(parameter, argumentOutOfRangeException);
    }


    /// <summary>
    /// Checks the provided <paramref name="parameter"/> for negative values
    /// </summary>
    /// <typeparam name="T">The parameter type</typeparam>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="parameter">The parameter we are trying to check</param>
    /// <param name="message">The formatted message we aim to return to the user</param>
    public static void FromNegativeValues<T, TException>(T parameter, String message)
        where T : IComparable<T>
        where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NegativeValueTemplate, Messages.BaseParameterName);
        }

        var exception = CreateException<TException>(message);

        FromNegativeValues(parameter, exception);
    }

    /// <summary>
    /// Checks the provided <paramref name="parameter"/> for negative values
    /// </summary>
    /// <typeparam name="T">The parameter type</typeparam>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="parameter">The parameter we are trying to check</param>
    /// <param name="exception">The exception that we aim to throw</param>
    /// <exception cref="ArgumentNullException">If the provided exception is null</exception>
    public static void FromNegativeValues<T, TException>(T parameter, TException exception)
        where T : IComparable<T>
        where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => Int32.TryParse(parameter.ToString(), out var number) && number.CompareTo(0) < 0, exception);
    }
}