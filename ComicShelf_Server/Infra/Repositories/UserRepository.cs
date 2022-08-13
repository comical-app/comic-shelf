using Infra.Context;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.RepositoryInterfaces;

namespace Infra.Repositories;

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

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username);
        return user ?? null;
    }

    public async Task<bool> CheckIfUsernameIsUniqueAsync(string username)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username.Trim());

        return user == null;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        user.CreatedAt = DateTime.Now;
        user.UpdatedAt = DateTime.Now;
        user.IsActive = true;
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return await Task.FromResult(user);
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        user.UpdatedAt = DateTime.Now;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return await Task.FromResult(user);
    }

    public async Task<bool> DeleteUserAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }
}