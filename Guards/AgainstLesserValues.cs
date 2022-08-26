using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    /// <summary>
    /// Aims to protect the provided <paramref name="parameter"/> from going beneath the given <paramref name="lowerThreshold"/>
    /// </summary>
    /// <typeparam name="T">The type we're aiming to protect</typeparam>
    /// <param name="parameter">The provided entity</param>
    /// <param name="lowerThreshold">The lower threshold for the given entity type</param>
    /// <param name="message">The custom message we aim to return via an exception</param>
    public static void FromValuesLessThan<T>(T parameter, T lowerThreshold, String message)
        where T : IComparable, IComparable<T>
    {
        FromValuesLessThan(parameter, lowerThreshold, message, null);
    }

    /// <summary>
    /// Aims to protect the provided <paramref name="parameter"/> from going beneath the given <paramref name="lowerThreshold"/>
    /// </summary>
    /// <typeparam name="T">The type we're aiming to protect</typeparam>
    /// <param name="parameter">The provided entity</param>
    /// <param name="lowerThreshold">The lower threshold for the given entity type</param>
    /// <param name="parameterName"></param>
    /// <param name="message">The custom message we aim to return via an exception</param>
    public static void FromValuesLessThan<T>(T parameter, T lowerThreshold, String parameterName, String message)
        where T : IComparable, IComparable<T>
    {
        if (String.IsNullOrWhiteSpace(parameterName))
        {
            parameterName = Messages.BaseParameterName;
        }

        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.LessThanValueTemplate, parameterName, lowerThreshold);
        }

        var argumentOutOfRangeException = new ArgumentOutOfRangeException(parameterName, message);

        FromValuesLessThan(parameter, lowerThreshold, argumentOutOfRangeException);
    }

    /// <summary>
    /// Aims to protect the provided <paramref name="parameter"/> from going beneath the given <paramref name="lowerThreshold"/>
    /// </summary>
    /// <typeparam name="T">The type we're aiming to protect</typeparam>
    /// <typeparam name="TException">The type of the exception we're aiming to throw</typeparam>
    /// <param name="parameter">The provided entity</param>
    /// <param name="lowerThreshold">The lower threshold for the given entity type</param>
    /// <param name="message">The custom message we aim to return via an exception</param>
    public static void FromValuesLessThan<T, TException>(T parameter, T lowerThreshold, String message)
    where T: IComparable, IComparable<T>
    where TException: Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.LessThanValueTemplate, Messages.BaseParameterName, lowerThreshold);
        }

        var exception = CreateException<TException>(message);

        FromValuesLessThan(parameter, lowerThreshold, exception);
    }

    /// <summary>
    /// Aims to protect the provided <paramref name="parameter"/> from going beneath the given <paramref name="lowerThreshold"/>
    /// </summary>
    /// <typeparam name="T">The type we're aiming to protect</typeparam>
    /// <typeparam name="TException">The type of the exception we're aiming to throw</typeparam>
    /// <param name="parameter">The provided entity</param>
    /// <param name="lowerThreshold">The lower threshold for the given entity type</param>
    /// <param name="exception">The custom <typeparamref name="TException"/> we're aiming to throw when the condtion fails</param>
    /// <exception cref="ArgumentNullException">Thrown when the provided <typeparamref name="TException"/> is <see langword="null"/></exception>
    public static void FromValuesLessThan<T, TException>(T parameter, T lowerThreshold, TException exception)
    where T: IComparable, IComparable<T>
    where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => parameter.CompareTo(lowerThreshold) < 0, exception);
    }
}