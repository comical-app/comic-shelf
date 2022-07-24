namespace Models;

public class Library
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Path { get; set; }
    
    public DateTime LastScan { get; set; }
    
    public IEnumerable<string> AcceptedExtensions { get; set; }
}