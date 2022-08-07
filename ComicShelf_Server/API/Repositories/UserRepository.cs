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

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return user ?? null;
    }

    public async Task<bool> CheckIfUsernameExistsAsync(string username)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x =>
            string.Equals(x.Username, username.Trim(), StringComparison.CurrentCultureIgnoreCase));
        return user != null;
    }

    public async Task<User> CreateUserAsync(CreateUserRequest user)
    {
        if (string.IsNullOrWhiteSpace(user.Username)) throw new ArgumentException("Username cannot be empty");

        if (string.IsNullOrWhiteSpace(user.Password)) throw new ArgumentException("Password cannot be empty");

        if (await CheckIfUsernameExistsAsync(user.Username)) throw new Exception("Username already exists");

        var newUser = new User
        {
            Username = user.Username.Trim(),
            Password = user.Password,
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

    public async Task<bool> UpdateUserAsync(User user)
    {
        var userToEdit = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
        if (userToEdit == null) return false;

        if (user.Username != userToEdit.Username)
        {
            if (await CheckIfUsernameExistsAsync(user.Username)) throw new Exception("Username already exists");
        }

        userToEdit.Username = user.Username.Trim();
        userToEdit.IsActive = user.IsActive;
        userToEdit.IsAdmin = user.IsAdmin;
        userToEdit.CanAccessOpds = user.CanAccessOpds;
        userToEdit.UpdatedAt = DateTime.Now;
        _context.Users.Update(userToEdit);
        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.Username == username && x.Password == password && x.IsActive);
        if (user == null) return null;

        user.LastLogin = DateTime.Now;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> ResetPasswordAsync(Guid userId, string newPassword, string newPasswordConfirmation)
    {
        if (string.IsNullOrWhiteSpace(newPassword)) throw new Exception("New password cannot be empty");

        if (string.IsNullOrWhiteSpace(newPasswordConfirmation)) throw new Exception("New password confirmation cannot be empty");

        if (newPassword != newPasswordConfirmation)throw new Exception("New password and new password confirmation do not match");

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) return false;

        user.Password = newPassword;
        user.UpdatedAt = DateTime.Now;
        _context.Users.Update(user);

        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword,
        string newPasswordConfirmation)
    {
        if (string.IsNullOrWhiteSpace(oldPassword)) throw new Exception("Old password cannot be empty");

        if (string.IsNullOrWhiteSpace(newPassword)) throw new Exception("New password cannot be empty");

        if (string.IsNullOrWhiteSpace(newPasswordConfirmation)) throw new Exception("New password confirmation cannot be empty");

        if (newPassword != newPasswordConfirmation) throw new Exception("New password and new password confirmation do not match");

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId && x.Password == oldPassword);
        if (user == null) return false;

        user.Password = newPassword;
        user.UpdatedAt = DateTime.Now;
        _context.Users.Update(user);

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