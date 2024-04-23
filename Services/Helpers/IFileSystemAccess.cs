namespace JITs.Services.Helpers;

public interface IFileSystemAccess
{
    string GetStorageDirectory();
    Stream OpenFileStream(string fileName);
    void UploadFile(string fileName, Stream file);
    Task UploadFileAsync(string fileName, Stream file);
    void UploadImage(string fileName, Stream file);
}
