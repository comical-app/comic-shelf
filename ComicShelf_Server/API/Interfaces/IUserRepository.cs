using API.Domain.Commands;
using Models;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> ListUsersAsync();

    Task<IEnumerable<User>> ListUsersWithOpdsAccessAsync();

    Task<User?> GetUserByIdAsync(Guid id);
    
    Task<bool> CheckIfUsernameExistsAsync(string username);
    
    Task<User> CreateUserAsync(CreateUserRequest user);
    
    Task<bool> UpdateUserAsync(User user);
    
    Task<bool> DeleteUserAsync(Guid id);
    
    Task<User?> LoginAsync(string username, string password);
    
    Task<bool> ResetPasswordAsync(Guid userId, string newPassword, string newPasswordConfirmation);
    
    Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword, string newPasswordConfirmation);
    
    Task<bool> ActivateUserAsync(Guid userId);
    
    Task<bool> DeactivateUserAsync(Guid userId);
    
    Task<bool> GiveOpdsAccessAsync(Guid userId);
    
    Task<bool> TakeOpdsAccessAsync(Guid userId);
}