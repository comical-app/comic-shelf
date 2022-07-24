using File = Models.File;

namespace API.Interfaces;

public interface IFileRepository
{
    Task Save(File? file);

    Task<File?> GetFileByName(string name);

    Task<File?> GetFileById(Guid id);

    IEnumerable<File> ReturnFiles();
}