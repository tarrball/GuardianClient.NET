using System.Diagnostics.CodeAnalysis;

namespace GuardianClient.Options.Search;

/// <summary>
/// Options for controlling the ordering of search results.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class GuardianApiContentOrderOptions
{
    /// <summary>
    /// Returns results in the specified order. Defaults to Newest in most cases, or Relevance when a query parameter is specified.
    /// </summary>
    public GuardianApiOrderBy? OrderBy { get; set; }

    /// <summary>
    /// Changes which type of date is used to order the results. Defaults to Published.
    /// </summary>
    public GuardianApiOrderDate? OrderDate { get; set; }
}
