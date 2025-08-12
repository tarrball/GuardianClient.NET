using GuardianClient.Options.Search;
using Shouldly;

namespace GuardianClient.Tests;

[TestClass]
public class GetItemAsyncTests : TestBase
{
    [TestMethod]
    public async Task GetItemAsync_WithValidId_ReturnsItem()
    {
        // First get a valid item ID from search
        var searchResult = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "technology",
            PageOptions = new GuardianApiContentPageOptions { PageSize = 1 }
        });
        searchResult.ShouldNotBeNull("Search should return results");
        searchResult.Results.Count.ShouldBe(1, "Should return exactly one result");

        var contentItem = searchResult.Results.First();
        var itemId = contentItem.Id;

        // Now get the specific item
        var singleItemResult = await ApiClient.GetItemAsync(itemId,
            new GuardianApiContentAdditionalInformationOptions { ShowFields = [GuardianApiContentShowFieldsOption.Body] });

        singleItemResult.ShouldNotBeNull("GetItem result should not be null");
        singleItemResult.Status.ShouldBe("ok", "API response status should be 'ok'");
        singleItemResult.Content.ShouldNotBeNull("Content should not be null");
        singleItemResult.Content.Id.ShouldBe(itemId, "Returned content ID should match requested ID");
        singleItemResult.Content.WebTitle.ShouldNotBeNullOrEmpty("Content should have a title");
        singleItemResult.Content.Fields.ShouldNotBeNull("Fields should be populated when ShowFields is specified");

        Console.WriteLine($"Retrieved item: {singleItemResult.Content.WebTitle}");
        Console.WriteLine($"Item ID: {singleItemResult.Content.Id}");
        Console.WriteLine($"Published: {singleItemResult.Content.WebPublicationDate}");

        if (!string.IsNullOrEmpty(singleItemResult.Content.Fields.Body))
        {
            Console.WriteLine($"Body length: {singleItemResult.Content.Fields.Body.Length} characters");
            Console.WriteLine(
                $"Body preview: {singleItemResult.Content.Fields.Body[..Math.Min(200, singleItemResult.Content.Fields.Body.Length)]}...");
        }
    }

    [TestMethod]
    public async Task GetItemAsync_WithInvalidId_ThrowsException()
    {
        var invalidId = "invalid/article/id/that/does/not/exist";

        var exception = await Should.ThrowAsync<HttpRequestException>(async () =>
        {
            await ApiClient.GetItemAsync(invalidId);
        });

        Console.WriteLine($"Expected exception for invalid ID: {exception.Message}");
    }

    [TestMethod]
    public async Task GetItemAsync_WithNullId_ThrowsArgumentException()
    {
        var exception = await Should.ThrowAsync<ArgumentException>(async () =>
        {
            await ApiClient.GetItemAsync(null!);
        });

        Console.WriteLine($"Expected exception for null ID: {exception.Message}");
    }

    [TestMethod]
    public async Task GetItemAsync_WithEmptyId_ThrowsArgumentException()
    {
        var exception = await Should.ThrowAsync<ArgumentException>(async () =>
        {
            await ApiClient.GetItemAsync("");
        });

        Console.WriteLine($"Expected exception for empty ID: {exception.Message}");
    }

    [TestMethod]
    public async Task GetItemAsync_WithShowFields_ReturnsEnhancedContent()
    {
        // Get a valid item ID
        var searchResult = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "politics",
            PageOptions = new GuardianApiContentPageOptions { PageSize = 1 }
        });

        var itemId = searchResult.Results.First().Id;

        // Request with multiple fields
        var result = await ApiClient.GetItemAsync(itemId,
            new GuardianApiContentAdditionalInformationOptions
            {
                ShowFields =
                [
                    GuardianApiContentShowFieldsOption.Headline,
                    GuardianApiContentShowFieldsOption.Body,
                    GuardianApiContentShowFieldsOption.Byline,
                    GuardianApiContentShowFieldsOption.Thumbnail
                ]
            });

        result.ShouldNotBeNull();
        result.Content.Fields.ShouldNotBeNull("Fields should be populated");
        result.Content.Fields.Headline.ShouldNotBeNullOrEmpty("Headline should be populated");

        Console.WriteLine($"Enhanced content for: {result.Content.WebTitle}");
        Console.WriteLine($"Headline: {result.Content.Fields.Headline}");
        Console.WriteLine($"Has body: {!string.IsNullOrEmpty(result.Content.Fields.Body)}");
        Console.WriteLine($"Has byline: {!string.IsNullOrEmpty(result.Content.Fields.Byline)}");
    }

    [TestMethod]
    public async Task GetItemAsync_WithShowTags_ReturnsContentWithTags()
    {
        // Get a valid item ID
        var searchResult = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "sport",
            PageOptions = new GuardianApiContentPageOptions { PageSize = 1 }
        });

        var itemId = searchResult.Results.First().Id;

        // Request with tags
        var result = await ApiClient.GetItemAsync(itemId,
            new GuardianApiContentAdditionalInformationOptions
            {
                ShowTags = [GuardianApiContentShowTagsOption.Keyword, GuardianApiContentShowTagsOption.Tone, GuardianApiContentShowTagsOption.Type]
            });

        result.ShouldNotBeNull();
        result.Content.Tags.ShouldNotBeNull("Tags should be populated");
        result.Content.Tags.Count.ShouldBeGreaterThan(0, "Should have at least one tag");

        Console.WriteLine($"Content '{result.Content.WebTitle}' has {result.Content.Tags.Count} tags:");
        foreach (var tag in result.Content.Tags.Take(5)) // Show first 5 tags
        {
            Console.WriteLine($"  - {tag.WebTitle} ({tag.Type})");
        }
    }

    [TestMethod]
    public async Task GetItemAsync_WithShowElements_ReturnsContentWithElements()
    {
        // Get a valid item ID
        var searchResult = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "music",
            PageOptions = new GuardianApiContentPageOptions { PageSize = 1 }
        });

        var itemId = searchResult.Results.First().Id;

        // Request with elements
        var result = await ApiClient.GetItemAsync(itemId,
            new GuardianApiContentAdditionalInformationOptions
            {
                ShowElements = [GuardianApiContentShowElementsOption.Image, GuardianApiContentShowElementsOption.Video]
            });

        result.ShouldNotBeNull();
        // Elements might be null if the article has no media, so we don't assert their presence

        Console.WriteLine($"Content '{result.Content.WebTitle}' elements:");
        if (result.Content.Elements != null)
        {
            Console.WriteLine($"  Has {result.Content.Elements.Count} media elements");
        }
        else
        {
            Console.WriteLine("  No media elements found");
        }
    }

    [TestMethod]
    public async Task GetItemAsync_WithShowBlocks_ReturnsContentWithBlocks()
    {
        // Get a valid item ID
        var searchResult = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "news",
            PageOptions = new GuardianApiContentPageOptions { PageSize = 1 }
        });

        var itemId = searchResult.Results.First().Id;

        // Request with blocks using string array (as it's still string-based)
        var result = await ApiClient.GetItemAsync(itemId,
            new GuardianApiContentAdditionalInformationOptions
            {
                ShowBlocks = ["main", "body"]
            });

        result.ShouldNotBeNull();
        // Blocks might be null depending on content type

        Console.WriteLine($"Content '{result.Content.WebTitle}' blocks:");
        if (result.Content.Blocks != null)
        {
            Console.WriteLine($"  Requested main and body blocks");
        }
        else
        {
            Console.WriteLine("  No blocks found");
        }
    }

    [TestMethod]
    public async Task GetItemAsync_WithAllOptions_ReturnsFullyEnhancedContent()
    {
        // Get a valid item ID
        var searchResult = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "culture",
            PageOptions = new GuardianApiContentPageOptions { PageSize = 1 }
        });

        var itemId = searchResult.Results.First().Id;

        // Request with all enhancement options
        var result = await ApiClient.GetItemAsync(itemId,
            new GuardianApiContentAdditionalInformationOptions
            {
                ShowFields = [GuardianApiContentShowFieldsOption.All],
                ShowTags = [GuardianApiContentShowTagsOption.All],
                ShowElements = [GuardianApiContentShowElementsOption.All],
                ShowBlocks = ["all"]
            });

        result.ShouldNotBeNull();
        result.Content.Fields.ShouldNotBeNull("All fields should be populated");
        result.Content.Tags.ShouldNotBeNull("All tags should be populated");

        Console.WriteLine($"Fully enhanced content: {result.Content.WebTitle}");
        Console.WriteLine($"  Fields populated: Yes");
        Console.WriteLine($"  Tags count: {result.Content.Tags.Count}");
        Console.WriteLine($"  Elements: {(result.Content.Elements?.Count ?? 0)}");
        Console.WriteLine($"  Has blocks: {result.Content.Blocks != null}");
    }
}
