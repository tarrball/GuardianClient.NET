using GuardianClient.Options.Search;
using Shouldly;

namespace GuardianClient.Tests;

[TestClass]
public class SearchAsyncTests : TestBase
{
    [TestMethod]
    public async Task SearchAsync_WithQuery_ReturnsResults()
    {
        var result = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "climate change",
            PageOptions = new GuardianApiContentPageOptions { PageSize = 5 }
        });

        result.ShouldNotBeNull("Search result should not be null");
        result.Status.ShouldBe("ok", "API response status should be 'ok'");
        result.Results.Count.ShouldBeGreaterThan(0, "Should return at least one result");
        result.Results.Count.ShouldBeLessThanOrEqualTo(5, "Should not return more than requested page size");

        var firstItem = result.Results.First();
        firstItem.Id.ShouldNotBeNullOrEmpty("Content item should have an ID");
        firstItem.WebTitle.ShouldNotBeNullOrEmpty("Content item should have a title");
        firstItem.WebUrl.ShouldNotBeNullOrEmpty("Content item should have a web URL");
        firstItem.ApiUrl.ShouldNotBeNullOrEmpty("Content item should have an API URL");

        Console.WriteLine($"Found {result.Results.Count} articles about climate change");
        Console.WriteLine($"First article: {firstItem.WebTitle}");
        Console.WriteLine($"Published: {firstItem.WebPublicationDate}");
    }

    [TestMethod]
    public async Task SearchAsync_WithNonExistentQuery_ReturnsNoResults()
    {
        var result = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "xyzabc123nonexistentquery456"
        });

        result.ShouldNotBeNull("Search result should not be null even with no matches");
        result.Status.ShouldBe("ok", "API response status should be 'ok'");
        result.Results.Count.ShouldBe(0, "Should return zero results for non-existent query");
    }

    [TestMethod]
    public async Task SearchAsync_WithNoOptions_ReturnsDefaultResults()
    {
        var result = await ApiClient.SearchAsync();

        result.ShouldNotBeNull("Search result should not be null");
        result.Status.ShouldBe("ok", "API response status should be 'ok'");
        result.Results.Count.ShouldBeGreaterThan(0, "Should return results with default options");
        result.Results.Count.ShouldBeLessThanOrEqualTo(10, "Default page size should be 10 or less");

        Console.WriteLine($"Default search returned {result.Results.Count} results");
    }

    [TestMethod]
    public async Task SearchAsync_WithFilterOptions_ReturnsFilteredResults()
    {
        var result = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "technology",
            FilterOptions = new GuardianApiContentFilterOptions
            {
                Section = "technology"
            },
            PageOptions = new GuardianApiContentPageOptions { PageSize = 3 }
        });

        result.ShouldNotBeNull("Search result should not be null");
        result.Status.ShouldBe("ok", "API response status should be 'ok'");
        result.Results.Count.ShouldBeGreaterThan(0, "Should return technology results");

        // Check that results are from technology section
        foreach (var item in result.Results)
        {
            item.SectionId.ShouldBe("technology", "All results should be from technology section");
        }

        Console.WriteLine($"Found {result.Results.Count} technology articles");
    }

    [TestMethod]
    public async Task SearchAsync_WithOrderOptions_ReturnsOrderedResults()
    {
        var result = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "sports",
            OrderOptions = new GuardianApiContentOrderOptions
            {
                OrderBy = GuardianApiContentOrderBy.Oldest
            },
            PageOptions = new GuardianApiContentPageOptions { PageSize = 2 }
        });

        result.ShouldNotBeNull("Search result should not be null");
        result.Status.ShouldBe("ok", "API response status should be 'ok'");
        result.Results.Count.ShouldBeGreaterThan(0, "Should return sports results");

        // Oldest first means first result should be older than or equal to second
        if (result.Results.Count >= 2)
        {
            var first = result.Results.First();
            var second = result.Results.Skip(1).First();

            if (first.WebPublicationDate.HasValue && second.WebPublicationDate.HasValue)
            {
                first.WebPublicationDate.Value.ShouldBeLessThanOrEqualTo(second.WebPublicationDate.Value,
                    "Results should be ordered oldest first");
            }
        }

        Console.WriteLine($"Found {result.Results.Count} sports articles (oldest first)");
    }

    [TestMethod]
    public async Task SearchAsync_WithAdditionalInformation_ReturnsEnhancedResults()
    {
        var result = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "environment",
            PageOptions = new GuardianApiContentPageOptions { PageSize = 2 },
            AdditionalInformationOptions = new GuardianApiContentAdditionalInformationOptions
            {
                ShowFields = [GuardianApiContentShowFieldsOption.Headline, GuardianApiContentShowFieldsOption.Thumbnail],
                ShowTags = [GuardianApiContentShowTagsOption.Keyword, GuardianApiContentShowTagsOption.Tone]
            }
        });

        result.ShouldNotBeNull("Search result should not be null");
        result.Status.ShouldBe("ok", "API response status should be 'ok'");
        result.Results.Count.ShouldBeGreaterThan(0, "Should return environment results");

        var firstItem = result.Results.First();
        firstItem.Fields.ShouldNotBeNull("Fields should be populated");
        firstItem.Tags.ShouldNotBeNull("Tags should be populated");

        Console.WriteLine($"Enhanced search returned {result.Results.Count} environment articles");
        Console.WriteLine($"First article has {firstItem.Tags.Count} tags");
    }
}
