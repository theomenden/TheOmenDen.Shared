namespace TheOmenDen.Shared.Extensions;
/// <summary>
/// A set of extensions on the <see cref="Type"/> that will provide ways to access inheritance between various types
/// </summary>
public static class TypeExtensions
{
    #region Inheritance Check Helpers
    /// <summary>
    /// Checks for inheritance between the supplied <paramref name="typeToCheck"/> and the <paramref name="baseType"/> in question.
    /// </summary>
    /// <param name="typeToCheck">The type that we want to check inheritance</param>
    /// <param name="baseType">The type we pass as the potential ancestor</param>
    /// <returns><see langword="true"/> when <paramref name="typeToCheck"/> is derived from <paramref name="baseType"/>; <see langword="false"/> otherwise</returns>
    public static Boolean IsDerivedFrom(Type typeToCheck, Type baseType)
    {
        var currentType = typeToCheck.BaseType;

        while (currentType is not null && currentType != typeof(object))
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == baseType)
            {
                return true;
            }

            currentType = currentType.BaseType;
        }

        return false;
    }

    /// <summary>
    /// Checks for inheritance between the supplied <paramref name="typeToCheck"/> and the <paramref name="baseType"/> in question.
    /// </summary>
    /// <param name="typeToCheck">The type that we want to check inheritance</param>
    /// <param name="baseType">The type we pass as the potential ancestor</param>
    /// <returns><see langword="true"/> when <paramref name="typeToCheck"/> is derived from <paramref name="baseType"/>; <see langword="false"/> otherwise</returns>
    public static Boolean IsDerived(this Type typeToCheck, Type baseType)
    => IsDerivedFrom(typeToCheck, baseType);
    #endregion

    /// <summary>
    /// Retrieves the generic constraint from the supplied <paramref name="type"/> using the <paramref name="baseType"/>'s dependency in question
    /// </summary>
    /// <param name="type">The type that we want to retrieve a generic argument from</param>
    /// <param name="baseType">The type we pass as the potential ancestor</param>
    /// <returns><see cref="Type"/> if a generic constraint is found; <see langword="null"/> otherwise</returns>
    /// <remarks><paramref name="type"/> must inherit from <paramref name="baseType"/></remarks>
    public static Type? GetGenericTypeValue(Type type, Type baseType)
    {
        if (IsDerivedFrom(type, baseType) && type.BaseType is Type currentType)
        {
            return currentType.GenericTypeArguments[1];
        }

        return null;
    }

    public static IEnumerable<TField> GetTypeFields<TField>(this Type type)
    => type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
           .Where(f => type.IsAssignableFrom(f.FieldType))
           .Select(f => (TField)f.GetValue(null))
           .ToArray();
}