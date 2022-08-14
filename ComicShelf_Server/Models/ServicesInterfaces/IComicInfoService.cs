using Models.ComicInfoModels;

namespace Models.ServicesInterfaces;

public interface IComicInfoService
{
    public Task<ComicInfo?> ExtractComicInfoAsync(string filepath);
    
    public Task<ComicInfo> AddComicInfoForFileAsync(ComicInfo comicInfo, string filepath);
    
    public Task<bool> VerifyFilepathHasComicInfoFileAsync(string filepath);
}