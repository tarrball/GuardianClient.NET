using GuardianClient.Options.Search;

namespace GuardianClient.Tests;

using Shouldly;

[TestClass]
public class GuardianApiClientTests : TestBase
{
    [TestMethod]
    public async Task SearchAsyncSmokeTest()
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
    public async Task SearchAsyncWithNoResults()
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
    public async Task GetItemAsyncSmokeTest()
    {
        var searchResult = await ApiClient.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "technology",
            PageOptions = new GuardianApiContentPageOptions { PageSize = 1 }
        });
        searchResult.ShouldNotBeNull("Search should return results");
        searchResult.Results.Count.ShouldBe(1, "Should return exactly one result");

        var contentItem = searchResult.Results.First();
        var itemId = contentItem.Id;
        var singleItemResult = await ApiClient.GetItemAsync(itemId,
            new GuardianApiContentAdditionalInformationOptions { ShowFields = [ShowFieldsOption.Body] });

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
}
