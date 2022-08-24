namespace TheOmenDen.Shared.Guards;

public static partial class Guard
{
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
}