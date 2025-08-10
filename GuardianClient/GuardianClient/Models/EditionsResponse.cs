using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class EditionsResponse : BaseResponse
{
    [JsonPropertyName("results")]
    public List<Edition> Results { get; set; } = new();
}