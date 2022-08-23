namespace TheOmenDen.Shared.Generators.Json;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, GenerationMode = JsonSourceGenerationMode.Default, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(IEnumerationBase))]
[JsonSerializable(typeof(ExceptionGravity))]
[JsonSerializable(typeof(OperationResult))]
[JsonSerializable(typeof(TraceEventIdentifiers))]
[JsonSerializable(typeof(EventIdIdentifier))]
[JsonSerializable(typeof(SuccessCode))]
[JsonSerializable(typeof(FailureCode))]
public partial class EnumerationBaseContext: JsonSerializerContext
{}

