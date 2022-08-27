using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    /// <summary>
    /// Protects the supplied <paramref name="parameter"/> from being <see langword="null"/> or <see cref="String.Empty"/>
    /// </summary>
    /// <param name="parameter">The supplied <see cref="string"/> we're protecting</param>
    /// <param name="parameterName">The <paramref name="parameter"/>'s name</param>
    public static void FromNullOrEmpty(String parameter, String parameterName)
        => FromNullOrEmpty(parameter, parameterName, null);

    /// <summary>
    /// Protects the supplied <paramref name="parameter"/> from being <see langword="null"/> or <see cref="String.Empty"/>
    /// </summary>
    /// <param name="parameter">The supplied <see cref="string"/> we're protecting</param>
    /// <param name="parameterName">The <paramref name="parameter"/>'s name</param>
    /// <param name="message">The custom message we aim to return via an exception</param>
    public static void FromNullOrEmpty(String parameter, String parameterName, String message)
    {
        if(String.IsNullOrWhiteSpace(parameterName))
        {
            parameterName = Messages.BaseParameterName;
        }

        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NotNullOrEmptyTemplate, parameterName);
        }

        var argumentException = new ArgumentException(message, parameterName);

        FromNullOrEmpty(parameter, argumentException);
    }

    /// <summary>
    /// Protects the supplied <paramref name="parameter"/> from being <see langword="null"/> or <see cref="String.Empty"/>
    /// </summary>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="parameter">The supplied <see cref="string"/> we're protecting</param>
    /// <param name="parameterName">The <paramref name="parameter"/>'s name</param>
    public static void FromNullOrEmpty<TException>(String parameter)
    where TException : Exception, new()
    {
        var message = String.Format(Messages.NotNullOrEmptyTemplate, Messages.BaseParameterName);

        FromNullOrEmpty<TException>(parameter, message);
    }

    /// <summary>
    /// Protects the supplied <paramref name="parameter"/> from being <see langword="null"/> or <see cref="String.Empty"/>
    /// </summary>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="parameter">The supplied <see cref="string"/> we're protecting</param>
    /// <param name="message">The custom message we aim to return via an exception</param>
    public static void FromNullOrEmpty<TException>(String parameter, String message)
    where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NotNullOrEmptyTemplate, Messages.BaseParameterName);
        }

        var exception = CreateException<TException>(message);

        FromNullOrEmpty(parameter, exception);
    }

    /// <summary>
    /// Protects the supplied <paramref name="parameter"/> from being <see langword="null"/> or <see cref="String.Empty"/>
    /// </summary>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="parameter">The supplied <see cref="string"/> we're protecting</param>
    /// <param name="exception">The custom exception we are throwing when the string <paramref name="parameter"/> is null or empty</param>
    /// <exception cref="ArgumentNullException">Thrown when the provided <typeparamref name="TException"/> is <see langword="null"/></exception>
    public static void FromNullOrEmpty<TException>(String parameter, TException exception)
        where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => String.IsNullOrEmpty(parameter), exception);
    }
}
