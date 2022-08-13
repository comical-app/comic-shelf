using Infra.Context;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.RepositoryInterfaces;

namespace Infra.Repositories;

public class ComicFileRepository : IComicFileRepository
{
    private readonly DatabaseContext _context;

    public ComicFileRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ComicFile> SaveFileAsync(ComicFile comicFile)
    {
        comicFile.CreatedAt = DateTime.Now;
        comicFile.UpdatedAt = DateTime.Now;
        comicFile.Analysed = false;
        
        await _context.ComicFiles.AddAsync(comicFile);
        await _context.SaveChangesAsync();
        
        return await Task.FromResult(comicFile);
    }

    public async Task<ComicFile> UpdateFileAsync(ComicFile comicFile)
    {
        comicFile.UpdatedAt = DateTime.Now;
        _context.ComicFiles.Update(comicFile);
        await _context.SaveChangesAsync();
        
        return await Task.FromResult(comicFile);
    }

    public async Task<ComicFile?> GetFileByNameAsync(string filename)
    {
        return await _context.ComicFiles.FirstOrDefaultAsync(x => x.Name == filename);
    }

    public async Task<ComicFile?> GetFileByIdAsync(Guid fileId)
    {
        return await _context.ComicFiles.FirstOrDefaultAsync(x => x.Id == fileId);
    }

    public async Task<IEnumerable<ComicFile>> ReturnFilesAsync()
    {
        return await _context.ComicFiles.OrderBy(x => x.UpdatedAt).ToListAsync();
    }

    public async Task<IEnumerable<ComicFile>> ReturnFilesByLibraryIdAsync(Guid libraryId)
    {
        return await _context.ComicFiles.Where(x => x.LibraryId == libraryId).OrderBy(x => x.UpdatedAt)
            .ToListAsync();
    }

    public async Task<bool> CheckFileExistsByFilenameAsync(string filename)
    {
        var file = await _context.ComicFiles.AsNoTracking().FirstOrDefaultAsync(x => x.Name == filename.Trim());
        
        return file != null;
    }
}