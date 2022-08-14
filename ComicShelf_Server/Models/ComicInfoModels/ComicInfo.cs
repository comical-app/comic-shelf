namespace Models.ComicInfoModels;

public class ComicInfo
{
    public string Title { get; set; }
    
    public string Series { get; set; }
    
    public string Number { get; set; }
    
    public int Count { get; set; }
    
    public int Volume { get; set; }
    
    public string AlternateSeries { get; set; }
    
    public string AlternateNumber { get; set; }
    
    public int AlternateCount { get; set; }
    
    public string Summary { get; set; }
    
    public string Notes { get; set; }
    
    public int Year { get; set; }
    
    public int Month { get; set; }
    
    public int Day { get; set; }
    
    public string Writer { get; set; }
    
    public string Penciller { get; set; }
    
    public string Inker { get; set; }
    
    public string Colorist { get; set; }
    
    public string Letterer { get; set; }
    
    public string CoverArtist { get; set; }
    
    public string Editor { get; set; }
    
    public string Publisher { get; set; }
    
    public string Imprint { get; set; }
    
    public string Genre { get; set; }
    
    public string Web { get; set; }
    
    public int PageCount { get; set; }
    
    public string LanguageISO { get; set; }
    
    public string Format { get; set; }
    
    public bool BlackAndWhite { get; set; }
    
    public Manga Manga {get;set;}
    
    public string Characters { get; set; }
    
    public string Teams { get; set; }
    
    public string Locations { get; set; }
    
    public string ScanInformation { get; set; }
    
    public string StoryArc { get; set; }
    
    public string SeriesGroup { get; set; }
    
    public AgeRating AgeRating {get;set;}
    
    public ArrayOfComicPageInfo Pages {get;set;}
    
    public decimal CommunityRating {get;set;}
}