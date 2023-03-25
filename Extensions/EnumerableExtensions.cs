using System.Collections;
using TheOmenDen.Shared.Utilities;

namespace TheOmenDen.Shared.Extensions;

/// <summary>
/// Provides a set of LINQ-esque extension methods over the <see cref="IEnumerable{T}" /> type.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Instructs the <paramref name="source"/> to enact the <paramref name="action"/> upon each of its respective elements
    /// </summary>
    /// <typeparam name="T">The underlying type we're enumerating</typeparam>
    /// <param name="source">The supplied enumerable</param>
    /// <param name="action">The action we wish to enact</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is null</exception>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        switch (source)
        {
            case T[] {Length: > 0} array: Array.ForEach(array, action);
                break;
            case List<T> {Count: > 0} list: list.ForEach(action);
                break;
            default:
                foreach (var element in source)
                {
                    action(element);
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
    public static T GetRandomElement<T>(this IEnumerable<T>? source)
    => source is not null
            ? source switch
            {
                List<T> { Count: > 0 } list => GetRandomElementFromList(list),
                T[] { Length: > 0 } array => GetRandomElementFromArray(array),
                ICollection<T> { Count: > 0 } collection => collection.ElementAt(Random.Shared.Next(collection.Count)),
                _ => FisherYatesDurstenfeldShuffle(source.ToArray()).First()
            }
            : throw new ArgumentNullException(nameof(source));

    /// <summary>
    /// Access a random element from the provided <paramref name="source"/>
    /// </summary>
    /// <typeparam name="T">The underlying type</typeparam>
    /// <param name="source">The provided array</param>
    /// <returns>A single <typeparamref name="T"/> from <paramref name="source"/></returns>
    public static T GetRandomElementFromArray<T>(this T[] source)
    => source[Random.Shared.Next(source.Length)];

    /// <summary>
    /// Retrieves a random element from the provided <paramref name="source"/>
    /// </summary>
    /// <typeparam name="T">The underlying type</typeparam>
    /// <param name="source">The provided array</param>
    /// <returns>A single <typeparamref name="T"/> from <paramref name="source"/></returns>
    public static T GetRandomElementFromList<T>(this List<T> source)
    => source[Random.Shared.Next(source.Count)];

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

        return totalElementsToReturn <= 0
            ? Enumerable.Empty<T>()
            : source switch
            {
                ICollection<T> collection when totalElementsToReturn >= collection.Count => collection,
                T[] array => GetRandomElementsFromArray(array, totalElementsToReturn),
                IList<T> list => GetRandomElementsFromList(list, totalElementsToReturn),
                _ => FisherYatesDurstenfeldShuffle(source.ToArray())[..totalElementsToReturn]
            };
    }

    /// <summary>
    /// Retrieves a random element from <paramref name="source"/> with the total <paramref name="totalElementsToReturn"/> being the number of random elements
    /// </summary>
    /// <typeparam name="T">The underlying type</typeparam>
    /// <param name="source">The supplied array</param>
    /// <param name="totalElementsToReturn">Number of random elements we wish to return</param>
    /// <returns>Ann array of: <typeparamref name="T"/></returns>
    /// <remarks>Attempts to implement a naive interpretation of the Fisher-Yates-Durstenfeld shuffling algorithm</remarks>
    public static T[] GetRandomElementsFromArray<T>(this T[] source, Int32 totalElementsToReturn)
    => FisherYatesDurstenfeldShuffle(source)[..totalElementsToReturn];

    /// <summary>
    /// Retrieves a random element retrieval from <paramref name="source"/> with the total <paramref name="totalElementsToReturn"/> being the number of random elements
    /// </summary>
    /// <typeparam name="T">The underlying type</typeparam>
    /// <param name="source">The supplied list</param>
    /// <param name="totalElementsToReturn">Number of random elements we wish to return</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of: <typeparamref name="T"/></returns>
    /// <remarks>Attempts to implement a naive interpretation of the Fisher-Yates-Durstenfeld shuffling algorithm</remarks>
    public static IEnumerable<T> GetRandomElementsFromList<T>(this IList<T>? source, Int32 totalElementsToReturn)
    => source is not null
            ? FisherYatesDurstenfeldShuffle(source.ToArray()).Take(totalElementsToReturn)
            : throw new ArgumentNullException(nameof(source));

    /// <summary>
    /// Shuffles the provided <paramref name="source" /> using an interpretation of the Fisher-Yates-Durstenfeld shuffling algorithm
    /// </summary>
    /// <typeparam name="T">The type of elements we're shuffling</typeparam>
    /// <param name="source">The provided <see cref="Array"/></param>
    /// <returns>The shuffled array</returns>
    /// <remarks><see href="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm">Fisher Yates Durstenfeld Shuffle on wikipedia</see></remarks>
    private static T[] FisherYatesDurstenfeldShuffle<T>(this T[] source)
    {
        var sourceSpan = source.AsSpan();

        for (var i = 0; i < sourceSpan.Length - 1; i++)
        {
            var randomIndex = Random.Shared.Next(i, source.Length);
            (sourceSpan[randomIndex], sourceSpan[i]) = (sourceSpan[i], sourceSpan[randomIndex]);
        }

        return sourceSpan.ToArray();
    }
}