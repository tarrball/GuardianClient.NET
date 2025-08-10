using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class BaseResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("userTier")]
    public string? UserTier { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}