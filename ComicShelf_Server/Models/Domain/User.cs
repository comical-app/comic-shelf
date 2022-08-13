using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain;

[Table("User")]
public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public DateTime? LastLogin { get; set; }
    
    public bool IsActive { get; set; }
    
    public bool IsAdmin { get; set; }
    
    public bool CanAccessOpds { get; set; }
}