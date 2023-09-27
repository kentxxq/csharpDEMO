using System.Text.Json;
using BenchmarkDotNet.Attributes;
using JsonSourceGenerator;

namespace MyBenchmark;

public class JsonSourceGeneratorBenchmark
{
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };
    private readonly Person _person = new() { Name = "ken", Age = 29 };
    private string _text = default!;

    [Benchmark(Baseline = true)]
    public void NormalTest()
    {
        _text = JsonSerializer.Serialize(_person, _options);
        JsonSerializer.Deserialize<Person>(_text);
    }

    [Benchmark]
    public void SourceGeneratorTest()
    {
        _text = JsonSerializer.Serialize(_person, PersonJsonContext.Default.Person);
        JsonSerializer.Deserialize(_text, PersonJsonContext.Default.Person);
    }
}