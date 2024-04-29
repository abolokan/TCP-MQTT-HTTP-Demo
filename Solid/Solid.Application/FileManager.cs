using Solid.Application.Interfaces;

namespace Solid.Application;

public class FileManager : IFileManager
{
    public async Task<bool> IsFileExistsAsync(string path)
    {
        return File.Exists(await ToHostingEnvironmentAsync(path));
    }

    public async Task<byte[]> GetFileDataAsync(string path)
    {
        return await File.ReadAllBytesAsync(await ToHostingEnvironmentAsync(path));
    }

    public async Task<string> ToHostingEnvironmentAsync(string path)
    {
        return await Task.FromResult(
            Path.IsPathRooted(path)
            ? path
            : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fw", path));
    }
}
