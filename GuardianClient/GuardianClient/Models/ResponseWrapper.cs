using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class ResponseWrapper<T>
{
    [JsonPropertyName("response")]
    public T Response { get; set; } = default!;
}
