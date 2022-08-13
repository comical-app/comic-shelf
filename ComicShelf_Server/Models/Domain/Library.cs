using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain;

[Table("Library")]
public class Library
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    public string Path { get; set; }
    
    public DateTime LastScan { get; set; }
    
    public string AcceptedExtensions { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public bool IsActive { get; set; }
    
    public IEnumerable<ComicFile> Files { get; set; }
}