using API.Context;
using API.Domain.Commands;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace API.Repositories;

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
        var library = await _context.Libraries.AsNoTracking().FirstOrDefaultAsync(x => x.Id == libraryId);
        return library ?? null;
    }

    public async Task<bool> CheckLibraryNameIsUniqueAsync(string libraryName)
    {
        if (string.IsNullOrWhiteSpace(libraryName)) throw new ArgumentException("Name cannot be empty");

        var library = await _context.Libraries.AsNoTracking().FirstOrDefaultAsync(x => x.Name == libraryName.Trim());

        return library == null;
    }

    public async Task<bool> CheckLibraryPathIsUniqueAsync(string libraryPath)
    {
        if (string.IsNullOrWhiteSpace(libraryPath)) throw new ArgumentException("Path cannot be empty");

        var library = await _context.Libraries.AsNoTracking().FirstOrDefaultAsync(x => x.Path == libraryPath.Trim());

        return library == null;
    }

    public async Task<Library> CreateLibraryAsync(CreateLibraryRequest library)
    {
        if (string.IsNullOrWhiteSpace(library.Name)) throw new Exception("Name cannot be empty");
        if (string.IsNullOrWhiteSpace(library.Path)) throw new Exception("Path cannot be empty");
        if (!await CheckLibraryNameIsUniqueAsync(library.Name)) throw new Exception("Name already exists");
        if (!await CheckLibraryPathIsUniqueAsync(library.Path)) throw new Exception("Path already used");

        var newLibrary = new Library
        {
            Name = library.Name,
            Path = library.Path,
            AcceptedExtensions = string.Join(",", library.AcceptedExtensions)
        };

        await _context.Libraries.AddAsync(newLibrary);
        await _context.SaveChangesAsync();
        return await Task.FromResult(newLibrary);
    }

    public async Task<bool> UpdateLibraryAsync(Guid libraryId, UpdateLibraryRequest library)
    {
        if (library == null) throw new ArgumentException("Library cannot be null");

        var libraryToEdit = await _context.Libraries.FirstOrDefaultAsync(x => x.Id == libraryId);
        if (libraryToEdit == null) return false;

        if (library.Name != libraryToEdit.Name)
        {
            if (string.IsNullOrWhiteSpace(library.Name)) throw new ArgumentException("Name cannot be empty");
            if (!await CheckLibraryNameIsUniqueAsync(library.Name)) throw new ArgumentException("Name already exists");
        }

        if (library.Path != libraryToEdit.Path)
        {
            if (string.IsNullOrWhiteSpace(library.Path)) throw new ArgumentException("Path cannot be empty");
            if (!await CheckLibraryPathIsUniqueAsync(library.Name)) throw new ArgumentException("Path already used");
        }

        libraryToEdit.Name = library.Name.Trim();
        libraryToEdit.Path = library.Path;
        libraryToEdit.AcceptedExtensions = string.Join(",", library.AcceptedExtensions);
        _context.Libraries.Update(libraryToEdit);
        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteLibraryAsync(Guid libraryId)
    {
        var library = await _context.Libraries.FirstOrDefaultAsync(x => x.Id == libraryId);
        if (library == null) return false;

        _context.Libraries.Remove(library);
        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task UpdateLastScanDate(Guid libraryId)
    {
        var library = await _context.Libraries.FirstOrDefaultAsync(x => x.Id == libraryId);
        if (library == null) throw new Exception("Library cannot be null");

        library.LastScan = DateTime.Now;
        _context.Libraries.Update(library);
        await _context.SaveChangesAsync();
    }
}