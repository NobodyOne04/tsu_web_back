using Backend6.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Forms;

namespace Web.Controllers;

/// <summary>Private folders. Someone else's folder answers 404, as if it didn't exist.</summary>
[Authorize]
public class FoldersController(DriveService drive) : AppController
{
    public async Task<IActionResult> Index() => View(await drive.ListRootFoldersAsync(Actor));

    public async Task<IActionResult> Details(Guid id) => View(await drive.GetFolderAsync(Actor, id));

    public IActionResult Create(Guid? parentId) => NameForm(new NameForm(), "New folder", Where(parentId));

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Create(Guid? parentId, NameForm model) => SubmitAsync(async () =>
    {
        var folder = await drive.CreateFolderAsync(Actor, model.Name, parentId);
        return RedirectToAction(nameof(Details), new { id = folder.Id });
    }, () => NameForm(model, "New folder", Where(parentId)));

    public async Task<IActionResult> Edit(Guid id) =>
        NameForm(new NameForm { Name = (await drive.GetFolderAsync(Actor, id)).Name }, "Rename folder", Where(id));

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(Guid id, NameForm model) => SubmitAsync(async () =>
    {
        await drive.RenameFolderAsync(Actor, id, model.Name);
        return Redirect(Where(id));
    }, () => NameForm(model, "Rename folder", Where(id)));

    public async Task<IActionResult> Delete(Guid id) => ConfirmDelete($"the folder \"{(await drive.GetFolderAsync(Actor, id)).Name}\"", Where(id));

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteConfirmed(Guid id) => SubmitAsync(async () =>
        Redirect(Where(await drive.DeleteFolderAsync(Actor, id))), () => ConfirmDelete("this folder", Where(id)));

    private string Where(Guid? folderId) =>
        folderId is { } id ? Url.Action(nameof(Details), new { id })! : Url.Action(nameof(Index))!;
}

[Authorize]
public class FilesController(DriveService drive) : AppController
{
    public async Task<IActionResult> Download(Guid id) => Send(await drive.DownloadAsync(Actor, id));

    public async Task<IActionResult> Create(Guid folderId)
    {
        await drive.GetFolderAsync(Actor, folderId);  // 404 unless it's yours
        return UploadForm("Upload a file", FolderUrl(folderId));
    }

    [HttpPost, ValidateAntiForgeryToken, RequestSizeLimit(Backend6.Core.Rules.MaxUploadBytes + 1024 * 1024)]
    public Task<IActionResult> Create(Guid folderId, UploadForm model) => SubmitAsync(async () =>
    {
        await drive.UploadAsync(Actor, folderId, ToUpload(model.File)!);
        return Redirect(FolderUrl(folderId));
    }, () => UploadForm("Upload a file", FolderUrl(folderId)));

    public async Task<IActionResult> Edit(Guid id)
    {
        var file = await drive.GetFileAsync(Actor, id);
        return NameForm(new NameForm { Name = file.Name }, $"Rename {file.Name}{file.Extension}", FolderUrl(file.FolderId));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, NameForm model)
    {
        var back = FolderUrl((await drive.GetFileAsync(Actor, id)).FolderId);
        return await SubmitAsync(async () => Redirect(FolderUrl(await drive.RenameFileAsync(Actor, id, model.Name))),
            () => NameForm(model, "Rename file", back));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var file = await drive.GetFileAsync(Actor, id);
        return ConfirmDelete($"the file \"{file.Name}{file.Extension}\"", FolderUrl(file.FolderId));
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id) => Redirect(FolderUrl(await drive.DeleteFileAsync(Actor, id)));

    private string FolderUrl(Guid folderId) => Url.Action("Details", "Folders", new { id = folderId })!;
}
