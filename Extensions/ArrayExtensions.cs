using System.Buffers;

namespace TheOmenDen.Shared.Extensions;

public static class ArrayExtensions
{
    /// <summary>
    /// Creates a slice of the supplied <paramref name="source"/> array, starting from the given <paramref name="index"/>
    /// </summary>
    /// <typeparam name="T">The underlying type in the array</typeparam>
    /// <param name="source">The provided array</param>
    /// <param name="index">The provided index</param>
    /// <returns>An sliced array of <typeparamref name="T"/> entities</returns>
    public static T[] SubArray<T>(this T[] source, Int32 index)
    {
        return SubArray<T>(source, index, source.Length - index);
    }

    /// <summary>
    /// Creates a sliced array from the provided <paramref name="source"/> array, starting from the given <paramref name="index"/> to the provided <paramref name="length"/>
    /// </summary>
    /// <typeparam name="T">The underlying type of the array</typeparam>
    /// <param name="source">The provided array that we want to slice</param>
    /// <param name="index">The starting index for the slice</param>
    /// <param name="length">The length of the slice</param>
    /// <returns>The sliced array</returns>
    public static T[] SubArray<T>(this T[] source, Int32 index, Int32 length)
    {
        if (index == 0 && length == source.Length)
        {
            return source;
        }

        if (length == 0)
        {
            return Array.Empty<T>();
        }

        var subarray = new T[length];

        Array.Copy(source, index, subarray, 0, length);

        return subarray;
    }

    /// <summary>
    /// Combines the provided <paramref name="source"/> array with the <paramref name="appendArray"/> values into a new array,
    /// starting from the <paramref name="index"/> provided, until the <paramref name="length"/>
    /// </summary>
    /// <typeparam name="T">The underlying type of the arrays</typeparam>
    /// <param name="source">The source array</param>
    /// <param name="appendArray">The array with values to append to the source array</param>
    /// <param name="index">starting index of the <paramref name="appendArray"/></param>
    /// <param name="length">The amount of entites from the provided index that we want to copy over, and expand our arrays by</param>
    /// <returns>The combination of the arrays</returns>
    public static T[] Append<T>(this T[] source, T[] appendArray, Int32 index, Int32 length)
    {
        if (length == 0)
        {
            return source;
        }

        var newLength = source.Length + length - index;

        if (newLength <= 0)
        {
            return Array.Empty<T>();
        }

        var newArray = new T[newLength];

        Array.Copy(source, 0, newArray, 0, source.Length);

        Array.Copy(appendArray, index, newArray, source.Length, length - index);

        return newArray;
    }

    /// <summary>
    /// Copies the supplied <paramref name="source"/> array into the <seealso cref="ArrayPool{T}"/>
    /// </summary>
    /// <typeparam name="T">The underlying array type</typeparam>
    /// <param name="source">The supplied array</param>
    /// <returns>The copied array's new <see cref="ArrayPool{T}"/> reference</returns>
    public static T[] CopyPooled<T>(this T[] source)
    {
        return SubArrayPooled(source, 0, source.Length);
    }

    /// <summary>
    /// Creates a sliced array from the supplied <paramref name="source"/> array, into the <see cref="ArrayPool{T}"/>
    /// </summary>
    /// <typeparam name="T">The underlying type of the supplied array</typeparam>
    /// <param name="source">The source array</param>
    /// <param name="index">The index where we start the bisection</param>
    /// <param name="length">The length that we need to rent from the <see cref="ArrayPool{T}"/> buffer</param>
    /// <returns>The copied array's new <see cref="ArrayPool{T}"/> reference</returns>
    public static T[] SubArrayPooled<T>(this T[] source, Int32 index, Int32 length)
    {
        var rentedSubArray = ArrayPool<T>.Shared.Rent(length);

        Array.Copy(source, index, rentedSubArray, 0, length);

        return rentedSubArray;
    }

    /// <summary>
    /// Returns the provided <paramref name="source"/> array to the buffered pool
    /// </summary>
    /// <typeparam name="T">The underlying type of the array</typeparam>
    /// <param name="source">The provided array to return</param>
    /// <remarks>See <see cref="ArrayPool{T}"/></remarks>
    public static void ReturnArrayToPool<T>(this T[] source)
    {
        if (source is null)
        {
            return;
        }

        ArrayPool<T>.Shared.Return(source);
    }
}
