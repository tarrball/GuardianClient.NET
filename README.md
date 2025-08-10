# GuardianClient.NET

---

<p align="center">
  <img src="./assets/guardian.png" width="150" alt="Guardian knight logo"/>
</p>

A modern .NET client library for The Guardian's Content API. Provides strongly-typed models and simple methods for
searching Guardian articles, tags, sections, and more.

[![NuGet Version](https://img.shields.io/nuget/v/GuardianClient.NET?logo=nuget)](https://www.nuget.org/packages/GuardianClient.NET)
[![Build Status](https://img.shields.io/github/actions/workflow/status/tarrball/GuardianClient.NET/deploy-nuget.yml?branch=main)](https://github.com/tarrball/GuardianClient.NET/actions)

## ✨ Features

- 🔍 **Content Search** - Search Guardian articles with full query support
- 🏷️ **Strongly Typed** - Complete C# models for all API responses
- 🔧 **Dependency Injection** - Easy setup with `services.AddGuardianApiClient()`
- 📦 **HttpClientFactory** - Proper HttpClient lifecycle management
- ⚡ **Async/Await** - Modern async patterns with cancellation support

## 🚀 Quick Start

### Installation

```bash
dotnet add package GuardianClient.NET
```

### Setup with Dependency Injection

```csharp
// Program.cs or Startup.cs
builder.Services.AddGuardianApiClient("your-api-key");
```

### Usage

```csharp
public class NewsService
{
    private readonly GuardianApiClient _client;

    public NewsService(GuardianApiClient client)
    {
        _client = client;
    }

    public async Task<ContentSearchResponse?> GetLatestNews()
    {
        return await _client.SearchAsync("politics", pageSize: 10);
    }
}
```

### Manual Setup (without DI)

```csharp
using var client = new GuardianApiClient("your-api-key");
var results = await client.SearchAsync("climate change", pageSize: 5);

foreach (var article in results.Results)
{
    Console.WriteLine($"{article.WebTitle} - {article.WebPublicationDate}");
}
```

## 🔑 Getting an API Key

1. Visit [The Guardian Open Platform](https://open-platform.theguardian.com/access/)
2. Sign up for a free developer account
3. Generate your API key

## 📋 Current Status

**✅ Implemented:**

- Content search with basic parameters
- Strongly-typed response models
- Dependency injection support
- HttpClientFactory integration

**🔄 In Development:**

- Additional endpoints (tags, sections, editions)
- Advanced search parameters
- Deep pagination support

## 🤝 Contributing

This is an early-stage project. Issues and pull requests are welcome!

## 📄 License

MIT License - see [LICENSE](LICENSE) for details.

---

*This is an unofficial library and is not affiliated with The Guardian.*
