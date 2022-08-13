using Models.Domain;

namespace Models.RepositoryInterfaces;

public interface IFileRepository
{
    Task<ComicFile> SaveFileAsync(ComicFile comicFile);

    Task<ComicFile> UpdateFileAsync(ComicFile comicFile);

    Task<ComicFile?> GetFileByNameAsync(string filename);

    Task<ComicFile?> GetFileByIdAsync(Guid fileId);

    Task<IEnumerable<ComicFile>> ReturnFilesAsync();

    Task<IEnumerable<ComicFile>> ReturnFilesByLibraryIdAsync(Guid libraryId);

    Task<bool> CheckFileExistsByFilenameAsync(string filename);
}