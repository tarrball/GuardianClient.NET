using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GuardianClient.Extensions;

namespace GuardianClient.Tests;

[TestClass]
public class SearchAsyncTests
{
    private static IConfiguration? _Configuration;
    private static ServiceProvider? _ServiceProvider;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        // Build configuration with user secrets
        _Configuration = new ConfigurationBuilder()
            .AddUserSecrets<SearchAsyncTests>()
            .Build();

        // Set up service collection with DI
        var services = new ServiceCollection();

        var apiKey = _Configuration["GuardianApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException(
                "Guardian API key not found in user secrets. " +
                "Run: dotnet user-secrets set \"GuardianApiKey\" \"your-api-key-here\" " +
                "from the test project directory.");
        }

        services.AddGuardianApiClient(apiKey);

        _ServiceProvider = services.BuildServiceProvider();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _ServiceProvider?.Dispose();
    }

    [TestMethod]
    public async Task SearchAsyncSmokeTest()
    {
        // Arrange
        var client = _ServiceProvider!.GetRequiredService<GuardianApiClient>();

        // Act
        var result = await client.SearchAsync("climate change", pageSize: 5);

        // Assert
        Assert.IsNotNull(result, "Search result should not be null");
        Assert.AreEqual("ok", result.Status, "API response status should be 'ok'");
        Assert.IsTrue(result.Results.Count > 0, "Should return at least one result");
        Assert.IsTrue(result.Results.Count <= 5, "Should not return more than requested page size");

        // Verify content items have required properties
        var firstItem = result.Results.First();
        Assert.IsFalse(string.IsNullOrEmpty(firstItem.Id), "Content item should have an ID");
        Assert.IsFalse(string.IsNullOrEmpty(firstItem.WebTitle), "Content item should have a title");
        Assert.IsFalse(string.IsNullOrEmpty(firstItem.WebUrl), "Content item should have a web URL");
        Assert.IsFalse(string.IsNullOrEmpty(firstItem.ApiUrl), "Content item should have an API URL");

        Console.WriteLine($"Found {result.Results.Count} articles about climate change");
        Console.WriteLine($"First article: {firstItem.WebTitle}");
        Console.WriteLine($"Published: {firstItem.WebPublicationDate}");
    }

    [TestMethod]
    public async Task SearchAsyncWithNoResults()
    {
        // Arrange
        var client = _ServiceProvider!.GetRequiredService<GuardianApiClient>();

        // Act - search for something very unlikely to return results
        var result = await client.SearchAsync("xyzabc123nonexistentquery456");

        // Assert
        Assert.IsNotNull(result, "Search result should not be null even with no matches");
        Assert.AreEqual("ok", result.Status, "API response status should be 'ok'");
        Assert.AreEqual(0, result.Results.Count, "Should return zero results for non-existent query");
    }
}
