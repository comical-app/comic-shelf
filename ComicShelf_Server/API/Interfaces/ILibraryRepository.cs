using API.Domain.Commands;
using Models;

namespace API.Interfaces;

public interface ILibraryRepository
{
    Task<IEnumerable<Library>> ListLibrariesAsync();

    Task<Library?> GetLibraryByIdAsync(Guid id);
    
    Task<bool> CheckLibraryNameIsUniqueAsync(string name);
    
    Task<bool> CheckLibraryPathIsUniqueAsync(string path);

    Task<Library> CreateLibraryAsync(CreateLibraryRequest library);
    
    Task<bool> UpdateLibraryAsync(Guid libraryId, UpdateLibraryRequest library);
    
    Task<bool> DeleteLibraryAsync(Guid id);
}