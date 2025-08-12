namespace GuardianClient.Options.Search;

/// <summary>
/// Options for filtering search results by date ranges.
/// </summary>
public class GuardianApiContentDateOptions
{
    /// <summary>
    /// Return only content published on or after that date.
    /// Example: 2014-02-16.
    /// </summary>
    public DateOnly FromDate { get; set; }

    /// <summary>
    /// Return only content published on or before that date.
    /// Example: 2014-02-17.
    /// </summary>
    public DateOnly ToDate { get; set; }

    /// <summary>
    /// Changes which type of date is used to filter the results using FromDate and ToDate.
    /// Accepted values: "published" (default - the date the content has been last published), "first-publication" (the date the content has been first published), "newspaper-edition" (the date the content appeared in print), "last-modified" (the date the content was last updated).
    /// </summary>
    public string? UseDate { get; set; }
}
