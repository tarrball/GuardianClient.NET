using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class ContentBlocks
{
    [JsonPropertyName("main")]
    public Block? Main { get; set; }

    [JsonPropertyName("body")]
    public List<Block>? Body { get; set; }
}