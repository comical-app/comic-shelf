using System.ComponentModel.DataAnnotations;

namespace Models.Commands;

public class UpdateLibraryCommand
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(30)]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Folders path are required")]
    public IEnumerable<string> FoldersPath { get; set; }
    
    public IEnumerable<string> AcceptedExtensions { get; set; }
}