using Microsoft.Extensions.Logging;
using Models.Commands;
using Models.Domain;
using Models.RepositoryInterfaces;
using Models.ServicesInterfaces;

namespace Services;

public class LibraryService : ILibraryService
{
    private readonly ILogger<LibraryService> _logger;
    private readonly ILibraryRepository _libraryRepository;

    public LibraryService(ILibraryRepository libraryRepository, ILogger<LibraryService> logger)
    {
        _libraryRepository = libraryRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Library>> ListLibrariesAsync()
    {
        try
        {
            return await _libraryRepository.ListLibrariesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to list libraries. {EMessage}", e.Message);
            throw;
        }
    }

    public async Task<Library?> GetLibraryByIdAsync(Guid libraryId)
    {
        try
        {
            return await _libraryRepository.GetLibraryByIdAsync(libraryId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get library by id ({LibraryId}). {EMessage}", libraryId, e.Message);
            throw;
        }
    }

    public async Task<bool> CheckLibraryNameIsUniqueAsync(string libraryName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(libraryName)) throw new ArgumentException("Name cannot be empty");

            return await _libraryRepository.CheckLibraryNameIsUniqueAsync(libraryName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to check if library name {LibraryName} is unique. {EMessage}", libraryName,
                e.Message);
            throw;
        }
    }

    public async Task<bool> CheckLibraryPathIsUniqueAsync(string libraryPath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(libraryPath)) throw new ArgumentException("Path cannot be empty");

            return await _libraryRepository.CheckLibraryPathIsUniqueAsync(libraryPath);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to check if library path {LibraryPath} is unique. {EMessage}", libraryPath,
                e.Message);
            throw;
        }
    }

    public async Task<Library> CreateLibraryAsync(CreateLibraryCommand command)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.Name)) throw new Exception("Name cannot be empty");
            if (string.IsNullOrWhiteSpace(command.Path)) throw new Exception("Path cannot be empty");
            if (!await CheckLibraryNameIsUniqueAsync(command.Name)) throw new Exception("Name already exists");
            if (!await CheckLibraryPathIsUniqueAsync(command.Path)) throw new Exception("Path already used");

            var newLibrary = new Library
            {
                Name = command.Name,
                Path = command.Path,
                AcceptedExtensions = string.Join(",", command.AcceptedExtensions)
            };

            return await _libraryRepository.CreateLibraryAsync(newLibrary);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to create library {LibraryName} at {LibraryPath}. {EMessage}", command.Name,
                command.Path, e.Message);
            throw;
        }
    }

    public async Task<bool> UpdateLibraryAsync(Guid libraryId, UpdateLibraryCommand command)
    {
        try
        {
            if (command == null) throw new ArgumentException("Library cannot be null");

            var libraryToEdit = await GetLibraryByIdAsync(libraryId);
            if (libraryToEdit == null) return false;

            if (command.Name != libraryToEdit.Name)
            {
                if (string.IsNullOrWhiteSpace(command.Name)) throw new ArgumentException("Name cannot be empty");
                if (!await CheckLibraryNameIsUniqueAsync(command.Name))
                    throw new ArgumentException("Name already exists");
            }

            if (command.Path != libraryToEdit.Path)
            {
                if (string.IsNullOrWhiteSpace(command.Path)) throw new ArgumentException("Path cannot be empty");
                if (!await CheckLibraryPathIsUniqueAsync(command.Name))
                    throw new ArgumentException("Path already used");
            }

            libraryToEdit.Name = command.Name.Trim();
            libraryToEdit.Path = command.Path;
            libraryToEdit.AcceptedExtensions = string.Join(",", command.AcceptedExtensions);

            return await _libraryRepository.UpdateLibraryAsync(libraryToEdit);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to update library {LibraryId}. {EMessage}", libraryId, e.Message);
            throw;
        }
    }

    public async Task<bool> DeleteLibraryAsync(Guid libraryId)
    {
        try
        {
            var library = await _libraryRepository.GetLibraryByIdAsync(libraryId);
            if (library == null) return false;

            return await _libraryRepository.DeleteLibraryAsync(library);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to delete library {LibraryId}. {EMessage}", libraryId, e.Message);
            throw;
        }
    }

    public async Task UpdateLastScanDate(Guid libraryId)
    {
        try
        {
            var library = await _libraryRepository.GetLibraryByIdAsync(libraryId);
            if (library == null) throw new Exception("Library cannot be null");

            library.LastScan = DateTime.Now;
            await _libraryRepository.UpdateLibraryAsync(library);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to update last scan date for library {LibraryId}. {EMessage}", libraryId,
                e.Message);
            throw;
        }
    }
}