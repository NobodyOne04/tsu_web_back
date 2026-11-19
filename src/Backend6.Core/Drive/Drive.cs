namespace Backend6.Core.Drive;

/// <summary>A private folder; it holds subfolders and files. Only its owner (or an admin) gets in.</summary>
public class Folder
{
    private readonly List<Folder> _folders = [];
    private readonly List<DriveFile> _files = [];

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string OwnerId { get; private set; } = "";
    public string Name { get; private set; } = "";
    public Guid? ParentId { get; private set; }
    public Folder? Parent { get; private set; }
    public IReadOnlyCollection<Folder> Folders => _folders;
    public IReadOnlyCollection<DriveFile> Files => _files;

    private Folder() { }

    public Folder(Actor owner, string name, Folder? parent = null)
    {
        if (parent is not null) Rules.Allow(parent.CanAccess(owner), "add folders here");
        (OwnerId, Parent, ParentId) = (parent?.OwnerId ?? owner.UserId, parent, parent?.Id);
        Name = Rules.Required(name, "Name");
    }

    public bool CanAccess(Actor? actor) => actor?.Owns(OwnerId) == true;

    public void Rename(Actor actor, string name)
    {
        Rules.Allow(CanAccess(actor), "rename this folder");
        Name = Rules.Required(name, "Name");
    }

    public void CheckCanDelete(Actor actor)
    {
        Rules.Allow(CanAccess(actor), "delete this folder");
        if (_folders.Count > 0 || _files.Count > 0) throw new DomainException("Only empty folders can be deleted.");
    }

    public DriveFile AddFile(Actor actor, string fileName, long size, string storedName, DateTime now)
    {
        Rules.Allow(CanAccess(actor), "upload here");
        var file = new DriveFile(this, fileName, size, storedName, now);
        _files.Add(file);
        return file;
    }

    public DriveFile File(Guid id) => _files.SingleOrDefault(f => f.Id == id) ?? throw new NotFoundException("File", id);

    public void RenameFile(Actor actor, Guid fileId, string name)
    {
        Rules.Allow(CanAccess(actor), "rename this file");
        File(fileId).Rename(name);
    }

    public string RemoveFile(Actor actor, Guid fileId)
    {
        Rules.Allow(CanAccess(actor), "delete this file");
        var file = File(fileId);
        _files.Remove(file);
        return file.StoredName;
    }
}

public class DriveFile : Attachment
{
    public Guid FolderId { get; private set; }
    public Folder Folder { get; private set; } = null!;
    public string Name { get; private set; } = "";
    public long Size { get; private set; }

    private DriveFile() { }

    internal DriveFile(Folder folder, string fileName, long size, string storedName, DateTime now)
        : base(fileName, storedName, now)
    {
        (Folder, FolderId, Size) = (folder, folder.Id, size);
        Rename(Path.GetFileNameWithoutExtension(FileName));
    }

    /// <summary>The display name; the extension stays the uploaded one.</summary>
    internal void Rename(string name) => Name = Rules.Required(name, "Name");

    public string Extension => Path.GetExtension(FileName);
}
