using System.Collections;
using TheOmenDen.Shared.Utilities;

namespace TheOmenDen.Shared.Extensions;

public static class EnumerableExtensions
{

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        switch (source)
        {
            case T[] array: Array.ForEach(array, action); break;
            case ICollection<T> collection: collection.ForEach(action); break;
            default:
                foreach (var item in source)
                {
                    action(item);
                }
                break;
        }

    }



    /// <summary>
    /// Retrieves a random value from the provided <paramref name="source"/> collection
    /// </summary>
    /// <typeparam name="T">The underlying type</typeparam>
    /// <param name="source">The provided collection</param>
    /// <returns>A random entry <typeparamref name="T"/></returns>
    /// <exception cref="InvalidOperationException">If there's nothing in the <paramref name="source"/> collection</exception>
    public static T GetRandomElement<T>(this IEnumerable<T> source)
    {
        switch (source)
        {
            case ICollection<T> { Count: > 0 } collection:
                return collection.ElementAt(ThreadSafeRandom.Global.Next(collection.Count));
            case T[] { Length: > 0 } array:
                return GetRandomElementFromArray(array);
            default:

                var randomElement = default(T);
                var indexCounter = 0;

                foreach (var element in source)
                {
                    indexCounter++;

                    if (ThreadSafeRandom.Global.Next(indexCounter) is 0)
                    {
                        randomElement = element;
                    }
                }

                if (indexCounter <= 0)
                {
                    throw new InvalidOperationException($"{nameof(source)} collection was empty");
                }

                return randomElement!;
        }
    }

    /// <summary>
    /// Retrieves a random <see cref="Index" /> from the provided <paramref name="source"/>
    /// </summary>
    /// <param name="source">The provided collection</param>
    /// <typeparam name="T">The underlying type</typeparam>
    /// <returns>A random <see cref="Index"/></returns>
public static Index GetRandomIndex<T>(this IEnumerable<T> source)
{
    var random = ThreadSafeRandom.Global.Next(source.Count() - 1);

    return GenerateRandomElementIndex(1, random);
}

/// <summary>
/// Access a random element from the provided <paramref name="source"/>
/// </summary>
/// <typeparam name="T">The underlying type</typeparam>
/// <param name="source">The provided array</param>
/// <returns>A single <typeparamref name="T"/> from <paramref name="source"/></returns>
public static T GetRandomElementFromArray<T>(this T[] source)
{
    var indexCounter = source.Length - 1;

    var index = GenerateRandomElementIndex(1, ThreadSafeRandom.Global.Next(indexCounter));

    return source[index];
}

/// <summary>
/// Retrieves a set of elements from the provided <paramref name="source"/> collection
/// </summary>
/// <typeparam name="T">The underlying type</typeparam>
/// <param name="source">The provided collection</param>
/// <param name="totalElementsToReturn">The number of random elements we want to recieve</param>
/// <returns><see cref="IEnumerable{T}"/>: <typeparamref name="T"/></returns>
/// <exception cref="ArgumentNullException">throw when the <paramref name="source"/> is null</exception>
public static IEnumerable<T> GetRandomElements<T>(this IEnumerable<T> source, Int32 totalElementsToReturn)
{
    if (source is null)
    {
        throw new ArgumentNullException(nameof(source));
    }

    if (totalElementsToReturn <= 0)
    {
        return Enumerable.Empty<T>();
    }

    switch (source)
    {
        case ICollection<T> collection when totalElementsToReturn >= collection.Count:
            return collection;
        case T[] array:
            return GetRandomElementsFromArray(array, totalElementsToReturn);
        case IList<T> list:
            return GetRandomElementsFromList(list, totalElementsToReturn);
        default:
            var randomIndex = 0;

            var sourceAsArray = source.ToArray();
            var subArray = sourceAsArray.SubArray(0, totalElementsToReturn);

            foreach (var item in sourceAsArray)
            {
                if (randomIndex < totalElementsToReturn)
                {
                    subArray[randomIndex] = item;
                    randomIndex++;
                    continue;
                }

                var index = ThreadSafeRandom.Global.Next(randomIndex + 1);

                if (index < totalElementsToReturn)
                {
                    subArray[index] = item;
                }

                randomIndex++;
            }

            return subArray;
    }
}

