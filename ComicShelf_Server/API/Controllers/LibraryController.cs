using API.Domain.Commands;
using API.Interfaces;
using Infra.Helpers;
using Microsoft.AspNetCore.Mvc;
using Models;
using File = Models.File;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryController : ControllerBase
{
    private readonly IFileRepository _fileRepository;
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILogger<LibraryController> _logger;

    public LibraryController(IFileRepository fileRepository,
        ILogger<LibraryController> logger, ILibraryRepository libraryRepository)
    {
        _fileRepository = fileRepository;
        _logger = logger;
        _libraryRepository = libraryRepository;
    }

    /// <summary>
    /// Returns all libraries
    /// </summary>
    /// <response code="200">Libraries retrieved</response>
    /// <response code="204">No library</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Library>), 200)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Get()
    {
        var libraries = await _libraryRepository.ListLibrariesAsync();

        if (!libraries.Any()) return NoContent();

        return Ok(libraries);
    }
    
    /// <summary>
    /// Find library by id
    /// </summary>
    /// <param name="libraryId" example="e9a314af-d4b6-4907-a707-ca583571f596">Library identification</param>
    /// <response code="200">Library retrieved</response>
    /// <response code="404">Library not found</response>
    [HttpGet("{libraryId:guid}")]
    [ProducesResponseType(typeof(Library), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(Guid libraryId)
    {
        var library = await _libraryRepository.GetLibraryByIdAsync(libraryId);

        if (library == null)
            return NotFound();

        return Ok(library);
    }
    
    /// <summary>
    /// Return all files from library
    /// </summary>
    /// <param name="libraryId" example="e9a314af-d4b6-4907-a707-ca583571f596">Library identification</param>
    /// <response code="200">files retrieved</response>
    /// <response code="404">Library not found</response>
    [HttpGet("{libraryId:guid}/files")]
    [ProducesResponseType(typeof(IEnumerable<File>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetFilesByLibraryId(Guid libraryId)
    {
        var library = await _libraryRepository.GetLibraryByIdAsync(libraryId);
        
        if (library == null)
            return NotFound();

        var files = await _fileRepository.ReturnFilesByLibraryIdAsync(libraryId);

        return Ok(files);
    }

    /// <summary>
    /// Check if library with given name exists
    /// </summary>
    /// <param name="libraryName" example="comics">Name</param>
    /// <response code="200">Library with given name exists</response>
    /// <response code="500">Fail to check if the library with that name exists</response>
    [HttpGet("verify-name-is-unique/{libraryName}")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> CheckLibraryName(string libraryName)
    {
        try
        {
            var library = await _libraryRepository.CheckLibraryNameIsUniqueAsync(libraryName);

            return Ok(library);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to check if the library with given name exists");
        }
    }

    /// <summary>
    /// Check if library with given path exists
    /// </summary>
    /// <param name="libraryPath" example="C:\Comics folder">Path</param>
    /// <response code="200">Library with given path exists</response>
    /// <response code="500">Fail to check if the library with that name exists</response>
    [HttpGet("verify-path-is-unique/{libraryPath}")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> CheckLibraryPath(string libraryPath)
    {
        try
        {
            var library = await _libraryRepository.CheckLibraryPathIsUniqueAsync(libraryPath);

            return Ok(library);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to check if the library with given path exists");
        }
    }

    /// <summary>
    /// Add new library
    /// </summary>
    /// <param name="library">Library object that needs to be added</param>
    /// <response code="201">library added</response>
    /// <response code="500">Fail to create library</response>
    [HttpPost]
    [ProducesResponseType(typeof(Library), 201)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Post([FromBody] CreateLibraryRequest library)
    {
        try
        {
            var result = await _libraryRepository.CreateLibraryAsync(library);

            return Created($"/library/{result.Id}", result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to create library");
        }
    }

    /// <summary>
    /// Update a library
    /// </summary>
    /// <param name="libraryId" example="e9a314af-d4b6-4907-a707-ca583571f596">Library identification</param>
    /// <param name="library">Library object that needs to be updated</param>
    /// <response code="204">Library updated</response>
    /// <response code="404">Library not found</response>
    /// <response code="500">Fail to update Library</response>
    [HttpPut("{libraryId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Put(Guid libraryId, [FromBody] UpdateLibraryRequest library)
    {
        try
        {
            var checkLibrary = await _libraryRepository.GetLibraryByIdAsync(libraryId);

            if (checkLibrary == null)
                return NotFound();

            var result = await _libraryRepository.UpdateLibraryAsync(libraryId, library);

            if (result) return NoContent();

            return BadRequest("Fail to update library");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to update library");
        }
    }

    /// <summary>
    /// Delete a library
    /// </summary>
    /// <param name="libraryId" example="e9a314af-d4b6-4907-a707-ca583571f596">Library identification</param>
    /// <response code="204">Library deleted</response>
    /// <response code="404">Library not found</response>
    /// <response code="500">Fail to delete library</response>
    [HttpDelete("{libraryId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(Guid libraryId)
    {
        try
        {
            var library = await _libraryRepository.GetLibraryByIdAsync(libraryId);

            if (library == null)
                return NotFound();

            var result = await _libraryRepository.DeleteLibraryAsync(libraryId);

            if (result) return NoContent();

            return BadRequest("Fail to delete library");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to delete library");
        }
    }

    /// <summary>
    /// Scan a library
    /// </summary>
    /// <param name="libraryId" example="e9a314af-d4b6-4907-a707-ca583571f596">Library identification</param>
    /// <response code="200">Library scanned</response>
    /// <response code="404">Library not found</response>
    /// <response code="500">Fail to scan library</response>
    [HttpGet("{libraryId:guid}/scan")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ScanLibrary(Guid libraryId)
    {
        try
        {
            var library = await _libraryRepository.GetLibraryByIdAsync(libraryId);
        
            if (library == null)
                return NotFound();
            
            await _libraryRepository.UpdateLastScanDate(libraryId);
            
            var newFilesCount = 0;
            var sourceDirectory = library.Path;
            var searchPatterns = library.AcceptedExtensions.Select(x => $"*.{x}");

            var enumerateFiles = searchPatterns.AsParallel().SelectMany(searchPattern =>
                Directory.EnumerateFiles(sourceDirectory, searchPattern, SearchOption.AllDirectories));

            foreach (var currentFile in enumerateFiles)
            {
                var file = new File();
                var fileInfo = new FileInfo(currentFile);

                file.Name = fileInfo.Name;
                file.Path = fileInfo.DirectoryName ?? string.Empty;
                file.Extension = Path.GetExtension(currentFile);
                file.MimeType = FileHelpers.GetMimeTypeFromExtension(file.Extension);
                file.Size = fileInfo.Length;
                file.LastModifiedDate = fileInfo.LastWriteTime;
                file.LibraryId = library.Id;

                if (await _fileRepository.GetFileByNameAsync(file.Name) != null) await _fileRepository.SetFileToBeAnalyzedAsync(file.Name, file.LastModifiedDate);

                await _fileRepository.SaveAsync(file);
                newFilesCount++;
            }

            return Ok(newFilesCount == 0 ? "No new file added" : $"{newFilesCount} new files added");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to scan library");
        }
    }
}