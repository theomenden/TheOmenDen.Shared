﻿namespace TheOmenDen.Shared.Extensions;
public static class ExceptionExtensions
{
    /// <summary>
    /// Retrieves the deepest nested Inner <see cref="Exception"/>
    /// </summary>
    /// <param name="exception">The provided exception</param>
    /// <returns>The innermost <see cref="Exception"/></returns>
    public static Exception GetInnermostException(this Exception exception)
    {
        var exceptionToCheck = exception;

        while (exceptionToCheck.InnerException != null)
        {
            exceptionToCheck = exceptionToCheck.InnerException;
        }

        return exceptionToCheck;
    }

    /// <summary>
    /// Retrieves the deepest nested exception message
    /// </summary>
    /// <param name="exception">The provided exception</param>
    /// <returns><see cref="String"/></returns>
    public static string GetInnermostExceptionMessage(this Exception exception) => GetInnermostException(exception).Message;
}