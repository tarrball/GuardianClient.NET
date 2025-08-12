using System.Diagnostics.CodeAnalysis;

namespace GuardianClient.Options.Search;

/// <summary>
/// Options for controlling pagination of search results.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class GuardianApiContentPageOptions
{
    /// <summary>
    /// Return only the result set from a particular page.
    /// Example: 5.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Modify the number of items displayed per page.
    /// Accepted values: 1 to 50.
    /// </summary>
    public int PageSize { get; set; }
}
