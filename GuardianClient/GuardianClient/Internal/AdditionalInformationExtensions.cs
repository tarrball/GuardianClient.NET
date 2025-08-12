using GuardianClient.Options.Search;

namespace GuardianClient.Internal;

/// <summary>
/// Extension methods for converting additional information enums to their API string values.
/// </summary>
internal static class AdditionalInformationExtensions
{
    internal static string ToApiString(this GuardianApiContentShowFieldsOption option) => option switch
    {
        GuardianApiContentShowFieldsOption.TrailText => "trailText",
        GuardianApiContentShowFieldsOption.Headline => "headline",
        GuardianApiContentShowFieldsOption.ShowInRelatedContent => "showInRelatedContent",
        GuardianApiContentShowFieldsOption.Body => "body",
        GuardianApiContentShowFieldsOption.LastModified => "lastModified",
        GuardianApiContentShowFieldsOption.HasStoryPackage => "hasStoryPackage",
        GuardianApiContentShowFieldsOption.Score => "score",
        GuardianApiContentShowFieldsOption.Standfirst => "standfirst",
        GuardianApiContentShowFieldsOption.ShortUrl => "shortUrl",
        GuardianApiContentShowFieldsOption.Thumbnail => "thumbnail",
        GuardianApiContentShowFieldsOption.Wordcount => "wordcount",
        GuardianApiContentShowFieldsOption.Commentable => "commentable",
        GuardianApiContentShowFieldsOption.IsPremoderated => "isPremoderated",
        GuardianApiContentShowFieldsOption.AllowUgc => "allowUgc",
        GuardianApiContentShowFieldsOption.Byline => "byline",
        GuardianApiContentShowFieldsOption.Publication => "publication",
        GuardianApiContentShowFieldsOption.InternalPageCode => "internalPageCode",
        GuardianApiContentShowFieldsOption.ProductionOffice => "productionOffice",
        GuardianApiContentShowFieldsOption.ShouldHideAdverts => "shouldHideAdverts",
        GuardianApiContentShowFieldsOption.LiveBloggingNow => "liveBloggingNow",
        GuardianApiContentShowFieldsOption.CommentCloseDate => "commentCloseDate",
        GuardianApiContentShowFieldsOption.StarRating => "starRating",
        GuardianApiContentShowFieldsOption.All => "all",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };

    internal static string ToApiString(this GuardianApiContentShowTagsOption option) => option switch
    {
        GuardianApiContentShowTagsOption.Blog => "blog",
        GuardianApiContentShowTagsOption.Contributor => "contributor",
        GuardianApiContentShowTagsOption.Keyword => "keyword",
        GuardianApiContentShowTagsOption.NewspaperBook => "newspaper-book",
        GuardianApiContentShowTagsOption.NewspaperBookSection => "newspaper-book-section",
        GuardianApiContentShowTagsOption.Publication => "publication",
        GuardianApiContentShowTagsOption.Series => "series",
        GuardianApiContentShowTagsOption.Tone => "tone",
        GuardianApiContentShowTagsOption.Type => "type",
        GuardianApiContentShowTagsOption.All => "all",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };

    internal static string ToApiString(this GuardianApiContentShowElementsOption option) => option switch
    {
        GuardianApiContentShowElementsOption.Audio => "audio",
        GuardianApiContentShowElementsOption.Image => "image",
        GuardianApiContentShowElementsOption.Video => "video",
        GuardianApiContentShowElementsOption.All => "all",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };

    internal static string ToApiString(this GuardianApiContentShowReferencesOption option) => option switch
    {
        GuardianApiContentShowReferencesOption.Author => "author",
        GuardianApiContentShowReferencesOption.BisacPrefix => "bisac-prefix",
        GuardianApiContentShowReferencesOption.EsaCricketMatch => "esa-cricket-match",
        GuardianApiContentShowReferencesOption.EsaFootballMatch => "esa-football-match",
        GuardianApiContentShowReferencesOption.EsaFootballTeam => "esa-football-team",
        GuardianApiContentShowReferencesOption.EsaFootballTournament => "esa-football-tournament",
        GuardianApiContentShowReferencesOption.Isbn => "isbn",
        GuardianApiContentShowReferencesOption.Imdb => "imdb",
        GuardianApiContentShowReferencesOption.Musicbrainz => "musicbrainz",
        GuardianApiContentShowReferencesOption.MusicbrainzGenre => "musicbrainzgenre",
        GuardianApiContentShowReferencesOption.OptaCricketMatch => "opta-cricket-match",
        GuardianApiContentShowReferencesOption.OptaFootballMatch => "opta-football-match",
        GuardianApiContentShowReferencesOption.OptaFootballTeam => "opta-football-team",
        GuardianApiContentShowReferencesOption.OptaFootballTournament => "opta-football-tournament",
        GuardianApiContentShowReferencesOption.PaFootballCompetition => "pa-football-competition",
        GuardianApiContentShowReferencesOption.PaFootballMatch => "pa-football-match",
        GuardianApiContentShowReferencesOption.PaFootballTeam => "pa-football-team",
        GuardianApiContentShowReferencesOption.R1Film => "r1-film",
        GuardianApiContentShowReferencesOption.ReutersIndexRic => "reuters-index-ric",
        GuardianApiContentShowReferencesOption.ReutersStockRic => "reuters-stock-ric",
        GuardianApiContentShowReferencesOption.WitnessAssignment => "witness-assignment",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };

    internal static string ToApiString(this GuardianApiContentShowRightsOption option) => option switch
    {
        GuardianApiContentShowRightsOption.Syndicatable => "syndicatable",
        GuardianApiContentShowRightsOption.SubscriptionDatabases => "subscription-databases",
        GuardianApiContentShowRightsOption.All => "all",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };
}
