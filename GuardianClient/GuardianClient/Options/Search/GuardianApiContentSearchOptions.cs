namespace GuardianClient.Options.Search;

/// <summary>
/// Options for searching content using the Guardian API.
/// </summary>
public class GuardianApiContentSearchOptions
{
    /// <summary>
    /// Request content containing this free text. Supports AND, OR and NOT operators, and exact phrase queries using double quotes.
    /// Examples: "sausages", "pork sausages", "sausages AND (mash OR chips)", "sausages AND NOT (saveloy OR battered)".
    /// </summary>
    public string? Query { get; set; }

    /// <summary>
    /// Specify in which indexed fields query terms should be searched on.
    /// Examples: ["body"], ["body", "thumbnail"].
    /// </summary>
    public string[]? QueryFields { get; set; }

    /// <summary>
    /// Options for requesting additional information to be included with search results.
    /// </summary>
    public GuardianApiContentAdditionalInformationOptions AdditionalInformationOptions { get; set; } = new();

    /// <summary>
    /// Options for filtering search results by various criteria.
    /// </summary>
    public GuardianApiContentFilterOptions FilterOptions { get; set; } = new();

    /// <summary>
    /// Options for filtering search results by date ranges.
    /// </summary>
    public GuardianApiContentDateOptions DateOptions { get; set; } = new();

    /// <summary>
    /// Options for controlling pagination of search results.
    /// </summary>
    public GuardianApiContentPageOptions PageOptions { get; set; } = new();

    /// <summary>
    /// Options for controlling the ordering of search results.
    /// </summary>
    public GuardianApiContentOrderOptions OrderOptions { get; set; } = new();
}
