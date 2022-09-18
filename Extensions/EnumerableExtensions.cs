using System;
using System.Reflection.Metadata.Ecma335;
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

        var sourceAsArray = source.ToArray();
        
        var randomList = new List<T>(totalElementsToReturn);

        using var random = new ThreadSafeRandom();

        var randomIndex = 0;
        
        Array.ForEach(sourceAsArray, element =>
        {
            if (randomIndex < totalElementsToReturn)
            {
                randomList.Add(element);
                Interlocked.Increment(ref randomIndex);
                return;
            }

            var index = random.Next(randomIndex + 1);

            if (index < totalElementsToReturn)
            {
                randomList[index] = element;
            }

            Interlocked.Increment(ref randomIndex);
        });

        return randomList;
    }

    public static IEnumerable<T> YieldRandomElements<T>(this IEnumerable<T> source, Int32 totalElementsToReturn)
    {
        using var random = new ThreadSafeRandom();

        var randomIndexCount = 0;

        foreach(var element in source)
        {
            if (randomIndexCount >= totalElementsToReturn)
            {
                var index = random.Next(randomIndexCount + 1);

                if (index < totalElementsToReturn)
                {
                    yield return element;
                    randomIndexCount++;
                    continue;
                }
            }
            yield return element;
            randomIndexCount++;
        }
    }
}