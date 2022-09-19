using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using TheOmenDen.Shared.Guards;
using TheOmenDen.Shared.Utilities;

namespace TheOmenDen.Shared.Extensions;

public static class EnumerableExtensions
{
    public static T GetRandomElement<T>(this IEnumerable<T> source)
    {
        using var random = ThreadSafeRandom.Global;

        if (source is ICollection<T> { Count: > 0 } collection)
        {
            return collection.ElementAt(random.Next(collection.Count));
        }

        var randomElement = default(T);
        var indexCounter = 0;

        foreach (var element in source)
        {
            indexCounter++;

            if (random.Next(indexCounter) is 0)
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