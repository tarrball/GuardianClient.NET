using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class Reference
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}