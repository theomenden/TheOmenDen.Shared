using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="parameterName"></param>
    public static void FromNullOrEmpty(String parameter, String parameterName)
    {
        FromNullOrEmpty(parameter, parameterName, null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="parameterName"></param>
    /// <param name="message"></param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="parameter"></param>
    public static void FromNullOrEmpty<TException>(String parameter)
    where TException : Exception, new()
    {
        var message = String.Format(Messages.NotNullOrEmptyTemplate, Messages.BaseParameterName);

        FromNullOrEmpty<TException>(parameter, message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="message"></param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="exception"></param>
    /// <exception cref="ArgumentNullException"></exception>
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
