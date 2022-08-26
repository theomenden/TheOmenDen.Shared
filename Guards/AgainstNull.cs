using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    /// <summary>
    /// Checks if the <paramref name="parameter"/> is null, and throws an exception if it is.
    /// </summary>
    /// <typeparam name="T">The type we're attempting to protect</typeparam>
    /// <param name="parameter">The supplied entity</param>
    /// <param name="parameterName">The name of the supplied entity</param>
    public static void FromNull<T>(T? parameter, String parameterName)
    {
        FromNull(parameter, parameterName, null);
    }

    /// <summary>
    /// Checks if the <paramref name="parameter"/> is null, and throws an exception if it is.
    /// </summary>
    /// <typeparam name="T">The type we're attempting to protect</typeparam>
    /// <param name="parameter">The supplied entity</param>
    /// <param name="parameterName">The name of the supplied entity</param>
    /// <param name="message">The custom message we send back to the caller within the exception</param>
    public static void FromNull<T>(T parameter, String parameterName, String message)
    {
        if (String.IsNullOrWhiteSpace(parameterName))
        {
            parameterName = Messages.BaseParameterName;
        }

        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NullValueTemplate, parameterName);
        }

        var argumentNullException = new ArgumentNullException(parameterName, message);

        FromNull(parameter, argumentNullException);
    }

    /// <summary>
    /// Checks if the <paramref name="parameter"/> is null, and throws a(n) <typeparamref name="TException"/> if it is.
    /// </summary>
    /// <typeparam name="T">The type we're attempting to protect</typeparam>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="parameter">The supplied entity</param>
    public static void FromNull<T, TException>(T parameter)
    where TException : Exception, new()
    {
        var message = String.Format(Messages.NullValueTemplate, Messages.BaseParameterName);

        FromNull<T, TException>(parameter, message);
    }

    /// <summary>
    /// Checks if the <paramref name="parameter"/> is null, and throws a(n) <typeparamref name="TException"/> if it is.
    /// </summary>
    /// <typeparam name="T">The type we're attempting to protect</typeparam>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="parameter">The supplied entity</param>
    /// <param name="message">The custom message we send back to the caller within the exception</param>
    public static void FromNull<T, TException>(T parameter, String message)
    where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NullValueTemplate, Messages.BaseParameterName);
        }

        var exception = CreateException<TException>(message);

        FromNull(parameter, exception);
    }

    /// <summary>
    /// Checks if the <paramref name="parameter"/> is null, and throws a(n) <typeparamref name="TException"/> if it is.
    /// </summary>
    /// <typeparam name="T">The type we're attempting to protect</typeparam>
    /// <typeparam name="TException">The type of exception we aim to throw<</typeparam>
    /// <param name="parameter">The supplied entity</param>
    /// <param name="exception">The suppleid custom exception we're throwing</param>
    /// <exception cref="ArgumentNullException">Thrown when the provided <typeparamref name="TException"/> <paramref name="exception"/> is null</exception>
    public static void FromNull<T, TException>(T? parameter, TException exception)
    where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => parameter is null, exception);
    }
}