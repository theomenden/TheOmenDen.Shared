using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    public static void FromCondition<TException>(Func<bool> predicate)
        where TException : Exception, new()
    {
        FromCondition<TException>(predicate, Messages.PreconditionTemplate);
    }

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