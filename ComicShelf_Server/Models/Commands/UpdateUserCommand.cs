using System.ComponentModel.DataAnnotations;

namespace Models.Commands;

public class UpdateUserCommand
{
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(30)]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "IsAdmin is required")]
    public bool IsAdmin { get; set; }
    
    [Required(ErrorMessage = "CanAccessOdps is required")]
    public bool CanAccessOpds { get; set; }
}