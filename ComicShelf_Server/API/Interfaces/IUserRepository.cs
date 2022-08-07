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
    
    Task<bool> UpdateUserAsync(Guid userId, UpdateUserRequest user);
    
    Task<bool> DeleteUserAsync(Guid id);
    
    Task<User?> LoginAsync(LoginUserRequest user);
    
    Task<bool> ResetPasswordAsync(ResetUserPasswordRequest user);
    
    Task<bool> ChangePasswordAsync(ChangeUserPasswordRequest user);
    
    Task<bool> ActivateUserAsync(Guid userId);
    
    Task<bool> DeactivateUserAsync(Guid userId);
    
    Task<bool> GiveOpdsAccessAsync(Guid userId);
    
    Task<bool> TakeOpdsAccessAsync(Guid userId);
}