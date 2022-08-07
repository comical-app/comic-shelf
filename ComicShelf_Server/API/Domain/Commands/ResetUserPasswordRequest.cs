namespace API.Domain.Commands;

public class ResetUserPasswordRequest
{
   public Guid UserId { get; set; } 
   public string NewPassword { get; set; } 
   public string NewPasswordConfirmation { get; set; }
}