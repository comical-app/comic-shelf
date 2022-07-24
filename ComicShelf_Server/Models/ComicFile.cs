namespace Models;

public class ComicFile
{
    public Guid Id { get; set; }
    
    public string Series { get; set; }
    
    public string Issue { get; set; }
    
    public Guid FileId { get; set; }
}