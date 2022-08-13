using Microsoft.Extensions.Logging;
using Models.Domain;
using Models.RepositoryInterfaces;
using Models.ServicesInterfaces;

namespace Services;

public class FileService : IFileService
{
    private readonly ILogger<FileService> _logger;
    private readonly IFileRepository _fileRepository;
    private readonly ILibraryService _libraryService;

    public FileService(IFileRepository fileRepository, ILibraryService libraryService, ILogger<FileService> logger)
    {
        _fileRepository = fileRepository;
        _libraryService = libraryService;
        _logger = logger;
    }

    public async Task<ComicFile> SaveFileAsync(ComicFile comicFile)
    {
        try
        {
            if (await CheckFileExistsByFilenameAsync(comicFile.Name)) throw new Exception("File already exists");

            return await _fileRepository.SaveFileAsync(comicFile);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to save file {FileName}. {EMessage}", comicFile.Name, e.Message);
            throw;
        }
    }

    public async Task<ComicFile?> GetFileByNameAsync(string filename)
    {
        try
        {
            return await _fileRepository.GetFileByNameAsync(filename);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to get file by name ({FileName}). {EMessage}", filename, e.Message);
            throw;
        }
    }

    public async Task<ComicFile?> GetFileByIdAsync(Guid fileId)
    {
        try
        {
            return await _fileRepository.GetFileByIdAsync(fileId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to get file by id ({FileId}). {EMessage}", fileId, e.Message);
            throw;
        }
    }

    public async Task<IEnumerable<ComicFile>> ReturnFilesAsync()
    {
        try
        {
            return await _fileRepository.ReturnFilesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to return files. {EMessage}", e.Message);
            throw;
        }
    }

    public async Task<IEnumerable<ComicFile>> ReturnFilesByLibraryIdAsync(Guid libraryId)
    {
        try
        {
            var library = await _libraryService.GetLibraryByIdAsync(libraryId);
            if (library == null) throw new Exception("Library not found");

            return await _fileRepository.ReturnFilesByLibraryIdAsync(libraryId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to return files by library {LibraryId}. {EMessage}", libraryId, e.Message);
            throw;
        }
    }

    public async Task<bool> CheckFileExistsByFilenameAsync(string filename)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filename)) throw new ArgumentException("Filename cannot be empty");

            return await _fileRepository.CheckFileExistsByFilenameAsync(filename);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to check file exists by filename {Filename}. {EMessage}", filename, e.Message);
            throw;
        }
    }

    public async Task SetFileToBeAnalyzedAsync(string filename)
    {
        try
        {
            var file = await GetFileByNameAsync(filename);
            if (file == null) throw new Exception("File not found");

            file.Analysed = false;

            await _fileRepository.UpdateFileAsync(file);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to set file to be analyzed {Filename}. {EMessage}", filename, e.Message);
            throw;
        }
    }

    public async Task SetFileAsAnalyzedAsync(string filename)
    {
        try
        {
            var file = await GetFileByNameAsync(filename);
            if (file == null) throw new Exception("File not found");

            file.Analysed = true;

            await _fileRepository.UpdateFileAsync(file);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Faield to set file as analyzed {Filename}. {EMessage}", filename, e.Message);
            throw;
        }
    }
}