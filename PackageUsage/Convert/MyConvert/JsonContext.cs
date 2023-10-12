using System.Text.Json.Serialization;
using DemoData.Models;

namespace MyConvert;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Person))]
[JsonSerializable(typeof(User))]
internal partial class JsonContext : JsonSerializerContext
{
}
