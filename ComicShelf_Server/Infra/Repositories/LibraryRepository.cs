using Infra.Context;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.RepositoryInterfaces;

namespace Infra.Repositories;

public class LibraryRepository : ILibraryRepository
{
    private readonly DatabaseContext _context;

    public LibraryRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Library>> ListLibrariesAsync()
    {
        return await _context.Libraries.ToListAsync();
    }

    public async Task<Library?> GetLibraryByIdAsync(Guid libraryId)
    {
        var library = await _context.Libraries.Include(x => x.Folders).AsNoTracking().FirstOrDefaultAsync(x => x.Id == libraryId);
        
        return library ?? null;
    }

    public async Task<bool> CheckLibraryNameIsUniqueAsync(string libraryName)
    {
        var library = await _context.Libraries.AsNoTracking().FirstOrDefaultAsync(x => x.Name == libraryName.Trim());

        return library == null;
    }

    public async Task<bool> CheckLibraryFolderPathIsUniqueAsync(string libraryPath)
    {
        var library = await _context.LibraryFolders.AsNoTracking().FirstOrDefaultAsync(x => x.Path == libraryPath.Trim());

        return library == null;
    }

    public async Task<Library> CreateLibraryAsync(Library library)
    {
        try
        {
            library.CreatedAt = DateTime.Now;
            library.UpdatedAt = DateTime.Now;
            library.IsActive = true;
            await _context.Libraries.AddAsync(library);
            await _context.SaveChangesAsync();

            return await Task.FromResult(library);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<bool> UpdateLibraryAsync(Library library)
    {
        library.UpdatedAt = DateTime.Now;
        _context.Libraries.Update(library);
        await _context.SaveChangesAsync();
        
        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteLibraryAsync(Library library)
    {
        _context.Libraries.Remove(library);
        await _context.SaveChangesAsync();
        
        return await Task.FromResult(true);
    }

    public async Task<bool> UpdateLibraryFolderAsync(LibraryFolder libraryFolder)
    {
        _context.LibraryFolders.Update(libraryFolder);
        await _context.SaveChangesAsync();
        
        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteLibraryFolderAsync(LibraryFolder libraryFolder)
    {
        _context.LibraryFolders.Remove(libraryFolder);
        await _context.SaveChangesAsync();
        
        return await Task.FromResult(true);
    }
}