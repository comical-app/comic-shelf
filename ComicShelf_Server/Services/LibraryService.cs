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

            return await _libraryRepository.CheckLibraryFolderPathIsUniqueAsync(libraryPath.Trim());
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
            if (!await CheckLibraryNameIsUniqueAsync(command.Name)) throw new Exception("Name already exists");

            var folderExists = false;
            foreach (var folder in command.FoldersPath)
            {
                if (!await CheckLibraryPathIsUniqueAsync(folder)) folderExists = true;
            }
            
            if(folderExists) throw new Exception("Path already used");

            var newLibrary = new Library
            {
                Name = command.Name,
                Folders = command.FoldersPath.Select(x => new LibraryFolder {Path = x.Trim(), IsActive = true}).ToList(),
                AcceptedExtensions = string.Join(",", command.AcceptedExtensions)
            };

            return await _libraryRepository.CreateLibraryAsync(newLibrary);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to create library {LibraryName} at {LibraryPath}. {EMessage}", command.Name,
                command.FoldersPath, e.Message);
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

            libraryToEdit.Name = command.Name.Trim();
            libraryToEdit.Folders = command.FoldersPath.Select(x => new LibraryFolder {Path = x.Trim()}).ToList();
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