namespace TheOmenDen.Shared.Generators.Json;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, GenerationMode = JsonSourceGenerationMode.Default, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = true)]
[JsonSerializable(typeof(OperationOutcome))]
[JsonSerializable(typeof(ApiResponse))]
public partial class ApiResponseContext: JsonSerializerContext
{
}
