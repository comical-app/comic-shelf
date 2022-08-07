using API.Interfaces;
using Infra.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using File = Models.File;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class LibraryController : ControllerBase
{
    private readonly IOptions<LibrariesConfig> _libraryConfig;
    private readonly IFileRepository _fileRepository;
    private readonly ILogger<LibraryController> _logger;

    public LibraryController(IOptions<LibrariesConfig> libraryConfig, IFileRepository fileRepository,
        ILogger<LibraryController> logger)
    {
        _libraryConfig = libraryConfig;
        _fileRepository = fileRepository;
        _logger = logger;
    }

    // [HttpPost]
    // public IActionResult InitializeLibraries()
    // {
    //     return Ok();
    // }
    //
    // [HttpGet]
    // public ActionResult<IEnumerable<Library>> GetLibraries()
    // {
    //     var libraries = _libraries.Value;
    //
    //     return Ok(libraries);
    // }
    //
    // [HttpGet("{id:guid}")]
    // public ActionResult<Library> GetLibrary(Guid id)
    // {
    //     var libraries = _libraries.Value;
    //     var library = libraries.FirstOrDefault(l => l.Id == id);
    //     
    //     return Ok(library);
    // }
    //
    // [HttpPost]
    // public IActionResult CreateLibrary([FromBody] string name)
    // {
    //     return Ok($"Library {name} created");
    // }
    //
    // [HttpPut("{id:guid}")]
    // public IActionResult UpdateLibrary(Guid id, [FromBody] string name)
    // {
    //     return Ok($"Library {id} updated to {name}");
    // }
    //
    // [HttpDelete("{id:guid}")]
    // public IActionResult DeleteLibrary(Guid id)
    // {
    //     return Ok($"Library {id} deleted");
    // }

    [HttpGet(Name = "scan-library")]
    public async Task<IActionResult> ScanLibrary()
    {
        var libraries = _libraryConfig.Value.Libraries.ToList();

        try
        {
            var newFilesCount = 0;
            var library = libraries[0];
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

                if (await _fileRepository.GetFileByNameAsync(file.Name) != null) continue;

                await _fileRepository.SaveAsync(file);
                newFilesCount++;
            }

            return Ok(newFilesCount == 0 ? "No new file added" : $"{newFilesCount} new files added");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest();
        }
    }
}