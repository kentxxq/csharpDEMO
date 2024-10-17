using System.Text.Json.Serialization;

namespace DockerDistribution.Models;

public class ManifestList
{
    [JsonPropertyName("schemaVersion")]
    public int SchemaVersion { get; set; }

    [JsonPropertyName("mediaType")]
    public string MediaType { get; set; }

    [JsonPropertyName("manifests")]
    public List<Manifest> Manifests { get; set; }
}

public class Manifest
{
    [JsonPropertyName("mediaType")]
    public string MediaType { get; set; }

    [JsonPropertyName("digest")]
    public string Digest { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("platform")]
    public Platform Platform { get; set; }
}

public class Platform
{
    [JsonPropertyName("architecture")]
    public string Architecture { get; set; }

    [JsonPropertyName("os")]
    public string Os { get; set; }

    [JsonPropertyName("features")]
    public List<string> Features { get; set; }
}
