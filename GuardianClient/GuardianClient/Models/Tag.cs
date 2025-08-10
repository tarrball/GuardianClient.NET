using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class Tag
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("webTitle")]
    public string WebTitle { get; set; } = string.Empty;

    [JsonPropertyName("webUrl")]
    public string? WebUrl { get; set; }

    [JsonPropertyName("apiUrl")]
    public string? ApiUrl { get; set; }

    [JsonPropertyName("sectionId")]
    public string? SectionId { get; set; }

    [JsonPropertyName("sectionName")]
    public string? SectionName { get; set; }

    [JsonPropertyName("references")]
    public List<Reference>? References { get; set; }
}