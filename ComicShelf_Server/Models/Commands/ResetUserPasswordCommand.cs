using System.ComponentModel.DataAnnotations;

namespace Models.Commands;

public class ResetUserPasswordCommand
{
    [Required(ErrorMessage = "User Id is required")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    public string NewPasswordConfirmation { get; set; }
}