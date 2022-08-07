using System.ComponentModel.DataAnnotations;

namespace API.Domain.Commands;

public class CreateLibraryRequest
{
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }
    
    [Required]
    public string Path { get; set; }
    
    public IEnumerable<string> AcceptedExtensions { get; set; }
}