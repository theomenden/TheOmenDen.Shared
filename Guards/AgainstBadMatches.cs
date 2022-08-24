using System.Text.RegularExpressions;
using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    public static void FromNotMatchingPattern(String parameter, String parameterName, String regexPattern, RegexOptions regexOptions = RegexOptions.None)
    {
        var message = String.Format(Messages.NotMatchedByPatternTemplate, Messages.BaseParameterName, regexPattern);

        FromNotMatchingPattern(parameter, parameterName, regexPattern, message, regexOptions);
    }

    public static void FromNotMatchingPattern(String parameter, String parameterName, String regexPattern, String message, RegexOptions regexOptions = RegexOptions.None)
    {
        if (String.IsNullOrWhiteSpace(parameterName))
        {
            parameterName = Messages.BaseParameterName;
        }

        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NotMatchedByPatternTemplate, parameterName, regexPattern);
        }

        var argumentException = new ArgumentException(message, parameterName);

        FromNotMatchingPattern(parameter, regexPattern, argumentException, regexOptions);

    }

    public static void FromNotMatchingPattern<TException>(String parameter, String regexPattern, RegexOptions regexOptions = RegexOptions.None)
    where TException : Exception, new()
    {
        var message = String.Format(Messages.NotMatchedByPatternTemplate, Messages.BaseParameterName, regexPattern);

        FromNotMatchingPattern<TException>(parameter, regexPattern, message, regexOptions);
    }

    public static void FromNotMatchingPattern<TException>(String parameter, String regexPattern, String message, RegexOptions regexOptions = RegexOptions.None)
        where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.NotMatchedByPatternTemplate, Messages.BaseParameterName, regexPattern);
        }

        var exception = CreateException<TException>(message);

        FromNotMatchingPattern(parameter, regexPattern, exception, regexOptions);
    }

    public static void FromNotMatchingPattern<TException>(String parameter, string regexPattern, TException exception, RegexOptions regexOptions = RegexOptions.None)
        where TException : Exception, new()
    {
        if (String.IsNullOrWhiteSpace(parameter))
        {
            throw new ArgumentNullException(nameof(parameter));
        }

        if (String.IsNullOrWhiteSpace(regexPattern))
        {
            throw new ArgumentNullException(nameof(regexPattern));
        }

        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => Regex.IsMatch(parameter, regexPattern, regexOptions), exception);
    }
}