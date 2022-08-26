using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    /// <summary>
    /// Evaluates the supplied <paramref name="predicate"/> to guard against
    /// </summary>
    /// <typeparam name="TException">The type of exception that we're aiming to throw</typeparam>
    /// <param name="predicate">The condition to be executed</param>
    public static void FromCondition<TException>(Func<bool> predicate)
        where TException : Exception, new()
    {
        FromCondition<TException>(predicate, Messages.PreconditionTemplate);
    }

    /// <summary>
    /// Evaluates the supplied <paramref name="predicate"/> to guard against
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="predicate">The condition to be executed</param>
    /// <param name="message">The custom message we aim to provide with the guard statement</param>
    public static void FromCondition<TException>(Func<bool> predicate, String message)
        where TException : Exception, new()
    {
        if(String.IsNullOrWhiteSpace(message))
        {
            message = Messages.PreconditionTemplate;
        }

        var exception = CreateException<TException>(message);

        FromCondition(predicate, exception);
    }

    /// <summary>
    /// Evaluates the supplied <paramref name="predicate"/> to guard against, and allows for a custom <typeparamref name="TException"/> to be defined
    /// </summary>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="predicate">The condition to be executed</param>
    /// <param name="exception">The exception we aim to throw</param>
    /// <exception cref="ArgumentNullException">Thrown when either the <paramref name="predicate"/> or <paramref name="exception"/> is <see langword="null"/></exception>
    public static void FromCondition<TException>(Func<bool> predicate, TException exception)
        where TException : Exception, new()
    {
        if(predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        if(exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        if(predicate.Invoke())
        {
            throw exception;
        }
    }

    /// <summary>
    /// Evaluates the supplied <paramref name="predicate"/> to guard against, with a custom <paramref name="message"/>
    /// </summary>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="message">The custom message we aim to include with the exception</param>
    /// <returns>The thrown <typeparamref name="TException"/></returns>
    private static TException CreateException<TException>(String message = null)
        where TException : Exception, new()
    {
        if(String.IsNullOrWhiteSpace(message))
        {
            return new TException();
        }

        try
        {
            return (TException)Activator.CreateInstance(typeof(TException),message);
        }
        catch (MissingMethodException)
        {
            return new TException();
        }
    }
}