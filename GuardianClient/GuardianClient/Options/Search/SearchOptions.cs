using System.Diagnostics.CodeAnalysis;

namespace GuardianClient.Options.Search;

/// <summary>
/// Options for searching content using the Guardian API.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class SearchOptions
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
    public AdditionalInformationOptions AdditionalInformationOptions { get; set; } = new();

    /// <summary>
    /// Options for filtering search results by various criteria.
    /// </summary>
    public FilterOptions FilterOptions { get; set; } = new();

    /// <summary>
    /// Options for filtering search results by date ranges.
    /// </summary>
    public DateOptions DateOptions { get; set; } = new();

    /// <summary>
    /// Options for controlling pagination of search results.
    /// </summary>
    public PageOptions PageOptions { get; set; } = new();

    /// <summary>
    /// Options for controlling the ordering of search results.
    /// </summary>
    public OrderOptions OrderOptions { get; set; } = new();
}
