using System.ComponentModel.DataAnnotations;

namespace API.Domain.Commands;

public class UpdateUserRequest
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string Username { get; set; }
    
    [Required]
    public bool IsAdmin { get; set; }
    
    [Required]
    public bool CanAccessOpds { get; set; }
}