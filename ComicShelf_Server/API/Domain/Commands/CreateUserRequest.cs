using System.ComponentModel.DataAnnotations;

namespace API.Domain.Commands;

public class CreateUserRequest
{
    [Required]
    [MaxLength(30)]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public bool IsAdmin { get; set; }
    
    [Required]
    public bool CanAccessOpds { get; set; }
}