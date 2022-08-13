using System.ComponentModel.DataAnnotations;

namespace Models.Commands;

public class CreateLibraryCommand
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(30)]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Path is required")]
    public string Path { get; set; }
    
    public IEnumerable<string> AcceptedExtensions { get; set; }
}