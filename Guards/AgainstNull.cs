using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public partial class Guard : ICanGuard
{
    public static void FromNull<T>(T? parameter, String parameterName)
        where T : class
    {
        FromNull(parameter, parameterName, null);
    }

    public static void FromNull<T>(T parameter, String parameterName, String message)
    where T : class
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

    public static void FromNull<T, TException>(T parameter)
            where T : class
        where TException : Exception, new()
    {
        var message = String.Format(Messages.NullValueTemplate, Messages.BaseParameterName);

        FromNull<T, TException>(parameter, message);
    }

    public static void FromNull<T, TException>(T parameter, String message)
        where T : class
        where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NullValueTemplate, Messages.BaseParameterName);
        }

        var exception = CreateException<TException>(message);

        FromNull(parameter, exception);
    }

    public static void FromNull<T, TException>(T parameter, TException exception)
        where T : class
        where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => parameter is null, exception);
    }
}