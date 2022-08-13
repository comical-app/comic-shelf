using Models.Domain;

namespace Models.ServicesInterfaces;

public interface IFileService
{
    Task<ComicFile> SaveFileAsync(ComicFile comicFile);

    Task<ComicFile?> GetFileByNameAsync(string filename);

    Task<ComicFile?> GetFileByIdAsync(Guid fileId);

    Task<IEnumerable<ComicFile>> ReturnFilesAsync();

    Task<IEnumerable<ComicFile>> ReturnFilesByLibraryIdAsync(Guid libraryId);

    Task<bool> CheckFileExistsByFilenameAsync(string filename);
    
    Task SetFileToBeAnalyzedAsync(string filename);
    
    Task SetFileAsAnalyzedAsync(string filename);
}