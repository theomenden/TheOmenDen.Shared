namespace TheOmenDen.Shared.Extensions;
public static class EnumerationFlagConstructExtensions
{
    public static Boolean IsEnumerationFlagConstruct<T>() => IsEnumerationFlagConstruct(typeof(T)).isConstruct;

    public static (Boolean isConstruct, Type[] genericArguments) IsEnumerationFlagConstruct(this Type type)
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
        var enumerations = new List<TKey>(namedKeys.Count);

        var namesList = names.Replace(" ", String.Empty)
            .Trim()
            .Split(',');

        Array.Sort(namesList);
        
        enumerations.AddRange(from item in namedKeys.Values
                              let result = Array.BinarySearch(namesList, item.Name)
                              where result >= 0
                              select item);

        return !enumerations.Any() 
            ? (false, Enumerable.Empty<TKey>()) 
            : (true, enumerations);
    }
}
