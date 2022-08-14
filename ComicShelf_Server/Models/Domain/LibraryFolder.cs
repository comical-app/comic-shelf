using System.ComponentModel.DataAnnotations;

namespace Models.Domain;

public class LibraryFolder
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid LibraryId { get; set; }

    public string Path { get; set; }
    
    public bool IsActive { get; set; }
    
    public DateTime LastScanAt { get; set; }
}