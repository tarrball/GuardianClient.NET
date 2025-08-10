using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class Section
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("webTitle")]
    public string WebTitle { get; set; } = string.Empty;

    [JsonPropertyName("webUrl")]
    public string? WebUrl { get; set; }

    [JsonPropertyName("apiUrl")]
    public string? ApiUrl { get; set; }

    [JsonPropertyName("editions")]
    public List<Edition>? Editions { get; set; }
}