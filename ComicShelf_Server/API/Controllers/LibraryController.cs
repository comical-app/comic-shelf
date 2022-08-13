using Infra.Helpers;
using Microsoft.AspNetCore.Mvc;
using Models.Commands;
using Models.Domain;
using Models.ServicesInterfaces;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryController : ControllerBase
{
    private readonly IComicFileService _comicFileService;
    private readonly ILibraryService _libraryService;
    private readonly ILogger<LibraryController> _logger;

    public LibraryController(IComicFileService comicFileService,
        ILogger<LibraryController> logger, ILibraryService libraryService)
    {
        _comicFileService = comicFileService;
        _logger = logger;
        _libraryService = libraryService;
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
        var libraries = await _libraryService.ListLibrariesAsync();

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
        var library = await _libraryService.GetLibraryByIdAsync(libraryId);

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
    [ProducesResponseType(typeof(IEnumerable<ComicFile>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetFilesByLibraryId(Guid libraryId)
    {
        var library = await _libraryService.GetLibraryByIdAsync(libraryId);
        
        if (library == null)
            return NotFound();

        var files = await _comicFileService.ReturnFilesByLibraryIdAsync(libraryId);

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
            var library = await _libraryService.CheckLibraryNameIsUniqueAsync(libraryName);

            return Ok(library);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to check if library with name \"{LibraryName}\" exists. {EMessage}", libraryName, e.Message);
            return BadRequest($"Failed to check if library with name \"{libraryName}\" exists.");
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
            var library = await _libraryService.CheckLibraryPathIsUniqueAsync(libraryPath);

            return Ok(library);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to check if library with path \"{LibraryPath}\" exists. {EMessage}", libraryPath, e.Message);
            return BadRequest($"Failed to check if library with path \"{libraryPath}\" exists.");
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
    public async Task<IActionResult> Post([FromBody] CreateLibraryCommand library)
    {
        try
        {
            var result = await _libraryService.CreateLibraryAsync(library);

            return Created($"/library/{result.Id}", result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create library. {EMessage}", e.Message);
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
    public async Task<IActionResult> Put(Guid libraryId, [FromBody] UpdateLibraryCommand library)
    {
        try
        {
            var checkLibrary = await _libraryService.GetLibraryByIdAsync(libraryId);

            if (checkLibrary == null)
                return NotFound();

            var result = await _libraryService.UpdateLibraryAsync(libraryId, library);

            if (result) return NoContent();

            return BadRequest("Fail to update library");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to update library. {EMessage}", e.Message);
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
            var library = await _libraryService.GetLibraryByIdAsync(libraryId);

            if (library == null)
                return NotFound();

            var result = await _libraryService.DeleteLibraryAsync(libraryId);

            if (result) return NoContent();

            return BadRequest("Fail to delete library");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete library. {EMessage}", e.Message);
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
            var library = await _libraryService.GetLibraryByIdAsync(libraryId);
        
            if (library == null)
                return NotFound();
            
            await _libraryService.UpdateLastScanDate(libraryId);
            
            var newFilesCount = 0;
            var sourceDirectory = library.Path;
            var searchPatterns = library.AcceptedExtensions.Select(x => $"*.{x}");

            var enumerateFiles = searchPatterns.AsParallel().SelectMany(searchPattern =>
                Directory.EnumerateFiles(sourceDirectory, searchPattern, SearchOption.AllDirectories));

            foreach (var currentFile in enumerateFiles)
            {
                var file = new ComicFile();
                var fileInfo = new FileInfo(currentFile);

                file.Name = fileInfo.Name;
                file.Path = fileInfo.DirectoryName ?? string.Empty;
                file.Extension = Path.GetExtension(currentFile);
                file.MimeType = FileHelpers.GetMimeTypeFromExtension(file.Extension);
                file.Size = fileInfo.Length;
                file.UpdatedAt = fileInfo.LastWriteTime;
                file.LibraryId = library.Id;

                if (await _comicFileService.GetFileByNameAsync(file.Name) != null) await _comicFileService.SetFileToBeAnalyzedAsync(file.Name);

                await _comicFileService.SaveFileAsync(file);
                newFilesCount++;
            }

            return Ok(newFilesCount == 0 ? "No new file added" : $"{newFilesCount} new files added");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to scan library. {EMessage}", e.Message);
            return BadRequest("Fail to scan library");
        }
    }
}