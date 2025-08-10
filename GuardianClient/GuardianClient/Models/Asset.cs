using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class Asset
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("mimeType")]
    public string? MimeType { get; set; }

    [JsonPropertyName("file")]
    public string? File { get; set; }

    [JsonPropertyName("typeData")]
    public Dictionary<string, object>? TypeData { get; set; }
}