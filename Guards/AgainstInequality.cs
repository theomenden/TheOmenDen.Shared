using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;
public static partial class Guard
{
    /// <summary>
    /// Checks if the provided <paramref name="parameter"/> satisfies an equality condition against the provided <paramref name="value"/>
    /// </summary>
    /// <typeparam name="T">The type we're protecting</typeparam>
    /// <typeparam name="TException">The custom exception we are throwing</typeparam>
    /// <param name="parameter">The provided entity</param>
    /// <param name="value">The value we want to test equality with</param>
    public static void FromInequality<T, TException>(T parameter, T value)
        where TException : Exception, new()
    => FromInequality<T, TException>(parameter, value, Messages.NotEqualToTemplate);
    

    /// <summary>
    /// Checks if the provided <paramref name="parameter"/> satisfies an equality condition against the provided <paramref name="value"/>
    /// </summary>
    /// <typeparam name="T">The type we're protecting</typeparam>
    /// <typeparam name="TException">The custom exception we are throwing</typeparam>
    /// <param name="parameter">The provided entity</param>
    /// <param name="value">The value we want to test equality with</param>
    /// <param name="message">The custom message we aim to return to the caller with the <typeparamref name="TException"/></param>
    public static void FromInequality<T, TException>(T parameter, T value, String message)
    where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = Messages.NotEqualToTemplate;
        }

        var exception = CreateException<TException>(message);

        FromInequality(parameter, value, exception);
    }

    /// <summary>
    /// Checks if the provided <paramref name="parameter"/> satisfies an equality condition against the provided <paramref name="value"/>
    /// </summary>
    /// <typeparam name="T">The type we're protecting</typeparam>
    /// <typeparam name="TException">The custom exception we are throwing</typeparam>
    /// <param name="parameter">The provided entity</param>
    /// <param name="value">The value we want to test equality with</param>
    /// <param name="exception">The custom exception we aim to throw</param>
    /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="exception"/> is null</exception>
    public static void FromInequality<T, TException>(T parameter, T value, TException exception)
        where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        var comparer = EqualityComparer<T>.Default;

        FromCondition(() => comparer.Equals(parameter, value), exception);
    }
}

