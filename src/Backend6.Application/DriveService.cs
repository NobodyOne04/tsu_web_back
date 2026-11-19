using Backend6.Core;
using Backend6.Core.Drive;

namespace Backend6.Application;

/// <summary>Private folders and files: owners only (admins see everything).</summary>
public class DriveService(IDriveRepository repo, IFileStore files, IClock clock, IUnitOfWork uow)
{
    public Task<IReadOnlyList<Folder>> ListRootFoldersAsync(Actor? actor)
    {
        var user = Rules.SignedIn(actor, "use the drive");
        return repo.ListRootFoldersAsync(user.IsAdmin ? null : user.UserId);
    }

    public async Task<Folder> GetFolderAsync(Actor? actor, Guid id)
    {
        var folder = await repo.FindFolderAsync(id) ?? throw new NotFoundException("Folder", id);
        // someone else's folder looks the same as a missing one
        return folder.CanAccess(actor) ? folder : throw new NotFoundException("Folder", id);
    }

    public async Task<Folder> CreateFolderAsync(Actor? actor, string name, Guid? parentId)
    {
        var owner = Rules.SignedIn(actor, "use the drive");
        var parent = parentId is { } id ? await GetFolderAsync(owner, id) : null;
        var folder = new Folder(owner, name, parent);
        repo.Add(folder);
        await uow.SaveChangesAsync();
        return folder;
    }

    public async Task RenameFolderAsync(Actor? actor, Guid id, string name)
    {
        (await GetFolderAsync(actor, id)).Rename(Rules.SignedIn(actor, "use the drive"), name);
        await uow.SaveChangesAsync();
    }

    public async Task<Guid?> DeleteFolderAsync(Actor? actor, Guid id)
    {
        var folder = await GetFolderAsync(actor, id);
        folder.CheckCanDelete(Rules.SignedIn(actor, "use the drive"));
        repo.Remove(folder);
        await uow.SaveChangesAsync();
        return folder.ParentId;
    }

    public async Task UploadAsync(Actor? actor, Guid folderId, Upload upload)
    {
        var folder = await GetFolderAsync(actor, folderId);
        var owner = Rules.SignedIn(actor, "use the drive");
        var stored = await Uploads.StoreAsync(files, upload);
        await Uploads.CommitOrUndoAsync(uow, files, stored, () => folder.AddFile(owner, upload.FileName, upload.Length, stored, clock.UtcNow));
    }

    public async Task<DriveFile> GetFileAsync(Actor? actor, Guid id) => (await FolderOfFile(actor, id)).File(id);

    public async Task<Guid> RenameFileAsync(Actor? actor, Guid id, string name)
    {
        var folder = await FolderOfFile(actor, id);
        folder.RenameFile(Rules.SignedIn(actor, "use the drive"), id, name);
        await uow.SaveChangesAsync();
        return folder.Id;
    }

    public async Task<Guid> DeleteFileAsync(Actor? actor, Guid id)
    {
        var folder = await FolderOfFile(actor, id);
        var stored = folder.RemoveFile(Rules.SignedIn(actor, "use the drive"), id);
        await uow.SaveChangesAsync();
        files.Delete(stored);
        return folder.Id;
    }

    public async Task<Download> DownloadAsync(Actor? actor, Guid id)
    {
        var file = await GetFileAsync(actor, id);
        return new Download(file.Name + file.Extension, files.OpenRead(file.StoredName), file.IsImage);
    }

    private async Task<Folder> FolderOfFile(Actor? actor, Guid fileId)
    {
        var folder = await repo.FindFolderByFileAsync(fileId) ?? throw new NotFoundException("File", fileId);
        return folder.CanAccess(actor) ? folder : throw new NotFoundException("File", fileId);
    }
}
