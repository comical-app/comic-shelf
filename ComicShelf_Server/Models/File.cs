using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("File")]
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
    
    public Guid LibraryId { get; set; }
}