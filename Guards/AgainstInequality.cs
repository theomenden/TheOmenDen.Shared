using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;
public partial class Guard : ICanGuard
{
    public static void FromInequality<T, TException>(T parameter, T value)
        where TException : Exception, new()
    {
        FromInequality<T, TException>(parameter, value, Messages.NotEqualToTemplate);
    }

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

