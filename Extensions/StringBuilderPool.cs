using System.Collections.Concurrent;

namespace TheOmenDen.Shared.Extensions;
/// <summary>
/// Creates a way to manage a pool of <see cref="StringBuilder"/>s to avoid creating them every time we need one
/// <inheritdoc cref="ConcurrentDictionary{TKey, TValue}"/>
/// <inheritdoc cref="Lazy{T}"/>
/// </summary>
/// <remarks>Should be able to register this as a singleton in the AddServices methods</remarks>
internal sealed class StringBuilderPool: ConcurrentDictionary<String, StringBuilder>
{
    private static readonly Lazy<StringBuilderPool> Builders = new(CreatePool);

    internal static StringBuilderPool Instance => Builders.Value;

    private StringBuilderPool(ConcurrentDictionary<String, StringBuilder> dictionary)
        : base(dictionary)
    {}
    
    private static StringBuilderPool CreatePool() => new(new());
}