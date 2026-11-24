using Backend6.Application;

namespace Backend6.Infrastructure;

/// <summary>Files in one folder outside the web root, named by random ids (never by user input).</summary>
public class LocalFileStore : IFileStore
{
    private readonly string _root;

    public LocalFileStore(string root)
    {
        _root = Path.GetFullPath(root);
        Directory.CreateDirectory(_root);
    }

    public async Task<string> SaveAsync(Stream content)
    {
        var name = Guid.NewGuid().ToString("N");
        await using var file = File.Create(PathOf(name));
        await content.CopyToAsync(file);
        return name;
    }

    public Stream OpenRead(string storedName) => File.OpenRead(PathOf(storedName));

    public void Delete(string storedName) => File.Delete(PathOf(storedName));

    private string PathOf(string storedName) =>
        Guid.TryParseExact(storedName, "N", out _) ? Path.Combine(_root, storedName)
            : throw new ArgumentException("Not a stored file name.", nameof(storedName));
}

internal class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
