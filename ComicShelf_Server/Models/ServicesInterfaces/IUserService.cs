using Models.Commands;
using Models.Domain;

namespace Models.ServicesInterfaces;

public interface IUserService
{
    Task<IEnumerable<User>> ListUsersAsync();

    Task<IEnumerable<User>> ListUsersWithOpdsAccessAsync();

    Task<User?> GetUserByIdAsync(Guid userId);

    Task<User?> GetUserByUsernameAsync(string username);
    
    Task<bool> CheckIfUsernameIsUniqueAsync(string username);
    
    Task<User> CreateUserAsync(CreateUserCommand command);
    
    Task<bool> UpdateUserAsync(Guid userId, UpdateUserCommand command);
    
    Task<bool> DeleteUserAsync(Guid userId);
    
    Task<User?> LoginAsync(LoginUserCommand command);
    
    Task<bool> ResetPasswordAsync(ResetUserPasswordCommand command);
    
    Task<bool> ChangePasswordAsync(ChangeUserPasswordCommand command);
    
    Task<bool> ActivateUserAsync(Guid userId);
    
    Task<bool> DeactivateUserAsync(Guid userId);
    
    Task<bool> GiveOpdsAccessAsync(Guid userId);
    
    Task<bool> TakeOpdsAccessAsync(Guid userId);
}