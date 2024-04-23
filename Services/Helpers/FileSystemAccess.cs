using IImage = Microsoft.Maui.Graphics.IImage;

namespace JITs.Services.Helpers;

public class FileSystemAccess : IFileSystemAccess
{
    private void CreateFolderIfNotExist(string path)
    {
        if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
    }

    public void UploadFile(string fileName, Stream file)
    {
        string path = GetFileUploadAbsolutePath(fileName);
        // Save the uploaded file to the specified path
        using var fileStream = new FileStream(path, FileMode.Create);
        file.CopyTo(fileStream);
    }

    public async Task UploadFileAsync(string fileName, Stream file)
    {
        string path = GetFileUploadAbsolutePath(fileName);
        // Save the uploaded file to the specified path
        try
        {
            using var fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void UploadImage(string fileName, Stream file)
    {
        try
        {
            string path = GetFileUploadAbsolutePath(fileName);
            
            var imageService = new Microsoft.Maui.Graphics.Skia.SkiaImageLoadingService();
            using (var fileStream = new FileStream(path, FileMode.Create))
            using (IImage image = imageService.FromStream(file))
            {
                // Set the length of the file to preallocate space
                fileStream.SetLength(image.AsStream().Length);
                image.Save(fileStream);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    private string GetFileUploadAbsolutePath(string fileName) => System.IO.Path.Combine(GetStorageDirectory(), fileName);
    
    public string GetStorageDirectory()
    {
        string configPath = "JITs";
        var filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configPath);
        if (!string.IsNullOrWhiteSpace(filePath))
            CreateFolderIfNotExist(filePath);
        return filePath;
    }

    public Stream OpenFileStream(string fileName)
    {
        string path = GetFileUploadAbsolutePath(fileName);
        return new FileStream(path, FileMode.Open, FileAccess.Read);
    }
}
