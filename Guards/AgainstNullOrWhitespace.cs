using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    /// <summary>
    /// Protects the provided <paramref name="parameter"/> from being <see langword="null"/>, <see cref="String.Empty"/>, or whitespace
    /// </summary>
    /// <param name="parameter">The string we're aiming to protect</param>
    /// <param name="parameterName">The name of the <paramref name="parameterName"/></param>
    public static void FromNullOrWhitespace(String parameter, String parameterName)
    {
        FromNullOrWhitespace(parameter, parameterName, null);
    }

    /// <summary>
    /// Protects the provided <paramref name="parameter"/> from being <see langword="null"/>, <see cref="String.Empty"/>, or whitespace
    /// </summary>
    /// <param name="parameter">The string we're aiming to protect</param>
    /// <param name="parameterName">The name of the <paramref name="parameterName"/></param>
    /// <param name="message">The message we'll return to the caller via a custom <see cref="Exception"/></param>
    public static void FromNullOrWhitespace(String parameter, String parameterName, String message)
    {
        if (String.IsNullOrWhiteSpace(parameterName))
        {
            parameterName = Messages.BaseParameterName;
        }

        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NotNullOrWhitespaceTemplate, parameterName);
        }

        var argumentException = new ArgumentException(message, parameterName);

        FromNullOrWhitespace(parameter, argumentException);
    }

    /// <summary>
    /// Protects the provided <paramref name="parameter"/> from being <see langword="null"/>, <see cref="String.Empty"/>, or whitespace 
    /// </summary>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="parameter">The string we're aiming to protect</param>
    public static void FromNullOrWhitespace<TException>(String parameter)
        where TException : Exception, new()
    {
        var message = String.Format(Messages.NotNullOrWhitespaceTemplate, Messages.BaseParameterName);

        FromNullOrWhitespace<TException>(parameter, message);
    }

    /// <summary>
    /// Protects the provided <paramref name="parameter"/> from being <see langword="null"/>, <see cref="String.Empty"/>, or whitespace 
    /// </summary>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="parameter">The string we're aiming to protect</param>
    /// <param name="message">The message we'll return to the caller via a custom <typeparamref name="TException"/></param>
    public static void FromNullOrWhitespace<TException>(String parameter, String message)
        where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NotNullOrWhitespaceTemplate, Messages.BaseParameterName);
        }

        var exception = CreateException<TException>(message);

        FromNullOrWhitespace(parameter, exception);
    }

    /// <summary>
    /// Protects the provided <paramref name="parameter"/> from being <see langword="null"/>, <see cref="String.Empty"/>, or whitespace 
    /// </summary>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="parameter">The string we're aiming to protect</param>
    /// <param name="exception">The custom exception we're aiming to throw when the condition is not met</param>
    /// <exception cref="ArgumentNullException">Thrown when the custom <typeparamref name="TException"/> <paramref name="exception"/> is <see langword="null"/></exception>
    public static void FromNullOrWhitespace<TException>(String parameter, TException exception)
        where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        Guard.FromCondition(() => String.IsNullOrWhiteSpace(parameter), exception);
    }
}