namespace GuardianClient.Options.Search;

public class GuardianApiContentAdditionalInformationOptions
{
    /// <summary>
    ///
    /// </summary>
    public string[]? ShowFields { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string[]? ShowTags { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string[]? ShowElements { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string[]? ShowReferences { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string[]? ShowBlocks { get; set; }
}

// TODO additional information table below shows all the options that are available in the Show* methods, so
// we could strongly type those.
