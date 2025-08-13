using GuardianClient.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GuardianClient.Tests;

public abstract class TestBase
{
    protected static IGuardianApiClient Client { get; }

    static TestBase()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<TestBase>()
            .Build();

        var apiKey = config["GuardianApiKey"]!;

        Client = new ServiceCollection()
            .AddGuardianApiClient(apiKey)
            .BuildServiceProvider()
            .GetRequiredService<GuardianApiClient>();
    }
}
