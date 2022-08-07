using File = Models.File;

namespace API.Interfaces;

public interface IFileRepository
{
    Task<File> SaveAsync(File file);

    Task<File?> GetFileByNameAsync(string name);

    Task<File?> GetFileByIdAsync(Guid id);

    Task<IEnumerable<File>> ReturnFilesAsync();

    Task<bool> CheckFileExistsAsync(string filename);
}