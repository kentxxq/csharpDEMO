using System.Text.Json.Serialization;

namespace DockerDistribution.Models;

public class DockerManifest
{
    [JsonPropertyName("schemaVersion")]
    public int SchemaVersion { get; set; }

    [JsonPropertyName("mediaType")]
    public string MediaType { get; set; }

    [JsonPropertyName("config")]
    public Config Config { get; set; }

    [JsonPropertyName("layers")]
    public List<Layer> Layers { get; set; }
}

public class Config
{
    [JsonPropertyName("mediaType")]
    public string MediaType { get; set; }

    [JsonPropertyName("digest")]
    public string Digest { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }
}

public class Layer
{
    [JsonPropertyName("mediaType")]
    public string MediaType { get; set; }

    [JsonPropertyName("digest")]
    public string Digest { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }
}
