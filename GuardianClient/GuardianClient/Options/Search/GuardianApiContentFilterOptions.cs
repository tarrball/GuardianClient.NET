using System.Diagnostics.CodeAnalysis;

namespace GuardianClient.Options.Search;

/// <summary>
/// Options for filtering content search results by various criteria.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class GuardianApiContentFilterOptions
{
    /// <summary>
    /// Return only content in those sections. Supports boolean operators.
    /// Example: "football".
    /// </summary>
    public string? Section { get; set; }

    /// <summary>
    /// Return only content with those references. Supports boolean operators.
    /// Example: "isbn/9780718178949".
    /// </summary>
    public string? Reference { get; set; }

    /// <summary>
    /// Return only content with references of those types. Supports boolean operators.
    /// Example: "isbn".
    /// </summary>
    public string? ReferenceType { get; set; }

    /// <summary>
    /// Return only content with those tags. Supports boolean operators.
    /// Example: "technology/apple".
    /// </summary>
    public string? Tag { get; set; }

    /// <summary>
    /// Return only content with those rights. Does not support boolean operators.
    /// Accepted values: "syndicatable", "subscription-databases".
    /// </summary>
    public string? Rights { get; set; }

    /// <summary>
    /// Return only content with those IDs. Does not support boolean operators.
    /// Example: "technology/2014/feb/17/flappy-bird-clones-apple-google".
    /// </summary>
    public string? Ids { get; set; }

    /// <summary>
    /// Return only content from those production offices. Supports boolean operators.
    /// Example: "aus".
    /// </summary>
    public string? ProductionOffice { get; set; }

    /// <summary>
    /// Return only content in those languages. Supports boolean operators.
    /// Accepts ISO language codes. Examples: "en", "fr".
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// Return only content with a given star rating. Does not support boolean operators.
    /// Accepted values: 1 to 5.
    /// </summary>
    public int? StarRating { get; set; }
}
