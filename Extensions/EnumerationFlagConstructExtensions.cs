namespace TheOmenDen.Shared.Extensions;
public static class EnumerationFlagConstructExtensions
{
    public static Boolean IsEnumerationFlagConstruct<T>() => IsEnumerationFlagConstruct(typeof(T)).isConstruct;

    public static (Boolean isConstruct, Type[] genericArguments) IsEnumerationFlagConstruct(this Type? type)
    {
        if (type is null || type.IsAbstract || type.IsGenericTypeDefinition)
        {
            return (false, Array.Empty<Type>());
        }

        do
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(EnumerationBaseFlag<,>))
            {
                return (true, type.GetGenericArguments());
            }
            type = type.BaseType;
        }
        while (type is not null);

        return (false, Array.Empty<Type>());
    }

    public static (Boolean isSuccessful, IEnumerable<TKey> enumerations) TryParseByName<TKey, TValue>(this Dictionary<String, TKey> namedKeys, String names)
        where TKey : EnumerationBaseFlag<TKey, TValue>
        where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        var namesList = names.Replace(' ', '\0')
            .Trim()
            .Split(',');

        Array.Sort(namesList);

        var enumerations =  new List<TKey>(namedKeys.Values
            .Select(item => new { item, result = Array.BinarySearch(namesList, item.Name) })
            .Where(t => t.result >= 0)
            .Select(t => t.item));

        return enumerations.Any() 
            ? (true, enumerations) 
            : (false, Enumerable.Empty<TKey>());
    }
}
