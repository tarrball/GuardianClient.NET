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
        var searchResult = await Client.SearchAsync(new SearchOptions
        {
            Query = "technology",
            PageOptions = new PageOptions { PageSize = 1 }
        });
        searchResult.ShouldNotBeNull();
        searchResult.Results.Count.ShouldBe(1);

        var contentItem = searchResult.Results.First();
        var itemId = contentItem.Id;

        // Now get the specific item
        var singleItemResult = await Client.GetItemAsync(itemId,
            new AdditionalInformationOptions { ShowFields = [ContentField.Body] });

        singleItemResult.ShouldNotBeNull();
        singleItemResult.Status.ShouldBe("ok");
        singleItemResult.Content.ShouldNotBeNull();
        singleItemResult.Content.Id.ShouldBe(itemId);
        singleItemResult.Content.WebTitle.ShouldNotBeNullOrEmpty();
        singleItemResult.Content.Fields.ShouldNotBeNull();

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
            await Client.GetItemAsync(invalidId);
        });

        Console.WriteLine($"Expected exception for invalid ID: {exception.Message}");
    }

    [TestMethod]
    public async Task GetItemAsync_WithNullId_ThrowsArgumentException()
    {
        var exception = await Should.ThrowAsync<ArgumentException>(async () =>
        {
            await Client.GetItemAsync(null!);
        });

        Console.WriteLine($"Expected exception for null ID: {exception.Message}");
    }

    [TestMethod]
    public async Task GetItemAsync_WithEmptyId_ThrowsArgumentException()
    {
        var exception = await Should.ThrowAsync<ArgumentException>(async () =>
        {
            await Client.GetItemAsync("");
        });

        Console.WriteLine($"Expected exception for empty ID: {exception.Message}");
    }

    [TestMethod]
    public async Task GetItemAsync_WithShowFields_ReturnsEnhancedContent()
    {
        // Get a valid item ID
        var searchResult = await Client.SearchAsync(new SearchOptions
        {
            Query = "politics",
            PageOptions = new PageOptions { PageSize = 1 }
        });

        var itemId = searchResult.Results.First().Id;

        // Request with multiple fields
        var result = await Client.GetItemAsync(itemId,
            new AdditionalInformationOptions
            {
                ShowFields =
                [
                    ContentField.Headline,
                    ContentField.Body,
                    ContentField.Byline,
                    ContentField.Thumbnail
                ]
            });

        result.ShouldNotBeNull();
        result.Content.Fields.ShouldNotBeNull();
        result.Content.Fields.Headline.ShouldNotBeNullOrEmpty();

        Console.WriteLine($"Enhanced content for: {result.Content.WebTitle}");
        Console.WriteLine($"Headline: {result.Content.Fields.Headline}");
        Console.WriteLine($"Has body: {!string.IsNullOrEmpty(result.Content.Fields.Body)}");
        Console.WriteLine($"Has byline: {!string.IsNullOrEmpty(result.Content.Fields.Byline)}");
    }

    [TestMethod]
    public async Task GetItemAsync_WithShowTags_ReturnsContentWithTags()
    {
        // Get a valid item ID
        var searchResult = await Client.SearchAsync(new SearchOptions
        {
            Query = "sport",
            PageOptions = new PageOptions { PageSize = 1 }
        });

        var itemId = searchResult.Results.First().Id;

        // Request with tags
        var result = await Client.GetItemAsync(itemId,
            new AdditionalInformationOptions
            {
                ShowTags = [ContentTag.Keyword, ContentTag.Tone, ContentTag.Type]
            });

        result.ShouldNotBeNull();
        result.Content.Tags.ShouldNotBeNull();
        result.Content.Tags.Count.ShouldBeGreaterThan(0);

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
        var searchResult = await Client.SearchAsync(new SearchOptions
        {
            Query = "music",
            PageOptions = new PageOptions { PageSize = 1 }
        });

        var itemId = searchResult.Results.First().Id;

        // Request with elements
        var result = await Client.GetItemAsync(itemId,
            new AdditionalInformationOptions
            {
                ShowElements = [ContentElement.Image, ContentElement.Video]
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
        var searchResult = await Client.SearchAsync(new SearchOptions
        {
            Query = "news",
            PageOptions = new PageOptions { PageSize = 1 }
        });

        var itemId = searchResult.Results.First().Id;

        // Request with blocks using string array (as it's still string-based)
        var result = await Client.GetItemAsync(itemId,
            new AdditionalInformationOptions
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
        var searchResult = await Client.SearchAsync(new SearchOptions
        {
            Query = "culture",
            PageOptions = new PageOptions { PageSize = 1 }
        });

        var itemId = searchResult.Results.First().Id;

        // Request with all enhancement options
        var result = await Client.GetItemAsync(itemId,
            new AdditionalInformationOptions
            {
                ShowFields = [ContentField.All],
                ShowTags = [ContentTag.All],
                ShowElements = [ContentElement.All],
                ShowBlocks = ["all"]
            });

        result.ShouldNotBeNull();
        result.Content.Fields.ShouldNotBeNull();
        result.Content.Tags.ShouldNotBeNull();

        Console.WriteLine($"Fully enhanced content: {result.Content.WebTitle}");
        Console.WriteLine($"  Fields populated: Yes");
        Console.WriteLine($"  Tags count: {result.Content.Tags.Count}");
        Console.WriteLine($"  Elements: {(result.Content.Elements?.Count ?? 0)}");
        Console.WriteLine($"  Has blocks: {result.Content.Blocks != null}");
    }
}
