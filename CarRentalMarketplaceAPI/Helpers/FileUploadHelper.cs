using Microsoft.AspNetCore.Http;

namespace CarRentalMarketplaceAPI.Helpers;

public static class FileUploadHelper
{
    public static async Task<string> SaveFileAsync(IFormFile file, string rootPath, string folderName)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Şəkil faylı boşdur");

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";

        var folderPath = Path.Combine(rootPath, folderName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine(folderName, fileName).Replace("\\", "/");
    }
}