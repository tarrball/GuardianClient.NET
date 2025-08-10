using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class Element
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("relation")]
    public string? Relation { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("assets")]
    public List<Asset>? Assets { get; set; }
}