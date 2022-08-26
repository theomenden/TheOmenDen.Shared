using System.Text.RegularExpressions;
using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    /// <summary>
    /// Checks to see if the supplied <paramref name="parameter"/> matches the supplied <paramref name="regexPattern"/>
    /// </summary>
    /// <param name="parameter">The value we aim to check</param>
    /// <param name="parameterName">The name of the value</param>
    /// <param name="regexPattern">The regular expression we're using</param>
    /// <param name="regexOptions"><see cref="RegexOptions"/> Any addition regex options</param>
    public static void FromNotMatchingPattern(String parameter, String parameterName, String regexPattern, RegexOptions regexOptions = RegexOptions.None)
    {
        var message = String.Format(Messages.NotMatchedByPatternTemplate, Messages.BaseParameterName, regexPattern);

        FromNotMatchingPattern(parameter, parameterName, regexPattern, message, regexOptions);
    }

    /// <summary>
    /// Checks to see if the supplied <paramref name="parameter"/> matches the supplied <paramref name="regexPattern"/>
    /// </summary>
    /// <param name="parameter">The value we aim to check</param>
    /// <param name="parameterName">The name of the value</param>
    /// <param name="regexPattern">The regular expression we're using</param>
    /// <param name="message">The custom message we aim to throw with an exception</param>
    /// <param name="regexOptions"><see cref="RegexOptions"/> Any addition regex options</param>
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

    /// <summary>
    /// Checks to see if the supplied <paramref name="parameter"/> matches the supplied <paramref name="regexPattern"/>
    /// </summary>
    /// <typeparam name="TException">The type of exception we aim to throw on failing the condition</typeparam>
    /// <param name="parameter">The value we aim to check</param>
    /// <param name="regexPattern">The regular expression we're using</param>
    /// <param name="regexOptions"><see cref="RegexOptions"/> Any additional regex options</param>
    public static void FromNotMatchingPattern<TException>(String parameter, String regexPattern, RegexOptions regexOptions = RegexOptions.None)
    where TException : Exception, new()
    {
        var message = String.Format(Messages.NotMatchedByPatternTemplate, Messages.BaseParameterName, regexPattern);

        FromNotMatchingPattern<TException>(parameter, regexPattern, message, regexOptions);
    }

    /// <summary>
    /// Checks to see if the supplied <paramref name="parameter"/> matches the supplied <paramref name="regexPattern"/>
    /// </summary>
    /// <typeparam name="TException">The type of exception we aim to throw on failing the condition</typeparam>
    /// <param name="parameter">The value we aim to check</param>
    /// <param name="regexPattern">The regular expression we're using</param>
    /// <param name="message">The custom message we aim to return with the caller via a thrown exception</param>
    /// <param name="regexOptions"><see cref="RegexOptions"/> Any additional regex options</param>
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

    /// <summary>
    /// Checks to see if the supplied <paramref name="parameter"/> matches the supplied <paramref name="regexPattern"/>
    /// </summary>
    /// <typeparam name="TException">The type of exception we aim to throw on failing the condition</typeparam>
    /// <param name="parameter">The value we aim to check</param>
    /// <param name="regexPattern">The regular expression we're using</param>
    /// <param name="exception">The exception we aim to throw</param>
    /// <param name="regexOptions"><see cref="RegexOptions"/> Any additional regex options</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the following are <see langword="null"/>: <paramref name="parameter"/>, <paramref name="regexPattern"/>, <paramref name="exception"/> </exception>
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