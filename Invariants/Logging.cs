namespace TheOmenDen.Shared.Invariants;

/// <summary>
/// A set of LogContexts to determine where the logging statement originated
/// </summary>
public sealed class Logging
{
    public sealed class LogContexts
    {
        public const String RequestTypeProperty = "RequestType";

        public const String FromRoute = "From Route";
    }
}