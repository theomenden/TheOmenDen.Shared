namespace TheOmenDen.Shared.Extensions;

/// <summary>
/// <para>Factory implementation to retrieve and create <see cref="StringBuilder"/>s from a given <see cref="StringBuilderPool"/></para>
/// <para>Works with an underlying locking mechanism for operations across threads</para>
/// </summary>
/// <typeparam name="T"></typeparam>
public static class StringBuilderPoolFactory<T>
{
    private static readonly StringBuilderPool _stringBuilders = StringBuilderPool.Instance;
    private static readonly object _stringBuilderLock = new();

    /// <summary>
    /// Checks the underlying <see cref="StringBuilderPool"/> to see if it contains a entry for the given <paramref name="key"/>
    /// </summary>
    /// <param name="key"></param>
    /// <returns><c>True</c> if the <see cref="StringBuilder"/> is found <c>False</c> otherwise</returns>
    public static Boolean Exists(String key)
    {
        lock (_stringBuilderLock)
        {
            return !String.IsNullOrEmpty(key)
                   && _stringBuilders.ContainsKey(GetLockKey(key));
        }
    }

    /// <summary>
    /// Retrieves a <see cref="StringBuilder"/> from the underlying <see cref="StringBuilderPool"/> that matches the provided <paramref name="key"/>
    /// </summary>
    /// <param name="key">The key to search with</param>
    /// <returns><see cref="StringBuilder"/></returns>
    public static StringBuilder Get(String key)
    {
        lock (_stringBuilderLock)
        {
            return _stringBuilders.ContainsKey(GetLockKey(key))
                ? _stringBuilders[GetLockKey(key)]
                : null;
        }
    }

    /// <summary>
    /// Creates an instance of a <see cref="StringBuilder"/> to add to the underlying <see cref="StringBuilderPool"/>
    /// </summary>
    /// <param name="key">The key to search with</param>
    /// <returns>The <see cref="StringBuilder"/></returns>
    public static StringBuilder Create(String key)
    {
        lock (_stringBuilderLock)
        {
            if (!_stringBuilders.ContainsKey(GetLockKey(key)))
            {
                _stringBuilders.TryAdd(GetLockKey(key), new StringBuilder());
            }

            return _stringBuilders[GetLockKey(key)];
        }
    }

    /// <summary>
    /// Removes the <see cref="StringBuilder"/> from the underlying <see cref="StringBuilderPool"/> that matches the <paramref name="key"/>
    /// </summary>
    /// <param name="key">The key to search with</param>
    public static void Remove(String key)
    {
        lock (_stringBuilderLock)
        {
            if (_stringBuilders.ContainsKey(GetLockKey(key)) &&
                _stringBuilders.TryRemove(GetLockKey(key), out var removedStringBuilder))
            {
                removedStringBuilder.Clear();
            }
        }
    }

    private static String GetLockKey(String key) => $"{typeof(T).Name}_{key}";
}
