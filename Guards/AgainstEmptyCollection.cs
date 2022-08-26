using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collectionToCheck"></param>
    /// <param name="parameterName"></param>
    public static void FromEmptyCollection<T>(IEnumerable<T> collectionToCheck, String parameterName)
    {
        FromEmptyCollection(collectionToCheck, parameterName, null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collectionToCheck"></param>
    /// <param name="parameterName"></param>
    /// <param name="message"></param>
    public static void FromEmptyCollection<T>(IEnumerable<T> collectionToCheck, String parameterName, String message)
    {
        if(String.IsNullOrWhiteSpace(parameterName))
        {
            parameterName = Messages.BaseParameterName;
        }

        if(String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.EmptyCollectionTemplate, parameterName);
        }

        var argumentException = new ArgumentException(message, parameterName);

        FromEmptyCollection(collectionToCheck, argumentException);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException"></typeparam>
    /// <param name="collectionToCheck"></param>
    public static void FromEmptyCollection<T, TException>(IEnumerable<T> collectionToCheck)
        where TException : Exception, new()
    {
        var message = String.Format(Messages.EmptyCollectionTemplate, Messages.BaseParameterName);

        FromEmptyCollection<T, TException>(collectionToCheck, message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException"></typeparam>
    /// <param name="collectionToCheck"></param>
    /// <param name="message"></param>
    public static void FromEmptyCollection<T, TException>(IEnumerable<T> collectionToCheck, String message)
    where TException : Exception, new()
    {
        if(String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.EmptyCollectionTemplate, Messages.BaseParameterName);
        }

        var exception = CreateException<TException>(message);

        FromEmptyCollection(collectionToCheck, exception);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException"></typeparam>
    /// <param name="collectionToCheck"></param>
    /// <param name="exception"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void FromEmptyCollection<T, TException>(IEnumerable<T> collectionToCheck, TException exception)
        where TException: Exception, new()
    {
        if(collectionToCheck is null)
        {
            throw new ArgumentNullException(nameof(collectionToCheck));
        }

        if(exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(collectionToCheck.Any, exception);
    }
}
