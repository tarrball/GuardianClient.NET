using System.Diagnostics.CodeAnalysis;

namespace GuardianClient.Options.Search;

/// <summary>
/// Options for requesting additional information to be included with search results.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class GuardianApiContentAdditionalInformationOptions
{
    /// <summary>
    /// Add fields associated with the content such as headline, body, thumbnail, etc.
    /// </summary>
    public ShowFieldsOption[]? ShowFields { get; set; }

    /// <summary>
    /// Add associated metadata tags such as contributor, keyword, tone, etc.
    /// </summary>
    public ShowTagsOption[]? ShowTags { get; set; }

    /// <summary>
    /// Add associated media elements such as images, audio, and video.
    /// </summary>
    public ShowElementsOption[]? ShowElements { get; set; }

    /// <summary>
    /// Add associated reference data such as ISBNs, IMDB IDs, author references, etc.
    /// </summary>
    public ShowReferencesOption[]? ShowReferences { get; set; }


    /// <summary>
    /// Add associated blocks (single block for content, one or more for liveblogs).
    /// <para>Supported values:</para>
    /// <list type="bullet">
    /// <item><description>"main" - Main content block</description></item>
    /// <item><description>"body" - Body content blocks</description></item>
    /// <item><description>"all" - All blocks</description></item>
    /// <item><description>"body:latest" - Latest body blocks (default limit: 20)</description></item>
    /// <item><description>"body:latest:10" - Latest 10 body blocks</description></item>
    /// <item><description>"body:oldest" - Oldest body blocks</description></item>
    /// <item><description>"body:oldest:10" - Oldest 10 body blocks</description></item>
    /// <item><description>"body:&lt;block-id&gt;" - Specific block by ID</description></item>
    /// <item><description>"body:around:&lt;block-id&gt;" - Block and 20 blocks around it</description></item>
    /// <item><description>"body:around:&lt;block-id&gt;:10" - Block and 10 blocks around it</description></item>
    /// <item><description>"body:key-events" - Key event blocks</description></item>
    /// <item><description>"body:published-since:&lt;timestamp&gt;" - Blocks since timestamp (e.g., "body:published-since:1556529318000")</description></item>
    /// </list>
    /// </summary>
    public string[]? ShowBlocks { get; set; }
}
