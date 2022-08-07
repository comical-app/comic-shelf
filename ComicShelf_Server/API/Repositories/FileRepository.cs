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

    public async Task<File?> GetFileByNameAsync(string name)
    {
        try
        {
            return await _context.Files.FirstOrDefaultAsync(x => x.Name == name);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<File?> GetFileByIdAsync(Guid id)
    {
        try
        {
            return await _context.Files.FirstOrDefaultAsync(x => x.Id == id);
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
        var file = await _context.Files.AsNoTracking().FirstOrDefaultAsync(x =>
            string.Equals(x.Name, filename.Trim(), StringComparison.CurrentCultureIgnoreCase));
        return file != null;
    }
}