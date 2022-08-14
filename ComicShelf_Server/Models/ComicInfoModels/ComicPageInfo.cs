namespace Models.ComicInfoModels;

public class ComicPageInfo
{
    public int Image { get; set; }

    public ComicPageType Type { get; set; }

    public bool DoublePage { get; set; }

    public long ImageSize { get; set; }

    public string Key { get; set; }

    public string Bookmark { get; set; }

    public int ImageWidth { get; set; }

    public int ImageHeight { get; set; }
}