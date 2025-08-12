using GuardianClient.Options.Search;
using Shouldly;

namespace GuardianClient.Tests;

[TestClass]
public class GuardianApiClientIntegrationTests : TestBase
{
    [TestMethod]
    public void ApiClient_ShouldNotBeNull()
    {
        ApiClient.ShouldNotBeNull("API client should be properly initialized");
        ApiClient.ShouldBeAssignableTo<IGuardianApiClient>("API client should implement the interface");
    }

    [TestMethod]
    public async Task EndToEnd_SearchAndGetItem_WorksTogether()
    {
        // Search for content
        var searchResult = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "artificial intelligence",
            PageOptions = new GuardianApiContentPageOptions { PageSize = 1 },
            FilterOptions = new GuardianApiContentFilterOptions
            {
                Section = "technology"
            }
        });

        searchResult.ShouldNotBeNull("Search should return results");
        searchResult.Results.Count.ShouldBe(1, "Should return exactly one result");

        // Get the specific item with enhanced information
        var contentItem = searchResult.Results.First();
        var detailedResult = await ApiClient.GetItemAsync(contentItem.Id,
            new GuardianApiContentAdditionalInformationOptions
            {
                ShowFields = [ShowFieldsOption.Headline, ShowFieldsOption.Body],
                ShowTags = [ShowTagsOption.Keyword]
            });

        detailedResult.ShouldNotBeNull("Detailed result should not be null");
        detailedResult.Content!.Id.ShouldBe(contentItem.Id, "IDs should match");
        detailedResult.Content.WebTitle.ShouldBe(contentItem.WebTitle, "Titles should match");
        detailedResult.Content.Fields.ShouldNotBeNull("Detailed fields should be populated");
        detailedResult.Content.Tags.ShouldNotBeNull("Tags should be populated");

        Console.WriteLine($"End-to-end test successful:");
        Console.WriteLine($"  Search found: {contentItem.WebTitle}");
        Console.WriteLine($"  Retrieved same item with {detailedResult.Content.Tags.Count} tags");
        Console.WriteLine($"  Body content length: {detailedResult.Content.Fields.Body?.Length ?? 0} characters");
    }

    [TestMethod]
    public async Task SearchWithComplexOptions_ReturnsExpectedResults()
    {
        // Test a complex search with multiple option types
        var result = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "climate change",
            QueryFields = ["body", "headline"],
            FilterOptions = new GuardianApiContentFilterOptions
            {
                Section = "environment"
            },
            DateOptions = new GuardianApiContentDateOptions
            {
                FromDate = new DateOnly(2023, 1, 1)
            },
            PageOptions = new GuardianApiContentPageOptions
            {
                Page = 1,
                PageSize = 5
            },
            OrderOptions = new GuardianApiContentOrderOptions
            {
                OrderBy = GuardianApiContentOrderBy.Relevance
            },
            AdditionalInformationOptions = new GuardianApiContentAdditionalInformationOptions
            {
                ShowFields = [ShowFieldsOption.Headline, ShowFieldsOption.Score],
                ShowTags = [ShowTagsOption.Tone]
            }
        });

        result.ShouldNotBeNull("Complex search should return results");
        result.Status.ShouldBe("ok", "API should respond successfully");
        result.Results.Count.ShouldBeLessThanOrEqualTo(5, "Should respect page size");

        // Verify enhanced data is present
        if (result.Results.Any())
        {
            var firstItem = result.Results.First();
            firstItem.Fields.ShouldNotBeNull("Enhanced fields should be present");
            firstItem.Tags.ShouldNotBeNull("Tags should be present");

            Console.WriteLine($"Complex search returned {result.Results.Count} results");
            Console.WriteLine($"First result: {firstItem.WebTitle}");
            Console.WriteLine($"Relevance score: {firstItem.Fields.Score ?? "N/A"}");
        }
    }

    [TestMethod]
    public async Task TypeSafetyTest_EnumsMapToCorrectApiValues()
    {
        // This test ensures our enums map to the correct API values
        var result = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "test",
            PageOptions = new GuardianApiContentPageOptions { PageSize = 1 },
            AdditionalInformationOptions = new GuardianApiContentAdditionalInformationOptions
            {
                ShowFields =
                [
                    ShowFieldsOption.Headline,
                    ShowFieldsOption.TrailText,
                    ShowFieldsOption.ShowInRelatedContent
                ],
                ShowTags = [ShowTagsOption.Tone, ShowTagsOption.Type],
                ShowElements = [ShowElementsOption.Image]
            }
        });

        result.ShouldNotBeNull("Type safety test should work");
        result.Status.ShouldBe("ok", "API should accept enum-mapped values");

        if (result.Results.Any())
        {
            var item = result.Results.First();
            Console.WriteLine($"Type safety test passed for: {item.WebTitle}");
            Console.WriteLine($"  Fields populated: {item.Fields != null}");
            Console.WriteLine($"  Tags populated: {item.Tags != null}");
            Console.WriteLine($"  Elements populated: {item.Elements != null}");
        }
    }
}
