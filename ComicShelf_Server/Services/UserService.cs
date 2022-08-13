using Microsoft.Extensions.Logging;
using Models.Commands;
using Models.Domain;
using Models.RepositoryInterfaces;
using Models.ServicesInterfaces;

namespace Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _userRepository;

    public UserService(ILogger<UserService> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> ListUsersAsync()
    {
        try
        {
            return await _userRepository.ListUsersAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to list users. {EMessage}", e.Message);
            throw;
        }
    }

    public async Task<IEnumerable<User>> ListUsersWithOpdsAccessAsync()
    {
        try
        {
            return await _userRepository.ListUsersWithOpdsAccessAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to list users with opds access. {EMessage}", e.Message);
            throw;
        }
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        try
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get user by id ({UserId}). {EMessage}", userId, e.Message);
            throw;
        }
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        try
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get user by username ({Username}). {EMessage}", username, e.Message);
            throw;
        }
    }

    public async Task<bool> CheckIfUsernameIsUniqueAsync(string username)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be empty");

            return await _userRepository.CheckIfUsernameIsUniqueAsync(username);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to check if username is unique ({Username}). {EMessage}", username, e.Message);
            throw;
        }
    }

    public async Task<User> CreateUserAsync(CreateUserCommand command)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.Username)) throw new ArgumentException("Username cannot be empty");
            if (string.IsNullOrWhiteSpace(command.Password)) throw new ArgumentException("Password cannot be empty");
            if (!await CheckIfUsernameIsUniqueAsync(command.Username)) throw new ArgumentException("Username already exists");

            var newUser = new User
            {
                Username = command.Username.Trim(),
                Password = BCrypt.Net.BCrypt.HashPassword(command.Password),
                IsAdmin = command.IsAdmin,
                CanAccessOpds = command.CanAccessOpds
            };
            
            return await _userRepository.CreateUserAsync(newUser);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create user {Username}. {EMessage}", command.Username, e.Message);
            throw;
        }
    }

    public async Task<bool> UpdateUserAsync(Guid userId, UpdateUserCommand command)
    {
        try
        {
            if (command == null) throw new ArgumentException("User cannot be null");
        
            var userToEdit = await GetUserByIdAsync(userId);
            if (userToEdit == null) return false;

            if (command.Username != userToEdit.Username)
            {
                if (!await CheckIfUsernameIsUniqueAsync(command.Username)) throw new ArgumentException("Username already exists");
            }

            userToEdit.Username = command.Username.Trim();
            userToEdit.IsAdmin = command.IsAdmin;
            userToEdit.CanAccessOpds = command.CanAccessOpds;

            await _userRepository.UpdateUserAsync(userToEdit);
            return await Task.FromResult(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to update user {UserId}. {EMessage}", userId, e.Message);
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        try
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;
            
            return await _userRepository.DeleteUserAsync(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete user {UserId}. {EMessage}", userId, e.Message);
            throw;
        }
    }

    public async Task<User?> LoginAsync(LoginUserCommand command)
    {
        try
        {
            var loggedUser = await GetUserByUsernameAsync(command.Username);
            if (loggedUser == null) return null;
            if (loggedUser.IsActive == false) return null;
        
            if (!BCrypt.Net.BCrypt.Verify(command.Password, loggedUser.Password)) return null;

            loggedUser.LastLogin = DateTime.Now;
            await _userRepository.UpdateUserAsync(loggedUser);

            return loggedUser;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
            throw;
        }
    }

    public async Task<bool> ResetPasswordAsync(ResetUserPasswordCommand command)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.NewPassword)) throw new Exception("New password cannot be empty");

            if (string.IsNullOrWhiteSpace(command.NewPasswordConfirmation)) throw new Exception("New password confirmation cannot be empty");

            if (command.NewPassword != command.NewPasswordConfirmation)throw new Exception("New password and new password confirmation do not match");

            var selectedUser = await GetUserByIdAsync(command.UserId);
            if (selectedUser == null) return false;

            selectedUser.Password = command.NewPassword;
            
            await _userRepository.UpdateUserAsync(selectedUser);
            
            return await Task.FromResult(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to reset password for user {UserId}. {EMessage}", command.UserId, e.Message);
            throw;
        }
    }

    public async Task<bool> ChangePasswordAsync(ChangeUserPasswordCommand command)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.OldPassword)) throw new Exception("Old password cannot be empty");

            if (string.IsNullOrWhiteSpace(command.NewPassword)) throw new Exception("New password cannot be empty");

            if (string.IsNullOrWhiteSpace(command.NewPasswordConfirmation)) throw new Exception("New password confirmation cannot be empty");

            if (command.NewPassword != command.NewPasswordConfirmation) throw new Exception("New password and new password confirmation do not match");

            var selectedUser = await GetUserByIdAsync(command.UserId);
            if (selectedUser == null) return false;
        
            if (!BCrypt.Net.BCrypt.Verify(command.OldPassword, selectedUser.Password)) return false;

            selectedUser.Password = command.NewPassword;
            
             await _userRepository.UpdateUserAsync(selectedUser);
            
             return await Task.FromResult(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to change password for user {UserId}. {EMessage}", command.UserId, e.Message);
            throw;
        }
    }

    public async Task<bool> ActivateUserAsync(Guid userId)
    {
        try
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = true;
            
             await _userRepository.UpdateUserAsync(user);
            
             return await Task.FromResult(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to activate user {UserId}. {EMessage}", userId, e.Message);
            throw;
        }
    }

    public async Task<bool> DeactivateUserAsync(Guid userId)
    {
        try
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = false;
            
             await _userRepository.UpdateUserAsync(user);
            
             return await Task.FromResult(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to deactivate user {UserId}. {EMessage}", userId, e.Message);
            throw;
        }
    }

    public async Task<bool> GiveOpdsAccessAsync(Guid userId)
    {
        try
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            user.CanAccessOpds = true;
            
             await _userRepository.UpdateUserAsync(user);
            
             return await Task.FromResult(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to give opds access to user {UserId}. {EMessage}", userId, e.Message);
            throw;
        }
    }

    public async Task<bool> TakeOpdsAccessAsync(Guid userId)
    {
        try
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            user.CanAccessOpds = false;
            
             await _userRepository.UpdateUserAsync(user);
            
             return await Task.FromResult(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to take opds access from user {UserId}. {EMessage}", userId, e.Message);
            throw;
        }
    }
}