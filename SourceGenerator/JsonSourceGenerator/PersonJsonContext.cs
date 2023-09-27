using System.Text.Json.Serialization;

namespace JsonSourceGenerator;

[JsonSerializable(typeof(Person))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public partial class PersonJsonContext : JsonSerializerContext
{
}