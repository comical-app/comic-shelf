using API.Context;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = Models.File;

namespace API.Repositories;

public class FileRepository : IFileRepository
{
    private readonly DatabaseContext _context;

    public FileRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<File> SaveAsync(File file)
    {
        try
        {
            if (await CheckFileExistsAsync(file.Name)) throw new Exception("File already exists");

            file.AddedAt = DateTime.Now;
            await _context.Files.AddAsync(file);
            await _context.SaveChangesAsync();
            return await Task.FromResult(file);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<File?> GetFileByNameAsync(string filename)
    {
        try
        {
            return await _context.Files.FirstOrDefaultAsync(x => x.Name == filename);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<File?> GetFileByIdAsync(Guid fileId)
    {
        try
        {
            return await _context.Files.FirstOrDefaultAsync(x => x.Id == fileId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<IEnumerable<File>> ReturnFilesAsync()
    {
        try
        {
            return await _context.Files.OrderBy(x => x.LastModifiedDate).ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<bool> CheckFileExistsAsync(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentException("Filename cannot be empty");
        
        var file = await _context.Files.AsNoTracking().FirstOrDefaultAsync(x => x.Name == filename.Trim());
        return file != null;
    }
}