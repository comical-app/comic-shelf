using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("Library")]
public class Library
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Path { get; set; }
    
    public DateTime LastScan { get; set; }
    
    public string AcceptedExtensions { get; set; }
    
    public IEnumerable<File> Files { get; set; }
}