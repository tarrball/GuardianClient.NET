using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class SingleItemResponse : BaseResponse
{
    [JsonPropertyName("content")]
    public ContentItem? Content { get; set; }

    [JsonPropertyName("leadContent")]
    public List<ContentItem>? LeadContent { get; set; }

    [JsonPropertyName("storyPackage")]
    public List<ContentItem>? StoryPackage { get; set; }

    [JsonPropertyName("editorsPicks")]
    public List<ContentItem>? EditorsPicks { get; set; }

    [JsonPropertyName("mostViewed")]
    public List<ContentItem>? MostViewed { get; set; }

    [JsonPropertyName("relatedContent")]
    public List<ContentItem>? RelatedContent { get; set; }
}