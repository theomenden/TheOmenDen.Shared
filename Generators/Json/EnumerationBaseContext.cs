namespace TheOmenDen.Shared.Generators.Json;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, GenerationMode = JsonSourceGenerationMode.Default, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(EnumerationBase))]
[JsonSerializable(typeof(ExceptionGravity))]
[JsonSerializable(typeof(OperationResult))]
[JsonSerializable(typeof(TraceEventIdentifiers))]
[JsonSerializable(typeof(EventIdIdentifier))]
public partial class EnumerationBaseContext: JsonSerializerContext
{}

