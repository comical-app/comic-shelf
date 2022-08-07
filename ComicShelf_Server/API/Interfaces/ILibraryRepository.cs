using API.Domain.Commands;
using Models;

namespace API.Interfaces;

public interface ILibraryRepository
{
    Task<IEnumerable<Library>> ListLibrariesAsync();

    Task<Library?> GetLibraryByIdAsync(Guid libraryId);
    
    Task<bool> CheckLibraryNameIsUniqueAsync(string libraryName);
    
    Task<bool> CheckLibraryPathIsUniqueAsync(string libraryPath);

    Task<Library> CreateLibraryAsync(CreateLibraryRequest library);
    
    Task<bool> UpdateLibraryAsync(Guid libraryId, UpdateLibraryRequest library);
    
    Task<bool> DeleteLibraryAsync(Guid libraryId);

    Task UpdateLastScanDate(Guid libraryId);
}