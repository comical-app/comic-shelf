using API.Context;
using API.Interfaces;
using File = Models.File;

namespace API.Repositories;

public class FileRepository : IFileRepository
{
    private readonly DatabaseContext _context;

    public FileRepository(DatabaseContext context)
    {
        _context = context;
    }

    public void Save(File file)
    {
        try
        {
            using var db = _context;
            db.Add(file);
            db.SaveChanges();
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
            using var db = _context;
            var files = db.Files.OrderBy(x => x.LastModifiedDate);

            return files;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }
}