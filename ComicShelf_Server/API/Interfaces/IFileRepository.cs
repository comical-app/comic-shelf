using File = Models.File;

namespace API.Interfaces;

public interface IFileRepository
{
    void Save(File file);

    IEnumerable<File> ReturnFiles();
}