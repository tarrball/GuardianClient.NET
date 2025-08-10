using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class ApiResponseWrapper<T>
{
    [JsonPropertyName("response")]
    public T Response { get; set; } = default!;
}