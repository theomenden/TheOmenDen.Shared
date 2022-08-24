using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    public static void FromValuesLessThan<T>(T parameter, T lowerThreshold, String message)
        where T : IComparable, IComparable<T>
    {
        FromValuesLessThan(parameter, lowerThreshold, message, null);
    }
    public static void FromValuesLessThan<T>(T parameter, T lowerThreshold, String parameterName, String message)
        where T : IComparable, IComparable<T>
    {
        if (String.IsNullOrWhiteSpace(parameterName))
        {
            parameterName = Messages.BaseParameterName;
        }

        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.LessThanValueTemplate, parameterName, lowerThreshold);
        }

        var argumentOutOfRangeException = new ArgumentOutOfRangeException(parameterName, message);

        FromValuesLessThan(parameter, lowerThreshold, argumentOutOfRangeException);
    }

    public static void FromValuesLessThan<T, TException>(T parameter, T lowerThreshold, String message)
    where T: IComparable, IComparable<T>
    where TException: Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.LessThanValueTemplate, Messages.BaseParameterName, lowerThreshold);
        }

        var exception = CreateException<TException>(message);

        FromValuesLessThan(parameter, lowerThreshold, exception);
    }

    public static void FromValuesLessThan<T, TException>(T parameter, T lowerThreshold, TException exception)
    where T: IComparable, IComparable<T>
    where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => parameter.CompareTo(lowerThreshold) < 0, exception);
    }
}