using TheOmenDen.Shared.Enumerations.Attributes;
using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;
public partial class Guard
{
    public static void FromNegativeValues<T>(T parameter, String parameterName)
    where T : IComparable<T>
    {
        var attribute = (AllowNegativeEnumerationKeysAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(AllowNegativeEnumerationKeysAttribute));

        if (attribute is not null)
        {
            return;
        }

        if (String.IsNullOrWhiteSpace(parameterName))
        {
            parameterName = Messages.BaseParameterName;
        }

        var message = String.Format(Messages.NegativeValueTemplate, parameterName);

        var argumentOutOfRangeException = new ArgumentOutOfRangeException(parameterName, message);

        FromNegativeValues(parameter, argumentOutOfRangeException);
    }

    public static void FromNegativeValues<T, TException>(T parameter, String message)
        where T : IComparable<T>
        where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NegativeValueTemplate, Messages.BaseParameterName);
        }

        var exception = CreateException<TException>(message);

        FromNegativeValues(parameter, exception);
    }

    public static void FromNegativeValues<T, TException>(T parameter, TException exception)
        where T : IComparable<T>
        where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => Int32.TryParse(parameter.ToString(), out var number) && number.CompareTo(0) < 0, exception);
    }
}