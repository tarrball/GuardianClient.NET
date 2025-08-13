using GuardianClient.Options.Search;
using Shouldly;

namespace GuardianClient.Tests;

[TestClass]
public class SearchAsyncTests : TestBase
{
    [TestMethod]
    public async Task SearchAsync_WithQuery_ReturnsResults()
    {
        var result = await Client.SearchAsync(new SearchOptions
        {
            Query = "climate change",
            PageOptions = new PageOptions { PageSize = 5 }
        });

        result.ShouldNotBeNull();
        result.Status.ShouldBe("ok");
        result.Results.Count.ShouldBeGreaterThan(0);
        result.Results.Count.ShouldBeLessThanOrEqualTo(5);

        var firstItem = result.Results.First();
        firstItem.Id.ShouldNotBeNullOrEmpty();
        firstItem.WebTitle.ShouldNotBeNullOrEmpty();
        firstItem.WebUrl.ShouldNotBeNullOrEmpty();
        firstItem.ApiUrl.ShouldNotBeNullOrEmpty();

        Console.WriteLine($"Found {result.Results.Count} articles about climate change");
        Console.WriteLine($"First article: {firstItem.WebTitle}");
        Console.WriteLine($"Published: {firstItem.WebPublicationDate}");
    }

    [TestMethod]
    public async Task SearchAsync_WithNonExistentQuery_ReturnsNoResults()
    {
        var result = await Client.SearchAsync(new SearchOptions
        {
            Query = "xyzabc123nonexistentquery456"
        });

        result.ShouldNotBeNull();
        result.Status.ShouldBe("ok");
        result.Results.Count.ShouldBe(0);
    }

    [TestMethod]
    public async Task SearchAsync_WithNoOptions_ReturnsDefaultResults()
    {
        var result = await Client.SearchAsync();

        result.ShouldNotBeNull();
        result.Status.ShouldBe("ok");
        result.Results.Count.ShouldBeGreaterThan(0);
        result.Results.Count.ShouldBeLessThanOrEqualTo(10);

        Console.WriteLine($"Default search returned {result.Results.Count} results");
    }

    [TestMethod]
    public async Task SearchAsync_WithFilterOptions_ReturnsFilteredResults()
    {
        var result = await Client.SearchAsync(new SearchOptions
        {
            Query = "technology",
            FilterOptions = new FilterOptions
            {
                Section = "technology"
            },
            PageOptions = new PageOptions { PageSize = 3 }
        });

        result.ShouldNotBeNull();
        result.Status.ShouldBe("ok");
        result.Results.Count.ShouldBeGreaterThan(0);

        // Check that results are from technology section
        foreach (var item in result.Results)
        {
            item.SectionId.ShouldBe("technology");
        }

        Console.WriteLine($"Found {result.Results.Count} technology articles");
    }

    [TestMethod]
    public async Task SearchAsync_WithOrderOptions_ReturnsOrderedResults()
    {
        var result = await Client.SearchAsync(new SearchOptions
        {
            Query = "sports",
            OrderOptions = new OrderOptions
            {
                OrderBy = ContentOrder.Oldest
            },
            PageOptions = new PageOptions { PageSize = 2 }
        });

        result.ShouldNotBeNull();
        result.Status.ShouldBe("ok");
        result.Results.Count.ShouldBeGreaterThan(0);

        // Oldest first means first result should be older than or equal to second
        if (result.Results.Count >= 2)
        {
            var first = result.Results.First();
            var second = result.Results.Skip(1).First();

            if (first.WebPublicationDate.HasValue && second.WebPublicationDate.HasValue)
            {
                first.WebPublicationDate.Value.ShouldBeLessThanOrEqualTo(second.WebPublicationDate.Value);
            }
        }

        Console.WriteLine($"Found {result.Results.Count} sports articles (oldest first)");
    }

    [TestMethod]
    public async Task SearchAsync_WithAdditionalInformation_ReturnsEnhancedResults()
    {
        var result = await Client.SearchAsync(new SearchOptions
        {
            Query = "environment",
            PageOptions = new PageOptions { PageSize = 2 },
            AdditionalInformationOptions = new AdditionalInformationOptions
            {
                ShowFields =
                    [ContentField.Headline, ContentField.Thumbnail],
                ShowTags = [ContentTag.Keyword, ContentTag.Tone]
            }
        });

        result.ShouldNotBeNull();
        result.Status.ShouldBe("ok");
        result.Results.Count.ShouldBeGreaterThan(0);

        var firstItem = result.Results.First();
        firstItem.Fields.ShouldNotBeNull();
        firstItem.Tags.ShouldNotBeNull();

        Console.WriteLine($"Enhanced search returned {result.Results.Count} environment articles");
        Console.WriteLine($"First article has {firstItem.Tags.Count} tags");
    }
}
