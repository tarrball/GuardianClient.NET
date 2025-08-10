using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class Block
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("bodyHtml")]
    public string? BodyHtml { get; set; }

    [JsonPropertyName("bodyTextSummary")]
    public string? BodyTextSummary { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("attributes")]
    public Dictionary<string, object>? Attributes { get; set; }

    [JsonPropertyName("published")]
    public bool Published { get; set; }

    [JsonPropertyName("createdDate")]
    public DateTime? CreatedDate { get; set; }

    [JsonPropertyName("firstPublishedDate")]
    public DateTime? FirstPublishedDate { get; set; }

    [JsonPropertyName("publishedDate")]
    public DateTime? PublishedDate { get; set; }

    [JsonPropertyName("lastModifiedDate")]
    public DateTime? LastModifiedDate { get; set; }

    [JsonPropertyName("contributors")]
    public List<string>? Contributors { get; set; }

    [JsonPropertyName("elements")]
    public List<Element>? Elements { get; set; }
}