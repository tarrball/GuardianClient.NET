namespace GuardianClient.Options.Search;

/// <summary>
/// Fields that can be included with content responses.
/// </summary>
public enum ShowFieldsOption
{
    TrailText,
    Headline,
    ShowInRelatedContent,
    Body,
    LastModified,
    HasStoryPackage,
    Score,
    Standfirst,
    ShortUrl,
    Thumbnail,
    Wordcount,
    Commentable,
    IsPremoderated,
    AllowUgc,
    Byline,
    Publication,
    InternalPageCode,
    ProductionOffice,
    ShouldHideAdverts,
    LiveBloggingNow,
    CommentCloseDate,
    StarRating,
    All
}

/// <summary>
/// Tag types that can be included with content responses.
/// </summary>
public enum ShowTagsOption
{
    Blog,
    Contributor,
    Keyword,
    NewspaperBook,
    NewspaperBookSection,
    Publication,
    Series,
    Tone,
    Type,
    All
}

/// <summary>
/// Media element types that can be included with content responses.
/// </summary>
public enum ShowElementsOption
{
    Audio,
    Image,
    Video,
    All
}

/// <summary>
/// Reference types that can be included with content responses.
/// </summary>
public enum ShowReferencesOption
{
    Author,
    BisacPrefix,
    EsaCricketMatch,
    EsaFootballMatch,
    EsaFootballTeam,
    EsaFootballTournament,
    Isbn,
    Imdb,
    Musicbrainz,
    MusicbrainzGenre,
    OptaCricketMatch,
    OptaFootballMatch,
    OptaFootballTeam,
    OptaFootballTournament,
    PaFootballCompetition,
    PaFootballMatch,
    PaFootballTeam,
    R1Film,
    ReutersIndexRic,
    ReutersStockRic,
    WitnessAssignment
}

/// <summary>
/// Rights types that can be included with content responses.
/// </summary>
public enum ShowRightsOption
{
    Syndicatable,
    SubscriptionDatabases,
    All
}

/// <summary>
/// Extension methods for converting additional information enums to their API string values.
/// </summary>
internal static class AdditionalInformationEnumExtensions
{
    internal static string ToApiString(this ShowFieldsOption option) => option switch
    {
        ShowFieldsOption.TrailText => "trailText",
        ShowFieldsOption.Headline => "headline",
        ShowFieldsOption.ShowInRelatedContent => "showInRelatedContent",
        ShowFieldsOption.Body => "body",
        ShowFieldsOption.LastModified => "lastModified",
        ShowFieldsOption.HasStoryPackage => "hasStoryPackage",
        ShowFieldsOption.Score => "score",
        ShowFieldsOption.Standfirst => "standfirst",
        ShowFieldsOption.ShortUrl => "shortUrl",
        ShowFieldsOption.Thumbnail => "thumbnail",
        ShowFieldsOption.Wordcount => "wordcount",
        ShowFieldsOption.Commentable => "commentable",
        ShowFieldsOption.IsPremoderated => "isPremoderated",
        ShowFieldsOption.AllowUgc => "allowUgc",
        ShowFieldsOption.Byline => "byline",
        ShowFieldsOption.Publication => "publication",
        ShowFieldsOption.InternalPageCode => "internalPageCode",
        ShowFieldsOption.ProductionOffice => "productionOffice",
        ShowFieldsOption.ShouldHideAdverts => "shouldHideAdverts",
        ShowFieldsOption.LiveBloggingNow => "liveBloggingNow",
        ShowFieldsOption.CommentCloseDate => "commentCloseDate",
        ShowFieldsOption.StarRating => "starRating",
        ShowFieldsOption.All => "all",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };

    internal static string ToApiString(this ShowTagsOption option) => option switch
    {
        ShowTagsOption.Blog => "blog",
        ShowTagsOption.Contributor => "contributor",
        ShowTagsOption.Keyword => "keyword",
        ShowTagsOption.NewspaperBook => "newspaper-book",
        ShowTagsOption.NewspaperBookSection => "newspaper-book-section",
        ShowTagsOption.Publication => "publication",
        ShowTagsOption.Series => "series",
        ShowTagsOption.Tone => "tone",
        ShowTagsOption.Type => "type",
        ShowTagsOption.All => "all",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };

    internal static string ToApiString(this ShowElementsOption option) => option switch
    {
        ShowElementsOption.Audio => "audio",
        ShowElementsOption.Image => "image",
        ShowElementsOption.Video => "video",
        ShowElementsOption.All => "all",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };

    internal static string ToApiString(this ShowReferencesOption option) => option switch
    {
        ShowReferencesOption.Author => "author",
        ShowReferencesOption.BisacPrefix => "bisac-prefix",
        ShowReferencesOption.EsaCricketMatch => "esa-cricket-match",
        ShowReferencesOption.EsaFootballMatch => "esa-football-match",
        ShowReferencesOption.EsaFootballTeam => "esa-football-team",
        ShowReferencesOption.EsaFootballTournament => "esa-football-tournament",
        ShowReferencesOption.Isbn => "isbn",
        ShowReferencesOption.Imdb => "imdb",
        ShowReferencesOption.Musicbrainz => "musicbrainz",
        ShowReferencesOption.MusicbrainzGenre => "musicbrainzgenre",
        ShowReferencesOption.OptaCricketMatch => "opta-cricket-match",
        ShowReferencesOption.OptaFootballMatch => "opta-football-match",
        ShowReferencesOption.OptaFootballTeam => "opta-football-team",
        ShowReferencesOption.OptaFootballTournament => "opta-football-tournament",
        ShowReferencesOption.PaFootballCompetition => "pa-football-competition",
        ShowReferencesOption.PaFootballMatch => "pa-football-match",
        ShowReferencesOption.PaFootballTeam => "pa-football-team",
        ShowReferencesOption.R1Film => "r1-film",
        ShowReferencesOption.ReutersIndexRic => "reuters-index-ric",
        ShowReferencesOption.ReutersStockRic => "reuters-stock-ric",
        ShowReferencesOption.WitnessAssignment => "witness-assignment",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };

    internal static string ToApiString(this ShowRightsOption option) => option switch
    {
        ShowRightsOption.Syndicatable => "syndicatable",
        ShowRightsOption.SubscriptionDatabases => "subscription-databases",
        ShowRightsOption.All => "all",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };
}