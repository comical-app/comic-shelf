using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain;

[Table("ComicFile")]
public class ComicFile
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Path { get; set; }
    
    public long Size { get; set; }
    
    public string Extension { get; set; }
    
    public string? MimeType { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public Guid LibraryId { get; set; }
    
    public Guid LibraryFolderId { get; set; }
    
    public bool Analysed { get; set; }
    
    public bool HasComicInfoFile { get; set; }
    
    public bool Scraped { get; set; }
}