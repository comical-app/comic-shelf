using Models.Commands;
using Models.Domain;

namespace Models.ServicesInterfaces;

public interface ILibraryService
{
    Task<IEnumerable<Library>> ListLibrariesAsync();

    Task<Library?> GetLibraryByIdAsync(Guid libraryId);
    
    Task<bool> CheckLibraryNameIsUniqueAsync(string libraryName);
    
    Task<bool> CheckLibraryPathIsUniqueAsync(string libraryPath);

    Task<Library> CreateLibraryAsync(CreateLibraryCommand command);
    
    Task<bool> UpdateLibraryAsync(Guid libraryId, UpdateLibraryCommand command);
    
    Task<bool> DeleteLibraryAsync(Guid libraryId);

    Task UpdateLastScanDate(Guid libraryId);
}