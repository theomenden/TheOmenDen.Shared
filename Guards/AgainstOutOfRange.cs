using TheOmenDen.Shared.Guards.Templates;

namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
    #region From Lower Boundary
    /// <summary>
    /// Protects the <typeparamref name="T"/> from having a <paramref name="value"/> that falls at or beneath the <paramref name="lowerBound"/>
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="parameterName">The name of the parameter</param>
    public static void FromLowerBoundary<T>(T value, T lowerBound, T upperBound, String parameterName)
        where T : IComparable, IComparable<T>
        => FromLowerBoundary(value, lowerBound, upperBound, parameterName, null);

    /// <summary>
    /// Protects the <typeparamref name="T"/> from having a <paramref name="value"/> that falls at or beneath the <paramref name="lowerBound"/> 
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="parameterName">The name of the parameter</param>
    /// <param name="message">The customized message we want to use with the <see cref="Exception"/></param>
    public static void FromLowerBoundary<T>(T value, T lowerBound, T upperBound, String parameterName, String? message)
        where T : IComparable, IComparable<T>
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.AtLowerBoundTemplate, Messages.BaseParameterName, lowerBound);
        }

        var argumentException = new ArgumentException(message, parameterName);

        FromLowerBoundary(value, lowerBound, upperBound, argumentException);
    }

    /// <summary>
    /// Protects the <typeparamref name="T"/> from having a <paramref name="value"/> that falls at or beneath the <paramref name="lowerBound"/> 
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <typeparam name="TException">The type of <see cref="Exception"/> we aim to throw</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="exception">The customized <see cref="Exception"/> we aim to throw when the condition is not met</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void FromLowerBoundary<T, TException>(T value, T lowerBound, T upperBound, TException exception)
        where T: IComparable, IComparable<T>
        where TException : Exception, new()
    {
        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => value.IsBetweenExclusiveLowerBound(lowerBound, upperBound), exception);
    }
    #endregion
    #region From Upper Boundary
    /// <summary>
    /// Protects the <typeparamref name="T"/> from having a <paramref name="value"/> that falls at or above the <paramref name="upperBound"/>  
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="parameterName">The name of the parameter</param>
    public static void FromUpperBoundary<T>(T value, T lowerBound, T upperBound, String parameterName)
        where T : IComparable, IComparable<T>
        => FromUpperBoundary(value, lowerBound, upperBound, parameterName, null);

    /// <summary>
    /// Protects the <typeparamref name="T"/> from having a <paramref name="value"/> that falls at or above the <paramref name="upperBound"/> 
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="parameterName">The name of the parameter</param>
    /// <param name="message"></param>
    public static void FromUpperBoundary<T>(T value, T lowerBound, T upperBound, String parameterName, String? message)
        where T : IComparable, IComparable<T>
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.AtUpperBoundTemplate, Messages.BaseParameterName, upperBound);
        }

        var argumentException = new ArgumentException(message, parameterName);

        FromUpperBoundary(value, lowerBound, upperBound, argumentException);
    }

    /// <summary>
    /// Protects the <typeparamref name="T"/> from having a <paramref name="value"/> that falls at or above the <paramref name="upperBound"/> 
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <typeparam name="TException">The type of <see cref="Exception"/> we aim to throw</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="exception">The customized <see cref="Exception"/> we aim to throw when the condition is not met</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void FromUpperBoundary<T, TException>(T value, T lowerBound, T upperBound, TException exception)
    where T : IComparable, IComparable<T>
    where TException : Exception, new()
    {

        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => value.IsBetweenExclusiveUpperBound(lowerBound, upperBound), exception);
    }
    #endregion
    #region From Either Boundary
    /// <summary>
    /// Evaluates a condition to make sure the provided <paramref name="value"/> is not outside, nor at either the <paramref name="lowerBound"/> and <paramref name="upperBound"/> 
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="parameterName">The name of the parameter</param>
    public static void FromEitherBoundaries<T>(T value, T lowerBound, T upperBound, String parameterName)
        where T : IComparable, IComparable<T>
        => FromEitherBoundaries(value, lowerBound, upperBound, parameterName, null);

    /// <summary>
    /// Evaluates a condition to make sure the provided <paramref name="value"/> is not outside, nor at either the <paramref name="lowerBound"/> and <paramref name="upperBound"/> 
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="parameterName">The name of the parameter</param>
    /// <param name="message">The custom message we want to return with an <see cref="Exception"/></param>
    public static void FromEitherBoundaries<T>(T value, T lowerBound, T upperBound, String parameterName, String? message)
        where T : IComparable, IComparable<T>
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.AtBoundaryPointsTemplate, Messages.BaseParameterName, lowerBound, upperBound);
        }

        var argumentException = new ArgumentException(message, parameterName);

        FromEitherBoundaries(value, lowerBound, upperBound, argumentException);
    }

    /// <summary>
    /// Evaluates a condition to make sure the provided <paramref name="value"/> is not outside, nor at either the <paramref name="lowerBound"/> and <paramref name="upperBound"/> 
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <typeparam name="TException">The type of <see cref="Exception"/> we aim to throw</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="exception">The customized <see cref="Exception"/> we aim to throw when the condition is not met</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void FromEitherBoundaries<T, TException>(T value, T lowerBound, T upperBound, TException exception)
    where T : IComparable, IComparable<T>
    where TException : Exception, new()
    {
        if(exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => value.IsBetweenExclusiveBounds(lowerBound, upperBound), exception);
    }
    #endregion
    #region From Being Outside Boundaries
    /// <summary>
    /// Evaluates a condition to make sure the provided <paramref name="value"/> is not outside the <paramref name="lowerBound"/> and <paramref name="upperBound"/> 
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="parameterName">The name of the parameter</param>
    public static void FromBeingOutsideBoundaries<T>(T value, T lowerBound, T upperBound, String parameterName)
        where T : IComparable, IComparable<T>
        => FromBeingOutsideBoundaries(value, lowerBound, upperBound, parameterName, null);

    /// <summary>
    /// Evaluates a condition to make sure the provided <paramref name="value"/> is not outside the <paramref name="lowerBound"/> and <paramref name="upperBound"/> 
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="parameterName">The name of the parameter</param>
    /// <param name="message"></param>
    public static void FromBeingOutsideBoundaries<T>(T value, T lowerBound, T upperBound, String parameterName, String? message)
        where T : IComparable, IComparable<T>
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.OutsideBoundaryPointsTemplate, Messages.BaseParameterName, lowerBound, upperBound);
        }

        var argumentException = new ArgumentException(message, parameterName);

        FromBeingOutsideBoundaries(value, lowerBound, upperBound, argumentException);
    }

    /// <summary>
    /// Evaluates a condition to make sure the provided <paramref name="value"/> is not outside the <paramref name="lowerBound"/> and <paramref name="upperBound"/> 
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <typeparam name="TException">The type of <see cref="Exception"/> we aim to throw</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="exception">The customized <see cref="Exception"/> we aim to throw when the condition is not met</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void FromBeingOutsideBoundaries<T, TException>(T value, T lowerBound, T upperBound, TException exception)
    where T : IComparable, IComparable<T>
    where TException : Exception, new()
    {

        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => value.IsBetween(lowerBound, upperBound), exception);
    }
    #endregion
    #region From Being Inside Boundaries
    /// <summary>
    /// Evaluates a condition to make sure the provided <paramref name="value"/> is not between <paramref name="lowerBound"/> and <paramref name="upperBound"/>
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="parameterName">The name of the parameter</param>
    public static void FromBeingInsideBoundaries<T>(T value, T lowerBound, T upperBound, String parameterName)
        where T : IComparable, IComparable<T>
        => FromBeingInsideBoundaries(value, lowerBound, upperBound, parameterName, null);

    /// <summary>
    /// Evaluates a condition to make sure the provided <paramref name="value"/> is not between <paramref name="lowerBound"/> and <paramref name="upperBound"/>
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="parameterName">The name of the parameter</param>
    /// <param name="message"></param>
    public static void FromBeingInsideBoundaries<T>(T value, T lowerBound, T upperBound, String parameterName, String? message)
        where T : IComparable, IComparable<T>
    {
        if (String.IsNullOrWhiteSpace(message))
        {
            message = String.Format(Messages.WithinBoundaryPointsTemplate, Messages.BaseParameterName, lowerBound, upperBound);
        }

        var argumentException = new ArgumentException(message, parameterName);

        FromBeingInsideBoundaries(value, lowerBound, upperBound, argumentException);
    }

    /// <summary>
    /// Evaluates a condition to make sure the provided <paramref name="value"/> is not between <paramref name="lowerBound"/> and <paramref name="upperBound"/>
    /// </summary>
    /// <typeparam name="T">The type we're working with</typeparam>
    /// <typeparam name="TException">The type of <see cref="Exception"/> we aim to throw</typeparam>
    /// <param name="value">The actual value we're comparing</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <param name="exception">The customized <see cref="Exception"/> we aim to throw when the condition is not met</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void FromBeingInsideBoundaries<T, TException>(T value, T lowerBound, T upperBound, TException exception)
    where T : IComparable, IComparable<T>
    where TException : Exception, new()
    {

        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        FromCondition(() => value.IsNotBetween(lowerBound, upperBound), exception);
    }
    #endregion
}