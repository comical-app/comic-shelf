using System.ComponentModel;

namespace Models.ComicInfoModels;

public enum AgeRating
{
    [Description("Unknown")]
    Unknown,
    [Description("Adults Only 18+")]
    AdultsOnly18Plus,
    [Description("Early Childhood")]
    EarlyChildhood,
    [Description("Everyone")]
    Everyone,
    [Description("Everyone 10+")]
    Everyone10Plus,
    [Description("G")]
    G,
    [Description("Kids to Adults")]
    KidsToAdults,
    [Description("M")]
    M,
    [Description("MA15+")]
    Ma15Plus,
    [Description("Mature 17+")]
    Mature17Plus,
    [Description("PG")]
    Pg,
    [Description("R18+")]
    R18Plus,
    [Description("Rating Pending")]
    RatingPending,
    [Description("Teen")]
    Teen,
    [Description("X18+")]
    X18Plus
}