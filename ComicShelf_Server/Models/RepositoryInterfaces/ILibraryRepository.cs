using Models.Domain;

namespace Models.RepositoryInterfaces;

public interface ILibraryRepository
{
    Task<IEnumerable<Library>> ListLibrariesAsync();

    Task<Library?> GetLibraryByIdAsync(Guid libraryId);
    
    Task<bool> CheckLibraryNameIsUniqueAsync(string libraryName);
    
    Task<bool> CheckLibraryPathIsUniqueAsync(string libraryPath);

    Task<Library> CreateLibraryAsync(Library library);
    
    Task<bool> UpdateLibraryAsync(Library library);
    
    Task<bool> DeleteLibraryAsync(Library library);
}