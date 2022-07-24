using System.ComponentModel.DataAnnotations;

namespace Models;

public class File
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Path { get; set; }
    
    public long Size { get; set; }
    
    public string Extension { get; set; }
    
    public string? MimeType { get; set; }
    
    public DateTime AddedAt { get; set; }
    
    public DateTime LastModifiedDate { get; set; }
}