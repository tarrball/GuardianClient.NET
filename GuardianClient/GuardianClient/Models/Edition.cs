using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class Edition
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("edition")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("webTitle")]
    public string? WebTitle { get; set; }

    [JsonPropertyName("webUrl")]
    public string? WebUrl { get; set; }

    [JsonPropertyName("apiUrl")]
    public string? ApiUrl { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }
}
