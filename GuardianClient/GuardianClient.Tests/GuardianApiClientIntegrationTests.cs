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
                ShowFields = [GuardianApiContentShowFieldsOption.Headline, GuardianApiContentShowFieldsOption.Body],
                ShowTags = [GuardianApiContentShowTagsOption.Keyword]
            });

        detailedResult.ShouldNotBeNull("Detailed result should not be null");
        detailedResult.Content!.Id.ShouldBe(contentItem.Id, "IDs should match");
        detailedResult.Content.WebTitle.ShouldBe(contentItem.WebTitle, "Titles should match");
        detailedResult.Content.Fields.ShouldNotBeNull("Detailed fields should be populated");
        detailedResult.Content.Tags.ShouldNotBeNull("Tags should be populated");

        Console.WriteLine("=== END-TO-END TEST RESULTS ===");
        Console.WriteLine();
        Console.WriteLine($"SEARCH RESULT:");
        Console.WriteLine($"  Title: {contentItem.WebTitle}");
        Console.WriteLine($"  Section: {contentItem.SectionName} ({contentItem.SectionId})");
        Console.WriteLine($"  Published: {contentItem.WebPublicationDate}");
        Console.WriteLine($"  URL: {contentItem.WebUrl}");
        Console.WriteLine();
        
        Console.WriteLine($"DETAILED CONTENT:");
        Console.WriteLine($"  ID: {detailedResult.Content.Id}");
        Console.WriteLine($"  Headline: {detailedResult.Content.Fields.Headline ?? "N/A"}");
        Console.WriteLine($"  Tags: {detailedResult.Content.Tags.Count} tags");
        
        if (detailedResult.Content.Tags.Any())
        {
            Console.WriteLine($"  Tag List:");
            foreach (var tag in detailedResult.Content.Tags.Take(10)) // Show first 10 tags
            {
                Console.WriteLine($"    - {tag.WebTitle} ({tag.Type})");
            }
            if (detailedResult.Content.Tags.Count > 10)
            {
                Console.WriteLine($"    ... and {detailedResult.Content.Tags.Count - 10} more tags");
            }
        }
        
        Console.WriteLine();
        Console.WriteLine($"FULL ARTICLE BODY:");
        Console.WriteLine("==================");
        if (!string.IsNullOrEmpty(detailedResult.Content.Fields.Body))
        {
            Console.WriteLine(detailedResult.Content.Fields.Body);
        }
        else
        {
            Console.WriteLine("No body content available");
        }
        Console.WriteLine("==================");
        Console.WriteLine();
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
                ShowFields = [GuardianApiContentShowFieldsOption.Headline, GuardianApiContentShowFieldsOption.Score],
                ShowTags = [GuardianApiContentShowTagsOption.Tone]
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
                    GuardianApiContentShowFieldsOption.Headline,
                    GuardianApiContentShowFieldsOption.TrailText,
                    GuardianApiContentShowFieldsOption.ShowInRelatedContent
                ],
                ShowTags = [GuardianApiContentShowTagsOption.Tone, GuardianApiContentShowTagsOption.Type],
                ShowElements = [GuardianApiContentShowElementsOption.Image]
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
