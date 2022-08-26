using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="parameterName"></param>
    public static void FromNull<T>(T? parameter, String parameterName)
    {
        FromNull(parameter, parameterName, null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="parameterName"></param>
    /// <param name="message"></param>
    public static void FromNull<T>(T parameter, String parameterName, String message)
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException"></typeparam>
    /// <param name="parameter"></param>
    public static void FromNull<T, TException>(T parameter)
    where TException : Exception, new()
    {
        var message = String.Format(Messages.NullValueTemplate, Messages.BaseParameterName);

        FromNull<T, TException>(parameter, message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="message"></param>
    public static void FromNull<T, TException>(T parameter, String message)
    where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NullValueTemplate, Messages.BaseParameterName);
        }

        var exception = CreateException<TException>(message);

        FromNull(parameter, exception);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException"></typeparam>
    /// <param name="parameter"></param>
    /// <param name="exception"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void FromNull<T, TException>(T? parameter, TException exception)
    where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => parameter is null, exception);
    }
}