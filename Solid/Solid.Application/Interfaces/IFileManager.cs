namespace Solid.Application.Interfaces;

public interface IFileManager
{
    Task<bool> IsFileExistsAsync(string path);
    Task<byte[]> GetFileDataAsync(string path);

    Task<string> ToHostingEnvironmentAsync(string path);
}
