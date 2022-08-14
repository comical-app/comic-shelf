namespace Services.Helpers;

public static class FileHelpers
{
    public static string GetMimeTypeFromExtension(string extension)
    {
        if (extension == null)
        {
            throw new ArgumentNullException(nameof(extension));
        }
        
        if (extension.StartsWith(".")){
            extension = extension[1..];
        }

        if (extension.Equals("rar", StringComparison.OrdinalIgnoreCase) || extension.Equals("cbr", StringComparison.OrdinalIgnoreCase))
        {
            return "application/x-rar-compressed";
        }

        if (extension.Equals("zip", StringComparison.OrdinalIgnoreCase) || extension.Equals("cbz", StringComparison.OrdinalIgnoreCase))
        {
            return "application/zip";
        }
        
        if (extension.Equals("7z", StringComparison.OrdinalIgnoreCase))
        {
            return "application/x-7z-compressed";
        }
        
        return extension.Equals("pdf", StringComparison.OrdinalIgnoreCase) ? "application/pdf" : "application/octet-stream";
    }
    
    public static bool VerifyIfFileIsCompressed(string filePath)
    {
        if (filePath == null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }
        
        var extension = Path.GetExtension(filePath);
        return extension.Equals(".rar", StringComparison.OrdinalIgnoreCase) || extension.Equals(".cbr", StringComparison.OrdinalIgnoreCase) || extension.Equals(".zip", StringComparison.OrdinalIgnoreCase) || extension.Equals(".cbz", StringComparison.OrdinalIgnoreCase) || extension.Equals(".7z", StringComparison.OrdinalIgnoreCase);
    }
}