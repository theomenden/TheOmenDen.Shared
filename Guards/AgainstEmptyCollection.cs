using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public partial class Guard: ICanGuard
{
    public static void FromEmptyCollection<T>(IEnumerable<T> collectionToCheck, String parameterName)
    {
        FromEmptyCollection(collectionToCheck, parameterName, null);
    }

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

        FromEmptyCollection(parameterName, argumentException);
    }

    public static void FromEmptyCollection<T, TException>(IEnumerable<T> collectionToCheck)
        where TException : Exception, new()
    {
        var message = String.Format(Messages.EmptyCollectionTemplate, Messages.BaseParameterName);

        FromEmptyCollection<T, TException>(collectionToCheck, message);
    }

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
