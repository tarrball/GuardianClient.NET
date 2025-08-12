# GuardianClient.NET

---

<p align="center">
  <img src="./assets/guardian.png" width="150" alt="Guardian knight logo"/>
</p>

A modern, comprehensive .NET client library for The Guardian's Content API. Provides strongly-typed models, flexible search options, and simple methods for searching Guardian articles with full API feature support.

[![NuGet Version](https://img.shields.io/nuget/v/GuardianClient.NET?logo=nuget)](https://www.nuget.org/packages/GuardianClient.NET)
[![Build Status](https://img.shields.io/github/actions/workflow/status/tarrball/GuardianClient.NET/deploy-nuget.yml?branch=main)](https://github.com/tarrball/GuardianClient.NET/actions)

## ✨ Features

- 🔍 **Complete Content Search** - Full Guardian Content API search with all parameters
- 🏷️ **Strongly Typed** - Type-safe enums for fields, tags, elements, and references  
- 🎯 **Comprehensive Filtering** - Search by section, tags, date ranges, production office, and more
- 📑 **Rich Content Options** - Include fields, tags, media elements, blocks, and references
- 🔧 **Interface-Based Design** - Easy mocking and dependency injection with `IGuardianApiClient`
- 📦 **HttpClientFactory Ready** - Proper HttpClient lifecycle management
- ⚡ **Modern Async** - Full async/await patterns with cancellation support
- 🎨 **Clean API** - Organized option classes for maintainable code

## 🚀 Quick Start

### Installation

```bash
dotnet add package GuardianClient.NET
```

### Setup with Dependency Injection (Recommended)

```csharp
// Program.cs or Startup.cs
builder.Services.AddGuardianApiClient("your-api-key");
```

### Basic Usage

```csharp
public class NewsService
{
    private readonly IGuardianApiClient _client;

    public NewsService(IGuardianApiClient client)
    {
        _client = client;
    }

    public async Task<ContentSearchResponse?> GetLatestTechNews()
    {
        return await _client.SearchAsync(new GuardianApiContentSearchOptions
        {
            Query = "artificial intelligence",
            FilterOptions = new GuardianApiContentFilterOptions
            {
                Section = "technology"
            },
            PageOptions = new GuardianApiContentPageOptions 
            { 
                PageSize = 10 
            }
        });
    }
}
```

## 🔧 Advanced Usage

### Comprehensive Search with All Options

```csharp
var results = await client.SearchAsync(new GuardianApiContentSearchOptions
{
    // Search terms with boolean operators
    Query = "climate change AND (policy OR legislation)",
    QueryFields = ["body", "headline"],
    
    // Filtering options
    FilterOptions = new GuardianApiContentFilterOptions
    {
        Section = "environment",
        Tag = "climate-change",
        Language = "en"
    },
    
    // Date filtering  
    DateOptions = new GuardianApiContentDateOptions
    {
        FromDate = new DateOnly(2023, 1, 1),
        UseDate = "published"
    },
    
    // Pagination
    PageOptions = new GuardianApiContentPageOptions
    {
        Page = 1,
        PageSize = 20
    },
    
    // Ordering
    OrderOptions = new GuardianApiContentOrderOptions
    {
        OrderBy = GuardianApiContentOrderBy.Relevance,
        OrderDate = GuardianApiContentOrderDate.Published
    },
    
    // Additional content
    AdditionalInformationOptions = new GuardianApiContentAdditionalInformationOptions
    {
        ShowFields = [
            GuardianApiContentShowFieldsOption.Headline,
            GuardianApiContentShowFieldsOption.Body,
            GuardianApiContentShowFieldsOption.Thumbnail
        ],
        ShowTags = [
            GuardianApiContentShowTagsOption.Keyword,
            GuardianApiContentShowTagsOption.Tone
        ],
        ShowElements = [GuardianApiContentShowElementsOption.Image]
    }
});
```

### Getting Individual Articles

```csharp
// Get a specific article by ID
var article = await client.GetItemAsync("world/2023/oct/15/climate-summit-agreement",
    new GuardianApiContentAdditionalInformationOptions
    {
        ShowFields = [
            GuardianApiContentShowFieldsOption.Body,
            GuardianApiContentShowFieldsOption.Byline
        ],
        ShowTags = [GuardianApiContentShowTagsOption.All]
    });

// Access the rich content
Console.WriteLine(article.Content.WebTitle);
Console.WriteLine(article.Content.Fields.Body); // Full HTML content
Console.WriteLine($"Author: {article.Content.Fields.Byline}");
```

### Manual Setup (without DI)

```csharp
using var client = new GuardianApiClient("your-api-key");
var results = await client.SearchAsync(new GuardianApiContentSearchOptions 
{
    Query = "sports",
    PageOptions = new GuardianApiContentPageOptions { PageSize = 5 }
});

foreach (var article in results.Results)
{
    Console.WriteLine($"{article.WebTitle} - {article.WebPublicationDate}");
}
```

## 🔑 Getting an API Key

1. Visit [The Guardian Open Platform](https://open-platform.theguardian.com/access/)
2. Sign up for a free developer account  
3. Generate your API key
4. Store it securely (use User Secrets for development)

## 🏗️ Available Options

### Filter Options
- **Section**: Filter by Guardian sections (e.g., "politics", "sport", "culture")
- **Tags**: Filter by content tags with boolean operators
- **Date Range**: Filter by publication date with flexible date types
- **Language**: Filter by content language (ISO codes)
- **Production Office**: Filter by Guardian production offices
- **Star Rating**: Filter by review ratings (1-5)

### Additional Content Options
- **ShowFields**: Include extra fields like body content, thumbnails, bylines
- **ShowTags**: Include metadata tags (keywords, contributors, tone)
- **ShowElements**: Include media elements (images, videos, audio)
- **ShowReferences**: Include reference data (ISBNs, IMDB IDs, etc.)
- **ShowBlocks**: Include content blocks (useful for live blogs)

### Ordering Options
- **OrderBy**: Sort by newest, oldest, or relevance
- **OrderDate**: Choose which date to use for sorting

## 📊 Current Status

**✅ Fully Implemented:**
- Complete Content API search with all parameters
- Individual article retrieval
- Strongly-typed models and enums for all options
- Advanced filtering, pagination, and sorting
- Rich content enhancement options
- Interface-based design for easy testing
- Comprehensive documentation and examples

**🎯 Feature Complete:** This library now supports the full Guardian Content API specification.

## 🧪 Testing

The library includes comprehensive test coverage:

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

Tests require a Guardian API key stored in user secrets:
```bash
dotnet user-secrets set "GuardianApiKey" "your-api-key-here"
```

## 🤝 Contributing

Contributions are welcome! Please:
1. Fork the repository
2. Create a feature branch
3. Add tests for new functionality
4. Ensure all tests pass
5. Submit a pull request

## 📄 License

MIT License - see [LICENSE](LICENSE) for details.

---

*This is an unofficial library and is not affiliated with The Guardian.*