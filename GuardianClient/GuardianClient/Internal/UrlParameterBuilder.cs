using GuardianClient.Options.Search;

namespace GuardianClient.Internal;

internal static class UrlParameterBuilder
{
    internal static void AddParameterIfNotEmpty(List<string> parameters, string parameterName, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            parameters.Add($"{parameterName}={Uri.EscapeDataString(value)}");
        }
    }

    internal static void AddParameterIfHasValue<T>(List<string> parameters, string parameterName, T? value) where T : struct
    {
        if (value.HasValue)
        {
            parameters.Add($"{parameterName}={value.Value}");
        }
    }

    internal static void AddParameterIfAny(List<string> parameters, string parameterName, string[]? values)
    {
        if (values is { Length: > 0 })
        {
            parameters.Add($"{parameterName}={string.Join(",", values.Select(Uri.EscapeDataString))}");
        }
    }

    internal static void AddParameterIfAny<T>(List<string> parameters, string parameterName, T[]? values, Func<T, string> converter)
    {
        if (values is { Length: > 0 })
        {
            parameters.Add($"{parameterName}={string.Join(",", values.Select(v => Uri.EscapeDataString(converter(v))))}");
        }
    }

    internal static void AddQueryParameters(SearchOptions options, List<string> parameters)
    {
        AddParameterIfNotEmpty(parameters, "q", options.Query);
        AddParameterIfAny(parameters, "query-fields", options.QueryFields);
    }

    internal static void AddFilterParameters(FilterOptions filterOptions, List<string> parameters)
    {
        AddParameterIfNotEmpty(parameters, "section", filterOptions.Section);
        AddParameterIfNotEmpty(parameters, "reference", filterOptions.Reference);
        AddParameterIfNotEmpty(parameters, "reference-type", filterOptions.ReferenceType);
        AddParameterIfNotEmpty(parameters, "tag", filterOptions.Tag);
        AddParameterIfNotEmpty(parameters, "rights", filterOptions.Rights);
        AddParameterIfNotEmpty(parameters, "ids", filterOptions.Ids);
        AddParameterIfNotEmpty(parameters, "production-office", filterOptions.ProductionOffice);
        AddParameterIfNotEmpty(parameters, "lang", filterOptions.Language);

        AddParameterIfHasValue(parameters, "star-rating", filterOptions.StarRating);
    }

    internal static void AddDateParameters(DateOptions dateOptions, List<string> parameters)
    {
        if (dateOptions.FromDate != default)
        {
            parameters.Add($"from-date={dateOptions.FromDate:yyyy-MM-dd}");
        }

        if (dateOptions.ToDate != default)
        {
            parameters.Add($"to-date={dateOptions.ToDate:yyyy-MM-dd}");
        }

        AddParameterIfNotEmpty(parameters, "use-date", dateOptions.UseDate);
    }

    internal static void AddPageParameters(PageOptions pageOptions, List<string> parameters)
    {
        if (pageOptions.Page > 0)
        {
            parameters.Add($"page={pageOptions.Page}");
        }

        if (pageOptions.PageSize > 0)
        {
            parameters.Add($"page-size={pageOptions.PageSize}");
        }
    }

    internal static void AddOrderParameters(OrderOptions orderOptions, List<string> parameters)
    {
        if (orderOptions.OrderBy.HasValue)
        {
            var orderByValue = orderOptions.OrderBy.Value switch
            {
                ContentOrder.Newest => "newest",
                ContentOrder.Oldest => "oldest",
                ContentOrder.Relevance => "relevance",
                _ => "newest"
            };
            parameters.Add($"order-by={orderByValue}");
        }

        if (orderOptions.OrderDate.HasValue)
        {
            var orderDateValue = orderOptions.OrderDate.Value switch
            {
                ContentType.Published => "published",
                ContentType.NewspaperEdition => "newspaper-edition",
                ContentType.LastModified => "last-modified",
                _ => "published"
            };
            parameters.Add($"order-date={orderDateValue}");
        }
    }

    internal static void AddAdditionalInformationParameters(
        AdditionalInformationOptions additionalOptions,
        List<string> parameters
    )
    {
        AddParameterIfAny(parameters, "show-fields", additionalOptions.ShowFields, f => f.ToApiString());
        AddParameterIfAny(parameters, "show-tags", additionalOptions.ShowTags, t => t.ToApiString());
        AddParameterIfAny(parameters, "show-elements", additionalOptions.ShowElements, e => e.ToApiString());
        AddParameterIfAny(parameters, "show-references", additionalOptions.ShowReferences, r => r.ToApiString());
        AddParameterIfAny(parameters, "show-blocks", additionalOptions.ShowBlocks);
    }
}