using Microsoft.AspNetCore.Http;

namespace CarRentalMarketplaceAPI.Helpers;

public static class FileUploadHelper
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

    public static async Task<string> SaveFileAsync(IFormFile file, string rootPath, string folderName)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Şəkil faylı boşdur");

        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!AllowedExtensions.Contains(extension))
            throw new Exception("Yalnız .jpg, .jpeg, .png, .webp faylları qəbul olunur");

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

    public static void DeleteFile(string rootPath, string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return;

        var fullPath = Path.Combine(rootPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}