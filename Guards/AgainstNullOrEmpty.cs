using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    public static void FromNullOrEmpty(String parameter, String parameterName)
    {
        FromNullOrEmpty(parameter, parameterName, null);
    }

    public static void FromNullOrEmpty(String parameter, String parameterName, String message)
    {
        if(String.IsNullOrWhiteSpace(parameterName))
        {
            parameterName = Messages.BaseParameterName;
        }

        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NotNullOrEmptyTemplate, parameterName);
        }

        var argumentException = new ArgumentException(message, parameterName);

        FromNullOrEmpty(parameter, argumentException);
    }

    public static void FromNullOrEmpty<TException>(String parameter)
    where TException : Exception, new()
    {
        var message = String.Format(Messages.NotNullOrEmptyTemplate, Messages.BaseParameterName);

        FromNullOrEmpty<TException>(parameter, message);
    }

    public static void FromNullOrEmpty<TException>(String parameter, String message)
    where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NotNullOrEmptyTemplate, Messages.BaseParameterName);
        }

        var exception = CreateException<TException>(message);

        FromNullOrEmpty(parameter, exception);
    }

    public static void FromNullOrEmpty<TException>(String parameter, TException exception)
        where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        Guard.FromCondition(() => String.IsNullOrEmpty(parameter), exception);
    }
}
