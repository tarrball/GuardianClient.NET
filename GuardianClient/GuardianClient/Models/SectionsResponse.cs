using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class SectionsResponse : BaseResponse
{
    [JsonPropertyName("results")]
    public List<Section> Results { get; set; } = new();
}