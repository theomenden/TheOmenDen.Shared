using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;
public static partial class Guard
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    public static void FromInequality<T, TException>(T parameter, T value)
        where TException : Exception, new()
    {
        FromInequality<T, TException>(parameter, value, Messages.NotEqualToTemplate);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    /// <param name="message"></param>
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
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    /// <param name="exception"></param>
    /// <exception cref="ArgumentNullException"></exception>
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

