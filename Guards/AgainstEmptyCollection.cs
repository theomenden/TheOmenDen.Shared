using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    /// <summary>
    /// Evaluates if the <paramref name="collectionToCheck"/> contains any members, throws an exception if not.
    /// </summary>
    /// <typeparam name="T">The type we're protecting</typeparam>
    /// <param name="collectionToCheck">The provided collection</param>
    /// <param name="parameterName">The name of the collection</param>
    public static void FromEmptyCollection<T>(IEnumerable<T> collectionToCheck, String parameterName) 
        => FromEmptyCollection(collectionToCheck, parameterName, null);

    /// <summary>
    /// Evaluates if the <paramref name="collectionToCheck"/> contains any members, throws an exception with the provided <paramref name="message"/> if not.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collectionToCheck">The provided collection</param>
    /// <param name="parameterName">The name of the collection</param>
    /// <param name="message">The custom message we aim to return to the caller with the exception</param>
    public static void FromEmptyCollection<T>(IEnumerable<T> collectionToCheck, String parameterName, String message)
    {
        if (String.IsNullOrWhiteSpace(parameterName))
        {
            parameterName = Messages.BaseParameterName;
        }

        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.EmptyCollectionTemplate, parameterName);
        }

        var argumentException = new ArgumentException(message, parameterName);

        FromEmptyCollection(collectionToCheck, argumentException);
    }

    /// <summary>
    /// Evaluates if the <paramref name="collectionToCheck"/> contains any members, throws an exception if not.
    /// </summary>
    /// <typeparam name="T">The type we're protecting</typeparam>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="collectionToCheck">The provided collection</param>
    public static void FromEmptyCollection<T, TException>(IEnumerable<T> collectionToCheck)
        where TException : Exception, new()
    {
        var message = String.Format(Messages.EmptyCollectionTemplate, Messages.BaseParameterName);

        FromEmptyCollection<T, TException>(collectionToCheck, message);
    }

    /// <summary>
    /// Evaluates if the <paramref name="collectionToCheck"/> contains any members, throws an exception with the provided <paramref name="message"/> if not.
    /// </summary>
    /// <typeparam name="T">The type we're protecting</typeparam>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="collectionToCheck">The provided collection</param>
    /// <param name="message">The custom message we aim to return to the caller with the exception</param>
    public static void FromEmptyCollection<T, TException>(IEnumerable<T> collectionToCheck, String message)
    where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.EmptyCollectionTemplate, Messages.BaseParameterName);
        }

        var exception = CreateException<TException>(message);

        FromEmptyCollection(collectionToCheck, exception);
    }

    /// <summary>
    /// Evaluates if the <paramref name="collectionToCheck"/> contains any members, thows <paramref name="exception"/> if not.
    /// </summary>
    /// <typeparam name="T">The type we're protecting</typeparam>
    /// <typeparam name="TException">The type of exception we aim to throw</typeparam>
    /// <param name="collectionToCheck">The provided collection</param>
    /// <param name="exception">The exception we aim to throw when the condition is not met</param>
    /// <exception cref="ArgumentNullException">Thrown when the supplied <paramref name="collectionToCheck"/> is <see langword="null"/> or when the <paramref name="exception"/> provided is <see langword="null"/></exception>
    public static void FromEmptyCollection<T, TException>(IEnumerable<T> collectionToCheck, TException exception)
        where TException : Exception, new()
    {
        if (collectionToCheck is null)
        {
            throw new ArgumentNullException(nameof(collectionToCheck));
        }

        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(collectionToCheck.Any, exception);
    }
}