/// <summary>
/// Array Optimized Random element retrieval from <paramref name="source"/> with the total <paramref name="totalElementsToReturn"/> being the number of random elements
/// </summary>
/// <typeparam name="T">The underlying type</typeparam>
/// <param name="source">The supplied array</param>
/// <param name="totalElementsToReturn">Number of random elements we wish to return</param>
/// <returns>Ann array of: <typeparamref name="T"/></returns>
public static T[] GetRandomElementsFromArray<T>(this T[] source, Int32 totalElementsToReturn)
{
    var sourceSpan = source.AsSpan();

    var arrayToReturn = new T[totalElementsToReturn];

    for (var i = 0; i < sourceSpan.Length; i++)
    {
        var index = GenerateRandomElementIndex(totalElementsToReturn, i);

        if (index.Value is not -1 && index.Value < totalElementsToReturn)
        {
            arrayToReturn[index] = source[i];
        }
    }

    return arrayToReturn;
}

/// <summary>
/// <see cref="IList{T}"/> Optimized Random element retrieval from <paramref name="source"/> with the total <paramref name="totalElementsToReturn"/> being the number of random elements
/// </summary>
/// <typeparam name="T">The underlying type</typeparam>
/// <param name="source">The supplied list</param>
/// <param name="totalElementsToReturn">Number of random elements we wish to return</param>
/// <returns>An array of: <typeparamref name="T"/></returns>
public static T[] GetRandomElementsFromList<T>(this IList<T> source, Int32 totalElementsToReturn)
{
    var returnList = new T[totalElementsToReturn];

    for (var i = 0; i < source.Count; i++)
    {
        var index = GenerateRandomElementIndex(totalElementsToReturn, i);

        if (index.Value is not -1 && index.Value < totalElementsToReturn)
        {
            returnList[index] = source[i];
        }
    }

    return returnList;
}

/// <summary>
/// Retrieves a random set of <see cref="Index"/> from the provided <paramref name="source"/>
/// </summary>
/// <typeparam name="T">The underlying type</typeparam>
/// <param name="source">The source collection</param>
/// <param name="totalIndiciesToReturn">The total elements we wish to return</param>
/// <returns><see cref="Array"/>:<see cref="Index"/></returns>
public static Index[] GetRandomElementIndiciesFromArray<T>(this T[] source, Int32 totalIndiciesToReturn)
{
    var returnList = new Index[totalIndiciesToReturn];

    for (var i = 0; i < source.Length; i++)
    {
        var index = GenerateRandomElementIndex(totalIndiciesToReturn, i);

        if (index.Value is not -1 && index.Value < totalIndiciesToReturn)
        {
            returnList[index] = new(i);
        }
    }

    return returnList;
}

/// <summary>
/// Retrieves a random set of <see cref="Index"/> from the provided <paramref name="source"/>
/// </summary>
/// <typeparam name="T">The underlying type</typeparam>
/// <param name="source">The source collection</param>
/// <param name="totalIndiciesToReturn">The total elements we wish to return</param>
/// <returns><see cref="Array"/>:<see cref="Index"/></returns>
public static Index[] GetRandomElementIndiciesFromList<T>(this IList<T> source, Int32 totalIndiciesToReturn)
{
    var randomIndexes = new Index[totalIndiciesToReturn];

    for (var i = 0; i < source.Count; i++)
    {
        var index = GenerateRandomElementIndex(totalIndiciesToReturn, i);

        if (index.Value is not -1 && index.Value < totalIndiciesToReturn)
        {
            randomIndexes[index] = new(i);
        }
    }

    return randomIndexes;
}

private static Index GenerateRandomElementIndex(int totalValuesToReturn, int randomIndex)
{
    if (randomIndex < totalValuesToReturn)
    {
        return new(randomIndex);
    }

    var index = ThreadSafeRandom.Global.Next(randomIndex + 1);

    return index < totalValuesToReturn ? index : -1;
}

}