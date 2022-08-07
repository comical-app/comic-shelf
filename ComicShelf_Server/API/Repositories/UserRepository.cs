using API.Context;
using API.Domain.Commands;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> ListUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<IEnumerable<User>> ListUsersWithOpdsAccessAsync()
    {
        return await _context.Users.Where(x => x.CanAccessOpds).ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
        return user ?? null;
    }

    public async Task<bool> CheckIfUsernameIsUniqueAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be empty");
        
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x =>
            string.Equals(x.Username, username.Trim(), StringComparison.CurrentCultureIgnoreCase));

        return user == null;
    }

    public async Task<User> CreateUserAsync(CreateUserRequest user)
    {
        if (string.IsNullOrWhiteSpace(user.Username)) throw new ArgumentException("Username cannot be empty");
        if (string.IsNullOrWhiteSpace(user.Password)) throw new ArgumentException("Password cannot be empty");
        if (!await CheckIfUsernameIsUniqueAsync(user.Username)) throw new ArgumentException("Username already exists");

        var newUser = new User
        {
            Username = user.Username.Trim(),
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
            IsAdmin = user.IsAdmin,
            CanAccessOpds = user.CanAccessOpds,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            IsActive = true
        };
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return await Task.FromResult(newUser);
    }

    public async Task<bool> UpdateUserAsync(Guid userId, UpdateUserRequest user)
    {
        if (user == null) throw new ArgumentException("User cannot be null");
        
        var userToEdit = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (userToEdit == null) return false;

        if (user.Username != userToEdit.Username)
        {
            if (!await CheckIfUsernameIsUniqueAsync(user.Username)) throw new ArgumentException("Username already exists");
        }

        userToEdit.Username = user.Username.Trim();
        userToEdit.IsAdmin = user.IsAdmin;
        userToEdit.CanAccessOpds = user.CanAccessOpds;
        userToEdit.UpdatedAt = DateTime.Now;
        _context.Users.Update(userToEdit);
        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<User?> LoginAsync(LoginUserRequest user)
    {
        var loggedUser = await _context.Users.FirstOrDefaultAsync(x =>
            x.Username == user.Username && x.IsActive);
        if (loggedUser == null) return null;
        
        if (!BCrypt.Net.BCrypt.Verify(user.Password, loggedUser.Password)) return null;

        loggedUser.LastLogin = DateTime.Now;
        _context.Users.Update(loggedUser);
        await _context.SaveChangesAsync();
        return loggedUser;
    }

    public async Task<bool> ResetPasswordAsync(ResetUserPasswordRequest user)
    {
        if (string.IsNullOrWhiteSpace(user.NewPassword)) throw new Exception("New password cannot be empty");

        if (string.IsNullOrWhiteSpace(user.NewPasswordConfirmation)) throw new Exception("New password confirmation cannot be empty");

        if (user.NewPassword != user.NewPasswordConfirmation)throw new Exception("New password and new password confirmation do not match");

        var selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.UserId);
        if (selectedUser == null) return false;

        selectedUser.Password = user.NewPassword;
        selectedUser.UpdatedAt = DateTime.Now;
        _context.Users.Update(selectedUser);

        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<bool> ChangePasswordAsync(ChangeUserPasswordRequest user)
    {
        if (string.IsNullOrWhiteSpace(user.OldPassword)) throw new Exception("Old password cannot be empty");

        if (string.IsNullOrWhiteSpace(user.NewPassword)) throw new Exception("New password cannot be empty");

        if (string.IsNullOrWhiteSpace(user.NewPasswordConfirmation)) throw new Exception("New password confirmation cannot be empty");

        if (user.NewPassword != user.NewPasswordConfirmation) throw new Exception("New password and new password confirmation do not match");

        var selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.UserId);
        if (selectedUser == null) return false;
        
        if (!BCrypt.Net.BCrypt.Verify(user.OldPassword, selectedUser.Password)) return false;

        selectedUser.Password = user.NewPassword;
        selectedUser.UpdatedAt = DateTime.Now;
        _context.Users.Update(selectedUser);

        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<bool> ActivateUserAsync(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) return false;

        user.IsActive = true;
        user.UpdatedAt = DateTime.Now;
        _context.Users.Update(user);

        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<bool> DeactivateUserAsync(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) return false;

        user.IsActive = false;
        user.UpdatedAt = DateTime.Now;
        _context.Users.Update(user);

        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<bool> GiveOpdsAccessAsync(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) return false;

        user.CanAccessOpds = true;
        user.UpdatedAt = DateTime.Now;
        _context.Users.Update(user);

        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<bool> TakeOpdsAccessAsync(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) return false;

        user.CanAccessOpds = false;
        user.UpdatedAt = DateTime.Now;
        _context.Users.Update(user);

        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }
}