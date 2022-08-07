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

    public async Task<Library?> GetLibraryByIdAsync(Guid id)
    {
        var library = await _context.Libraries.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return library ?? null;
    }

    public async Task<bool> CheckLibraryNameIsUniqueAsync(string name)
    {
        var library = await _context.Libraries.AsNoTracking().FirstOrDefaultAsync(x =>
            string.Equals(x.Name, name.Trim(), StringComparison.CurrentCultureIgnoreCase));
        return library != null;
    }

    public async Task<bool> CheckLibraryPathIsUniqueAsync(string path)
    {
        var library = await _context.Libraries.AsNoTracking().FirstOrDefaultAsync(x =>
            string.Equals(x.Path, path.Trim(), StringComparison.CurrentCultureIgnoreCase));
        return library != null;
    }

    public async Task<Library> CreateLibraryAsync(CreateLibraryRequest library)
    {
        if (await CheckLibraryNameIsUniqueAsync(library.Name)) throw new Exception("Name already exists");
        if (await CheckLibraryPathIsUniqueAsync(library.Path)) throw new Exception("Path already used");

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

    public async Task<bool> UpdateLibraryAsync(UpdateLibraryRequest library)
    {
        var libraryToEdit = await _context.Libraries.FirstOrDefaultAsync(x => x.Id == library.Id);
        if (libraryToEdit == null) return false;

        if (library.Name != libraryToEdit.Name)
        {
            if (await CheckLibraryNameIsUniqueAsync(library.Name)) throw new Exception("Name already exists");
        }

        if (library.Path != libraryToEdit.Path)
        {
            if (await CheckLibraryPathIsUniqueAsync(library.Name)) throw new Exception("Path already used");
        }

        libraryToEdit.Name = library.Name.Trim();
        libraryToEdit.Path = library.Path;
        libraryToEdit.AcceptedExtensions = string.Join(",", library.AcceptedExtensions);
        _context.Libraries.Update(libraryToEdit);
        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteLibraryAsync(Guid id)
    {
        var library = await _context.Libraries.FirstOrDefaultAsync(x => x.Id == id);
        if (library == null) return false;

        _context.Libraries.Remove(library);
        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }
}