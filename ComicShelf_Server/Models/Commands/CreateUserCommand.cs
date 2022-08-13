using System.ComponentModel.DataAnnotations;

namespace Models.Commands;

public class CreateUserCommand
{
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(30)]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    [Required(ErrorMessage = "IsAdmin is required")]
    public bool IsAdmin { get; set; }

    [Required(ErrorMessage = "CanAccessOpds is required")]
    public bool CanAccessOpds { get; set; }
}