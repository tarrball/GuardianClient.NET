namespace GuardianClient.Options.Search;

/// <summary>
/// Specifies the order in which search results should be returned.
/// </summary>
public enum ContentOrder
{
    /// <summary>
    /// Order by newest content first. Default in all other cases.
    /// </summary>
    Newest,

    /// <summary>
    /// Order by oldest content first.
    /// </summary>
    Oldest,

    /// <summary>
    /// Order by relevance to search query. Default where q parameter is specified.
    /// </summary>
    Relevance
}
