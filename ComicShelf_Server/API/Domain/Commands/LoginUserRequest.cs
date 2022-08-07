using System.ComponentModel.DataAnnotations;

namespace API.Domain.Commands;

public class LoginUserRequest
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}