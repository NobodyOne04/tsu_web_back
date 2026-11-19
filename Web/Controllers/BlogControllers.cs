using Backend6.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Forms;

namespace Web.Controllers;

public class PostsController(BlogService blog) : AppController
{
    public async Task<IActionResult> Index() => View(await blog.ListPostsAsync());

    public async Task<IActionResult> Details(Guid id) => View(await blog.GetPostAsync(id));

    [Authorize]
    public Task<IActionResult> Create() => ShowForm(new PostForm(), "New post", Url.Action(nameof(Index))!);

    [HttpPost, ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> Create(PostForm model)
    {
        var form = await ShowForm(model, "New post", Url.Action(nameof(Index))!);
        return await SubmitAsync(async () =>
        {
            var post = await blog.CreatePostAsync(Actor, model.CategoryId, model.Title, model.Text);
            return RedirectToAction(nameof(Details), new { id = post.Id });
        }, () => form);
    }

    [Authorize]
    public async Task<IActionResult> Edit(Guid id)
    {
        var post = await blog.GetPostAsync(id);
        if (!post.CanEdit(Actor)) return Forbid();
        return await ShowForm(new PostForm { CategoryId = post.CategoryId, Title = post.Title, Text = post.Text }, "Edit post", Url.Action(nameof(Details), new { id })!);
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> Edit(Guid id, PostForm model)
    {
        var form = await ShowForm(model, "Edit post", Url.Action(nameof(Details), new { id })!);
        return await SubmitAsync(async () =>
        {
            await blog.EditPostAsync(Actor, id, model.CategoryId, model.Title, model.Text);
            return RedirectToAction(nameof(Details), new { id });
        }, () => form);
    }

    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var post = await blog.GetPostAsync(id);
        return post.CanEdit(Actor) ? ConfirmDelete($"the post \"{post.Title}\" with its comments and attachments", Url.Action(nameof(Details), new { id })!) : Forbid();
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await blog.DeletePostAsync(Actor, id);
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> ShowForm(PostForm model, string heading, string back)
    {
        ViewData["Categories"] = new SelectList(await blog.ListCategoriesAsync(), "Id", "Name", model.CategoryId);
        ViewData["Title"] = heading;
        ViewData["Back"] = back;
        return View("Form", model);
    }
}

[Authorize(Roles = Backend6.Infrastructure.Roles.Administrators)]
public class PostCategoriesController(BlogService blog) : AppController
{
    private string Back => Url.Action(nameof(Index))!;

    public async Task<IActionResult> Index() => View(await blog.ListCategoriesAsync());

    public IActionResult Create() => NameForm(new NameForm(), "New blog category", Back);

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Create(NameForm model) => SubmitAsync(async () =>
    {
        await blog.CreateCategoryAsync(Actor, model.Name);
        return RedirectToAction(nameof(Index));
    }, () => NameForm(model, "New blog category", Back));

    public async Task<IActionResult> Edit(Guid id) =>
        NameForm(new NameForm { Name = (await blog.GetCategoryAsync(id)).Name }, "Rename blog category", Back);

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(Guid id, NameForm model) => SubmitAsync(async () =>
    {
        await blog.RenameCategoryAsync(Actor, id, model.Name);
        return RedirectToAction(nameof(Index));
    }, () => NameForm(model, "Rename blog category", Back));

    public async Task<IActionResult> Delete(Guid id) => ConfirmDelete($"the category \"{(await blog.GetCategoryAsync(id)).Name}\"", Back);

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteConfirmed(Guid id) => SubmitAsync(async () =>
    {
        await blog.DeleteCategoryAsync(Actor, id);
        return RedirectToAction(nameof(Index));
    }, () => ConfirmDelete("this category", Back));
}

[Authorize]
public class PostCommentsController(BlogService blog) : AppController
{
    public IActionResult Create(Guid postId) => TextForm(new TextForm(), "New comment", PostUrl(postId));

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Create(Guid postId, TextForm model) => SubmitAsync(async () =>
    {
        await blog.AddCommentAsync(Actor, postId, model.Text);
        return Redirect(PostUrl(postId));
    }, () => TextForm(model, "New comment", PostUrl(postId)));

    public async Task<IActionResult> Edit(Guid id)
    {
        var comment = await blog.GetCommentAsync(id);
        return comment.CanEdit(Actor) ? TextForm(new TextForm { Text = comment.Text }, "Edit comment", PostUrl(comment.PostId)) : Forbid();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, TextForm model)
    {
        var back = PostUrl((await blog.GetCommentAsync(id)).PostId);
        return await SubmitAsync(async () => Redirect(PostUrl(await blog.EditCommentAsync(Actor, id, model.Text))),
            () => TextForm(model, "Edit comment", back));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var comment = await blog.GetCommentAsync(id);
        return comment.CanEdit(Actor) ? ConfirmDelete("this comment", PostUrl(comment.PostId)) : Forbid();
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id) => Redirect(PostUrl(await blog.DeleteCommentAsync(Actor, id)));

    private string PostUrl(Guid postId) => Url.Action("Details", "Posts", new { id = postId })!;
}

public class PostAttachmentsController(BlogService blog) : AppController
{
    public async Task<IActionResult> Show(Guid id) => Send(await blog.DownloadAsync(id));

    [Authorize]
    public async Task<IActionResult> Create(Guid postId) =>
        (await blog.GetPostAsync(postId)).CanEdit(Actor) ? UploadForm("Attach a file", PostUrl(postId)) : Forbid();

    [HttpPost, ValidateAntiForgeryToken, Authorize, RequestSizeLimit(Backend6.Core.Rules.MaxUploadBytes + 1024 * 1024)]
    public Task<IActionResult> Create(Guid postId, UploadForm model) => SubmitAsync(async () =>
    {
        await blog.AttachAsync(Actor, postId, ToUpload(model.File)!);
        return Redirect(PostUrl(postId));
    }, () => UploadForm("Attach a file", PostUrl(postId)));

    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var attachment = await blog.GetAttachmentAsync(id);
        return attachment.Post.CanEdit(Actor) ? ConfirmDelete($"the attachment \"{attachment.FileName}\"", PostUrl(attachment.PostId)) : Forbid();
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> DeleteConfirmed(Guid id) => Redirect(PostUrl(await blog.DeleteAttachmentAsync(Actor, id)));

    private string PostUrl(Guid postId) => Url.Action("Details", "Posts", new { id = postId })!;
}
