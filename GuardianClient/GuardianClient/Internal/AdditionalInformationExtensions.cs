using GuardianClient.Options.Search;

namespace GuardianClient.Internal;

/// <summary>
/// Extension methods for converting additional information enums to their API string values.
/// </summary>
internal static class AdditionalInformationExtensions
{
    internal static string ToApiString(this ContentField option) => option switch
    {
        ContentField.TrailText => "trailText",
        ContentField.Headline => "headline",
        ContentField.ShowInRelatedContent => "showInRelatedContent",
        ContentField.Body => "body",
        ContentField.LastModified => "lastModified",
        ContentField.HasStoryPackage => "hasStoryPackage",
        ContentField.Score => "score",
        ContentField.Standfirst => "standfirst",
        ContentField.ShortUrl => "shortUrl",
        ContentField.Thumbnail => "thumbnail",
        ContentField.Wordcount => "wordcount",
        ContentField.Commentable => "commentable",
        ContentField.IsPremoderated => "isPremoderated",
        ContentField.AllowUgc => "allowUgc",
        ContentField.Byline => "byline",
        ContentField.Publication => "publication",
        ContentField.InternalPageCode => "internalPageCode",
        ContentField.ProductionOffice => "productionOffice",
        ContentField.ShouldHideAdverts => "shouldHideAdverts",
        ContentField.LiveBloggingNow => "liveBloggingNow",
        ContentField.CommentCloseDate => "commentCloseDate",
        ContentField.StarRating => "starRating",
        ContentField.All => "all",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };

    internal static string ToApiString(this ContentTag option) => option switch
    {
        ContentTag.Blog => "blog",
        ContentTag.Contributor => "contributor",
        ContentTag.Keyword => "keyword",
        ContentTag.NewspaperBook => "newspaper-book",
        ContentTag.NewspaperBookSection => "newspaper-book-section",
        ContentTag.Publication => "publication",
        ContentTag.Series => "series",
        ContentTag.Tone => "tone",
        ContentTag.Type => "type",
        ContentTag.All => "all",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };

    internal static string ToApiString(this ContentElement option) => option switch
    {
        ContentElement.Audio => "audio",
        ContentElement.Image => "image",
        ContentElement.Video => "video",
        ContentElement.All => "all",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };

    internal static string ToApiString(this Reference option) => option switch
    {
        Reference.Author => "author",
        Reference.BisacPrefix => "bisac-prefix",
        Reference.EsaCricketMatch => "esa-cricket-match",
        Reference.EsaFootballMatch => "esa-football-match",
        Reference.EsaFootballTeam => "esa-football-team",
        Reference.EsaFootballTournament => "esa-football-tournament",
        Reference.Isbn => "isbn",
        Reference.Imdb => "imdb",
        Reference.Musicbrainz => "musicbrainz",
        Reference.MusicbrainzGenre => "musicbrainzgenre",
        Reference.OptaCricketMatch => "opta-cricket-match",
        Reference.OptaFootballMatch => "opta-football-match",
        Reference.OptaFootballTeam => "opta-football-team",
        Reference.OptaFootballTournament => "opta-football-tournament",
        Reference.PaFootballCompetition => "pa-football-competition",
        Reference.PaFootballMatch => "pa-football-match",
        Reference.PaFootballTeam => "pa-football-team",
        Reference.R1Film => "r1-film",
        Reference.ReutersIndexRic => "reuters-index-ric",
        Reference.ReutersStockRic => "reuters-stock-ric",
        Reference.WitnessAssignment => "witness-assignment",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };

    internal static string ToApiString(this ContentRight option) => option switch
    {
        ContentRight.Syndicatable => "syndicatable",
        ContentRight.SubscriptionDatabases => "subscription-databases",
        ContentRight.All => "all",
        _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
    };
}
