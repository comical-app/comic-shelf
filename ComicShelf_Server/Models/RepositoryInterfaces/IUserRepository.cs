using Models.Domain;

namespace Models.RepositoryInterfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> ListUsersAsync();

    Task<IEnumerable<User>> ListUsersWithOpdsAccessAsync();

    Task<User?> GetUserByIdAsync(Guid userId);

    Task<User?> GetUserByUsernameAsync(string username);
    
    Task<bool> CheckIfUsernameIsUniqueAsync(string username);
    
    Task<User> CreateUserAsync(User user);
    
    Task<User> UpdateUserAsync(User user);
    
    Task<bool> DeleteUserAsync(User user);
}