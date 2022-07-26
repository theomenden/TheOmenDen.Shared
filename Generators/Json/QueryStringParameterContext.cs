using TheOmenDen.Shared.Parameters;

namespace TheOmenDen.Shared.Generators.Json;

[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, GenerationMode = JsonSourceGenerationMode.Default, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = true)]
[JsonSerializable(typeof(QueryStringParameters))]
public partial class QueryStringParameterContext : JsonSerializerContext
{}

