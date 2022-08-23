
namespace TheOmenDen.Shared.Guards.Templates;

internal static class Messages
{
    public const string BaseParameterName = "parameter";

    public const string NullValueTemplate = @"[{0}] value cannot be Null";

    public const string PreconditionTemplate = "Preconditin not met.";

    public const string EmptyCollectionTemplate = @"[{0}] should contain at least one element.";

    public const string NotNullOrEmptyTemplate = @"[{0}] cannot be Null or Empty";

    public const string NotNullOrWhitespaceTemplate = @"[{0}] cannot be Null, Whitespace, or Empty";

    public const string NotMatchedByPatternTemplate = @"[{0}] does not match the pattern provided [{1}]";

    public const string NotEqualToTemplate = @"Equality was not met";
}