using System.Diagnostics;
using TheOmenDen.Shared.Exceptions;
using TheOmenDen.Shared.Guards;

namespace TheOmenDen.Shared.Extensions;

public static class GuardExtensions
{
    #region Generalized Against Null
    public static void Null<T>(this ICanGuard guardClase, T? valueArgument)
    {
        if (valueArgument is null)
        {
            throw new ArgumentNullException(nameof(valueArgument));
        }
    }
    #endregion
    #region String Guards
    public static void NullOrEmpty(this ICanGuard guardClause, String valueAgrument)
    {
        if (String.IsNullOrEmpty(valueAgrument))
        {
            throw new ArgumentNullException(nameof(valueAgrument));
        }
    }

    public static String NullOrEmptyWithDefault(this ICanGuard guardClause, String valueArgument, String defaultValue)
    {
        if (String.IsNullOrEmpty(valueArgument))
        {
            return defaultValue;
        }
        return valueArgument;
    }
    #endregion
    #region Predicate Based Guards
    public static void Requires(this ICanGuard guardClause, Func<bool> predicate, string message)
    {
        if (predicate())
        {
            return;
        }

        Debug.Fail(message);
        throw new GuardException(message);
    }

    public static void Requires<T>(this ICanGuard guardClause, T argumentValue, Func<T, bool> predicate, string message)
    {
        if(predicate(argumentValue))
        {
            return;
        }

        Debug.Fail(message);
        throw new GuardException(message);
    }

    [Conditional("DEBUG")]
    public static void Ensures(this ICanGuard guardClause, Func<bool> predicate, string message)
    {
        Debug.Assert(predicate(), message);
    }
    #endregion
}