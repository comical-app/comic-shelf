namespace Models;

public class File
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Path { get; set; }
    
    public long Size { get; set; }
    
    public string Extension { get; set; }
    
    public DateTime LastModifiedDate { get; set; }
}