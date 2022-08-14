using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Models.ComicInfoModels;
using Models.ServicesInterfaces;
using SharpCompress.Archives;

namespace Services;

public class ComicInfoService : IComicInfoService
{
    private readonly ILogger<ComicInfoService> _logger;
    private readonly IComicFileService _comicFileService;

    public ComicInfoService(IComicFileService comicFileService, ILogger<ComicInfoService> logger)
    {
        _comicFileService = comicFileService;
        _logger = logger;
    }

    public async Task<ComicInfo?> ExtractComicInfoAsync(string filepath)
    {
        try
        {
            // if (!await _comicFileService.CheckFileExistsByFilenameAsync(filepath)) return null;

            using var compressedFile = ArchiveFactory.Open(filepath);
            var filesEntries = compressedFile.Entries;
            var comicInfoFile = filesEntries.FirstOrDefault(x => x.Key.ToLower().EndsWith("comicinfo.xml"));

            if (comicInfoFile == null) return null;

            await using var stream = comicInfoFile.OpenEntryStream();
            var serializer = new XmlSerializer(typeof(ComicInfo));
            var comicInfo = (ComicInfo) serializer.Deserialize(stream);
            
            return comicInfo;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to verify comic info exists. {EMessage}", e.Message);
            return null;
        }
    }

    public async Task<ComicInfo> AddComicInfoForFileAsync(ComicInfo comicInfo, string filepath)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> VerifyFilepathHasComicInfoFileAsync(string filepath)
    {
        try
        {
            if (!await _comicFileService.CheckFileExistsByFilenameAsync(filepath)) return false;

            using var archive = ArchiveFactory.Open(filepath);
            var filesEntries = archive.Entries;
            var comicInfoFile = filesEntries.FirstOrDefault(x => x.Key.EndsWith("comicinfo.xml"));

            return comicInfoFile != null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to verify comic info exists for file \"{Filepath}\". {EMessage}", filepath,
                e.Message);
            return false;
        }
    }
}