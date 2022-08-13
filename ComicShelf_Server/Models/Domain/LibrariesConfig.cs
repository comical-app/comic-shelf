namespace Models.Domain;

public class LibrariesConfig
{
    public string ComicVineApiKey { get; set; }
    
    public string ConfigLocation { get; set; }
    
    public IEnumerable<Library> Libraries { get; set; }
}