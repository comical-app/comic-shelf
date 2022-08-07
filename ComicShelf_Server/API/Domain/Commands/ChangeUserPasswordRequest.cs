namespace API.Domain.Commands;

public class ChangeUserPasswordRequest
{
    public Guid UserId { get; set; } 
    public string OldPassword { get; set; }
    public string NewPassword { get; set; } 
    public string NewPasswordConfirmation { get; set; }
}