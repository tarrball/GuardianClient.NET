namespace GuardianClient.Options.Search;

/// <summary>
/// Specifies which type of date is used to order the results.
/// </summary>
public enum ContentType
{
    /// <summary>
    /// The date the content appeared on the web. Default.
    /// </summary>
    Published,

    /// <summary>
    /// The date the content appeared in print.
    /// </summary>
    NewspaperEdition,

    /// <summary>
    /// The date the content was last updated.
    /// </summary>
    LastModified
}
