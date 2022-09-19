using TheOmenDen.Shared.Utilities;

namespace TheOmenDen.Shared.Extensions;

public static class EnumerableExtensions
{
    /// <summary>
    /// <paramref name="source"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns>A random entry <typeparamref name="T"/></returns>
    /// <exception cref="InvalidOperationException">If there's nothing in the <paramref name="source"/> collection</exception>
    public static T GetRandomElement<T>(this IEnumerable<T> source)
    {
        
        if (source is ICollection<T> { Count: > 0 } collection)
        {
            return collection.ElementAt(ThreadSafeRandom.Global.Next(collection.Count));
        }

        if(source is T[] { Length: > 0} array)
        {
            return GetRandomElementFromArray(array);
        }

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

        if (source is ICollection<T> collection && totalElementsToReturn >= collection.Count)
        {
            return collection;
        }

        if(source is IList<T> list)
        {
            return GetRandomElementsFromList<T>(list, totalElementsToReturn);
        }

        if(source is T[] array)
        {
            return GetRandomElementsFromArray<T>(array, totalElementsToReturn);
        }

        var randomIndex = 0;

        var sourceAsArray = ArrayExtensions.CopyPooled(source.ToArray());
        var subArray = ArrayExtensions.SubArrayPooled(sourceAsArray, 0, totalElementsToReturn);

        foreach (var item in source)
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

        ArrayExtensions.ReturnArrayToPool(sourceAsArray);

        return subArray;
    }

    /// <summary>
    /// Array Optimized Random element retrieval from <paramref name="source"/> with the total <paramref name="totalElementsToReturn"/> being the number of random elements
    /// </summary>
    /// <typeparam name="T">The underlying type</typeparam>
    /// <param name="source">The supplied array</param>
    /// <param name="totalElementsToReturn">Number of random elements we wish to return</param>
    /// <returns><see cref="Array"/>: <typeparamref name="T"/></returns>
    public static T[] GetRandomElementsFromArray<T>(this T[] source, Int32 totalElementsToReturn)
    {
        var sourceSpan = source.AsSpan();

        var arrayToReturn = new T[totalElementsToReturn];

        for (int i = 0; i < sourceSpan.Length; i++)
        {
            var index = GenerateRandomElementIndex(totalElementsToReturn, i);

            if (index != -1 && index < totalElementsToReturn)
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
    /// <returns><see cref="Array{T}"/>: <typeparamref name="T"/></returns>
    public static T[] GetRandomElementsFromList<T>(this IList<T> source, Int32 totalElementsToReturn)
    {
        var returnList = new T[totalElementsToReturn];
        
        for(int i = 0; i < source.Count; i++)
        {
            var index = GenerateRandomElementIndex(totalElementsToReturn, i);

            if (index != -1 && index < totalElementsToReturn)
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

        for (int i = 0; i < source.Length; i++)
        {
            var index = GenerateRandomElementIndex(totalIndiciesToReturn, i);

            if (index != -1 && index < totalIndiciesToReturn)
            {
                returnList[index] = new Index(i);
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
        var returnList = new Index[totalIndiciesToReturn];

        for (int i = 0; i < source.Count; i++)
        {
            var index = GenerateRandomElementIndex(totalIndiciesToReturn, i);

            if (index != -1 && index < totalIndiciesToReturn)
            {
                returnList[index] = new Index(i);
            }
        }

        return returnList;
    }

    private static Int32 GenerateRandomElementIndex(int totalValuesToReturn,  int randomIndex)
    {
        if (randomIndex < totalValuesToReturn)
        {
            return randomIndex;
        }

        var index = ThreadSafeRandom.Global.Next(randomIndex + 1);

        if (index < totalValuesToReturn)
        {
            return index;
        }

        return -1;
    }
}