using GuardianClient.Options.Search;
using Shouldly;

namespace GuardianClient.Tests;

[TestClass]
public class GuardianApiClientIntegrationTests : TestBase
{
    [TestMethod]
    public void ApiClient_ShouldNotBeNull()
    {
        Client.ShouldNotBeNull();
        Client.ShouldBeAssignableTo<IGuardianApiClient>();
    }

    [TestMethod]
    public async Task EndToEnd_SearchAndGetItem_WorksTogether()
    {
        // Search for content
        var searchResult = await Client.SearchAsync(new SearchOptions
        {
            Query = "artificial intelligence",
            PageOptions = new PageOptions { PageSize = 1 },
            FilterOptions = new FilterOptions
            {
                Section = "technology"
            }
        });

        searchResult.ShouldNotBeNull();
        searchResult.Results.Count.ShouldBe(1);

        // Get the specific item with enhanced information
        var contentItem = searchResult.Results.First();
        var detailedResult = await Client.GetItemAsync(contentItem.Id,
            new AdditionalInformationOptions
            {
                ShowFields = [ContentField.Headline, ContentField.Body],
                ShowTags = [ContentTag.Keyword]
            });

        detailedResult.ShouldNotBeNull();
        detailedResult.Content!.Id.ShouldBe(contentItem.Id);
        detailedResult.Content.WebTitle.ShouldBe(contentItem.WebTitle);
        detailedResult.Content.Fields.ShouldNotBeNull();
        detailedResult.Content.Tags.ShouldNotBeNull();

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
        var result = await Client.SearchAsync(new SearchOptions
        {
            Query = "climate change",
            QueryFields = ["body", "headline"],
            FilterOptions = new FilterOptions
            {
                Section = "environment"
            },
            DateOptions = new DateOptions
            {
                FromDate = new DateOnly(2023, 1, 1)
            },
            PageOptions = new PageOptions
            {
                Page = 1,
                PageSize = 5
            },
            OrderOptions = new OrderOptions
            {
                OrderBy = ContentOrder.Relevance
            },
            AdditionalInformationOptions = new AdditionalInformationOptions
            {
                ShowFields = [ContentField.Headline, ContentField.Score],
                ShowTags = [ContentTag.Tone]
            }
        });

        result.ShouldNotBeNull();
        result.Status.ShouldBe("ok");
        result.Results.Count.ShouldBeLessThanOrEqualTo(5);

        // Verify enhanced data is present
        if (result.Results.Any())
        {
            var firstItem = result.Results.First();
            firstItem.Fields.ShouldNotBeNull();
            firstItem.Tags.ShouldNotBeNull();

            Console.WriteLine($"Complex search returned {result.Results.Count} results");
            Console.WriteLine($"First result: {firstItem.WebTitle}");
            Console.WriteLine($"Relevance score: {firstItem.Fields.Score ?? "N/A"}");
        }
    }

    [TestMethod]
    public async Task TypeSafetyTest_EnumsMapToCorrectApiValues()
    {
        // This test ensures our enums map to the correct API values
        var result = await Client.SearchAsync(new SearchOptions
        {
            Query = "test",
            PageOptions = new PageOptions { PageSize = 1 },
            AdditionalInformationOptions = new AdditionalInformationOptions
            {
                ShowFields =
                [
                    ContentField.Headline,
                    ContentField.TrailText,
                    ContentField.ShowInRelatedContent
                ],
                ShowTags = [ContentTag.Tone, ContentTag.Type],
                ShowElements = [ContentElement.Image]
            }
        });

        result.ShouldNotBeNull();
        result.Status.ShouldBe("ok");

        if (result.Results.Any())
        {
            var item = result.Results.First();
            Console.WriteLine($"Type safety test passed for: {item.WebTitle}");
            Console.WriteLine($"  Fields populated: {item.Fields != null}");
            Console.WriteLine($"  Tags populated: {item.Tags != null}");
            Console.WriteLine($"  Elements populated: {item.Elements != null}");
        }
    }

    [TestMethod]
    public async Task SimpleSearch()
    {
        var result = await Client.SearchAsync(new SearchOptions
        {
            AdditionalInformationOptions = new AdditionalInformationOptions
            {
                ShowFields = [ContentField.Body],
                ShowElements = [ContentElement.Image]
            },
            PageOptions = new PageOptions
            {
                Page = 0,
                PageSize = 10
            }
        });

        var body = result?.Results.First(r => r.Type == "article").Fields?.Body;

        body.ShouldNotBeNull();
    }
}
