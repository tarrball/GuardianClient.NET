using System.Text.Json.Serialization;

namespace GuardianClient.Models;

public class ContentFields
{
    [JsonPropertyName("trailText")]
    public string? TrailText { get; set; }

    [JsonPropertyName("headline")]
    public string? Headline { get; set; }

    [JsonPropertyName("showInRelatedContent")]
    public string? ShowInRelatedContent { get; set; }

    [JsonPropertyName("body")]
    public string? Body { get; set; }

    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("hasStoryPackage")]
    public string? HasStoryPackage { get; set; }

    [JsonPropertyName("score")]
    public string? Score { get; set; }

    [JsonPropertyName("standfirst")]
    public string? Standfirst { get; set; }

    [JsonPropertyName("shortUrl")]
    public string? ShortUrl { get; set; }

    [JsonPropertyName("thumbnail")]
    public string? Thumbnail { get; set; }

    [JsonPropertyName("wordcount")]
    public string? WordCount { get; set; }

    [JsonPropertyName("commentable")]
    public string? Commentable { get; set; }

    [JsonPropertyName("isPremoderated")]
    public string? IsPremoderated { get; set; }

    [JsonPropertyName("allowUgc")]
    public string? AllowUgc { get; set; }

    [JsonPropertyName("byline")]
    public string? Byline { get; set; }

    [JsonPropertyName("publication")]
    public string? Publication { get; set; }

    [JsonPropertyName("internalPageCode")]
    public string? InternalPageCode { get; set; }

    [JsonPropertyName("productionOffice")]
    public string? ProductionOffice { get; set; }

    [JsonPropertyName("shouldHideAdverts")]
    public string? ShouldHideAdverts { get; set; }

    [JsonPropertyName("liveBloggingNow")]
    public string? LiveBloggingNow { get; set; }

    [JsonPropertyName("commentCloseDate")]
    public DateTime? CommentCloseDate { get; set; }

    [JsonPropertyName("starRating")]
    public string? StarRating { get; set; }
}