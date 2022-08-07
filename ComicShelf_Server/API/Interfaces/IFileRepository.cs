using File = Models.File;

namespace API.Interfaces;

public interface IFileRepository
{
    Task<File> SaveAsync(File file);

    Task<File?> GetFileByNameAsync(string filename);

    Task<File?> GetFileByIdAsync(Guid fileId);

    Task<IEnumerable<File>> ReturnFilesAsync();

    Task<IEnumerable<File>> ReturnFilesByLibraryIdAsync(Guid libraryId);

    Task<bool> CheckFileExistsAsync(string filename);
}