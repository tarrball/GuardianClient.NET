using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class BaseResponse
{
    /// <summary>
    /// The status of the response. It refers to the state of the API. Successful calls will receive an "ok" even if your query did not return any results.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// The user tier for the API account.
    /// </summary>
    [JsonPropertyName("userTier")]
    public string? UserTier { get; set; }

    /// <summary>
    /// The number of results available for your search overall.
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }
}
