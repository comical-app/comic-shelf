using System.ComponentModel.DataAnnotations;

namespace Models.Commands;

public class ChangeUserPasswordCommand
{
    [Required(ErrorMessage = "User Id is required")]
    public Guid UserId { get; set; } 
    
    [Required(ErrorMessage = "Old password is required")]
    public string OldPassword { get; set; }
    
    [Required(ErrorMessage = "New password is required")]
    public string NewPassword { get; set; } 
    
    [Required(ErrorMessage = "Confirm password is required")]
    public string NewPasswordConfirmation { get; set; }
}