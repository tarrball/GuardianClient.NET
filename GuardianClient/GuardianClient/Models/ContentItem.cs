using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class ContentItem
{
    /// <summary>
    /// The path to content.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The id of the section.
    /// </summary>
    [JsonPropertyName("sectionId")]
    public string? SectionId { get; set; }

    /// <summary>
    /// The name of the section.
    /// </summary>
    [JsonPropertyName("sectionName")]
    public string? SectionName { get; set; }

    /// <summary>
    /// The combined date and time of publication.
    /// </summary>
    [JsonPropertyName("webPublicationDate")]
    public DateTime? WebPublicationDate { get; set; }

    [JsonPropertyName("webTitle")]
    public string WebTitle { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the html content.
    /// </summary>
    [JsonPropertyName("webUrl")]
    public string WebUrl { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the raw content.
    /// </summary>
    [JsonPropertyName("apiUrl")]
    public string ApiUrl { get; set; } = string.Empty;

    [JsonPropertyName("isHosted")]
    public bool IsHosted { get; set; }

    [JsonPropertyName("pillarId")]
    public string? PillarId { get; set; }

    [JsonPropertyName("pillarName")]
    public string? PillarName { get; set; }

    [JsonPropertyName("fields")]
    public ContentFields? Fields { get; set; }

    [JsonPropertyName("tags")]
    public List<Tag>? Tags { get; set; }

    [JsonPropertyName("elements")]
    public List<Element>? Elements { get; set; }

    [JsonPropertyName("references")]
    public List<Reference>? References { get; set; }

    [JsonPropertyName("blocks")]
    public ContentBlocks? Blocks { get; set; }
}
