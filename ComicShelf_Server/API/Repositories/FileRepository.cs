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

    public async Task Save(File? file)
    {
        try
        {
            var checkExistentFile = _context.Files.FirstOrDefault(x => x.Name == file.Name);
            if (checkExistentFile != null) return;
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }

    public async Task<File?> GetFileByName(string name)
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

    public async Task<File?> GetFileById(Guid id)
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

    public IEnumerable<File> ReturnFiles()
    {
        try
        {
            var files = _context.Files.OrderBy(x => x.LastModifiedDate);

            return files;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }
}