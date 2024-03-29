﻿namespace TheOmenDen.Shared.Extensions;

public static class IsBetweenExtensions
{
    /// <summary>
    /// <paramref name="lowerBound"/>  ≤ <paramref name="value"/> ≤ <paramref name="upperBound"/>
    /// </summary>
    /// <typeparam name="T">Any type that implements <see cref="IComparable{T}"/></typeparam>
    /// <param name="value">The value to check</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <returns><see langword="true"/> when the value falls between the range; <see langword="false"/> otherwise</returns>
    public static bool IsBetween<T>(this T value, T lowerBound, T upperBound) where T : IComparable<T>
        => lowerBound.CompareTo(value) <= 0 
           && upperBound.CompareTo(value) >= 0;

    /// <summary>
    /// <paramref name="lowerBound"/> &lt; <paramref name="value"/> ≤ <paramref name="upperBound"/>
    /// </summary>
    /// <typeparam name="T">Any type that implements <see cref="IComparable{T}"/></typeparam>
    /// <param name="value">The value to check</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <returns><see langword="true"/> when the value falls between the range; <see langword="false"/>When the value is at the lower boundary, and otherwise</returns>
    public static bool IsBetweenExclusiveLowerBound<T>(this T value, T lowerBound, T upperBound) where T : IComparable<T>
        => lowerBound.CompareTo(value) < 0 
           && value.CompareTo(upperBound) <= 0;


    /// <summary>
    /// <paramref name="lowerBound"/> ≤ <paramref name="value"/> &lt; <paramref name="upperBound"/>
    /// </summary>
    /// <typeparam name="T">Any type that implements <see cref="IComparable{T}"/></typeparam>
    /// <param name="value">The value to check</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <returns><see langword="true"/> when the value falls between the range; <see langword="false"/> otherwise</returns>
    public static bool IsBetweenExclusiveUpperBound<T>(this T value, T lowerBound, T upperBound) where T : IComparable<T>
        => lowerBound.CompareTo(value) <= 0 
           && upperBound.CompareTo(value) > 0;


    /// <summary>
    /// <paramref name="lowerBound"/> &lt; <paramref name="value"/> &lt; <paramref name="upperBound"/>
    /// </summary>
    /// <typeparam name="T">Any type that implements <see cref="IComparable{T}"/></typeparam>
    /// <param name="value">The value to check</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <returns><see langword="true"/> when the value falls exclusively between the range; <see langword="false"/> At Either boundary point, or otherwise.</returns>
    public static bool IsBetweenExclusiveBounds<T>(this T value, T lowerBound, T upperBound) where T : IComparable<T>
        => lowerBound.CompareTo(value) < 0 
           && upperBound.CompareTo(value) > 0;


    /// <summary>
    /// <paramref name="value"/> &gt; <paramref name="upperBound"/> or <paramref name="value"/> &lt; <paramref name="lowerBound"/>
    /// </summary>
    /// <typeparam name="T">Any type that implements <see cref="IComparable{T}"/></typeparam>
    /// <param name="value">The value to check</param>
    /// <param name="lowerBound">The lower boundary point</param>
    /// <param name="upperBound">The upper boundary point</param>
    /// <returns><see langword="true"/> When the value falls outside the range; <see langword="false"/> otherwise</returns>
    public static bool IsNotBetween<T>(this T value, T lowerBound, T upperBound) where T : IComparable<T>
        => !IsBetween(value, lowerBound, upperBound);

}