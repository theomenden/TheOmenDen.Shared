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
        var randomIndex = 0;
        var sourceSpan = source.AsSpan();

        var arrayToReturn = new T[totalElementsToReturn];

        foreach (var item in sourceSpan)
        {
            GenerateRandomIndex<T>(totalElementsToReturn, item, arrayToReturn, randomIndex);
            randomIndex++;
        }

        return arrayToReturn;
    }

    public static T[] GetRandomElementsFromList<T>(this List<T> source, Int32 totalElementsToReturn)
    {
        var randomIndex = 0;

        var arrayToReturn = new T[totalElementsToReturn];

        foreach (var item in source)
        {
            GenerateRandomIndex(totalElementsToReturn, item, arrayToReturn, randomIndex);
            randomIndex++;
        }

        return arrayToReturn;
    }

    private static void GenerateRandomIndex<T>(int totalElementsToReturn, T item, T[] subArray, int randomIndex)
    {
        if (randomIndex < totalElementsToReturn)
        {
            subArray[randomIndex] = item;
            return;
        }

        var index = ThreadSafeRandom.Global.Next(randomIndex + 1);

        if (index < totalElementsToReturn)
        {
            subArray[index] = item;
        }
    }
}