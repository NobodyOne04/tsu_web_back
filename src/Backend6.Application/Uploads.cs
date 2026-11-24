using Backend6.Core;

namespace Backend6.Application;

/// <summary>Store first, then commit; undo the stored file if the commit fails. Delete files only after a commit.</summary>
internal static class Uploads
{
    public static async Task<string> StoreAsync(IFileStore files, Upload upload)
    {
        if (upload.Length <= 0) throw new DomainException("Choose a file.");
        if (upload.Length > Rules.MaxUploadBytes)
            throw new DomainException($"Files must be at most {Rules.MaxUploadBytes / 1024 / 1024} MB.");
        Rules.FileName(upload.FileName);
        await using var content = upload.Open();
        return await files.SaveAsync(content);
    }

    public static async Task CommitOrUndoAsync(IUnitOfWork uow, IFileStore files, string storedName, Action attach)
    {
        try
        {
            attach();
            await uow.SaveChangesAsync();
        }
        catch
        {
            files.Delete(storedName);
            throw;
        }
    }

    public static void DeleteAll(IFileStore files, IEnumerable<string> storedNames)
    {
        foreach (var name in storedNames) files.Delete(name);
    }
}
