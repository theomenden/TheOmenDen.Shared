using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    public static void FromNullOrWhitespace(String parameter, String parameterName)
    {
        FromNullOrWhitespace(parameter, parameterName, null);
    }

    public static void FromNullOrWhitespace(String parameter, String parameterName, String message)
    {
        if (String.IsNullOrWhiteSpace(parameterName))
        {
            parameterName = Messages.BaseParameterName;
        }

        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NotNullOrWhitespaceTemplate, parameterName);
        }

        var argumentException = new ArgumentException(message, parameterName);

        FromNullOrWhitespace(parameter, argumentException);
    }

    public static void FromNullOrWhitespace<TException>(String parameter)
        where TException : Exception, new()
    {
        var message = String.Format(Messages.NotNullOrWhitespaceTemplate, Messages.BaseParameterName);

        FromNullOrWhitespace<TException>(parameter, message);
    }

    public static void FromNullOrWhitespace<TException>(String parameter, String message)
        where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NotNullOrWhitespaceTemplate, Messages.BaseParameterName);
        }

        var exception = CreateException<TException>(message);

        FromNullOrWhitespace(parameter, exception);
    }

    public static void FromNullOrWhitespace<TException>(String parameter, TException exception)
        where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        Guard.FromCondition(() => String.IsNullOrWhiteSpace(parameter), exception);
    }
}