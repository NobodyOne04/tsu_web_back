using Backend6.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Forms;

namespace Web.Controllers;

/// <summary>The forum home: categories with their forums.</summary>
public class ForumController(ForumService forum) : AppController
{
    public async Task<IActionResult> Index() => View(await forum.ListCategoriesAsync());
}

[Authorize(Roles = Backend6.Infrastructure.Roles.Administrators)]
public class ForumCategoriesController(ForumService forum) : AppController
{
    private string Back => Url.Action("Index", "Forum")!;

    public IActionResult Create() => NameForm(new NameForm(), "New forum category", Back);

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Create(NameForm model) => SubmitAsync(async () =>
    {
        await forum.CreateCategoryAsync(Actor, model.Name);
        return Redirect(Back);
    }, () => NameForm(model, "New forum category", Back));

    public async Task<IActionResult> Edit(Guid id) =>
        NameForm(new NameForm { Name = (await forum.GetCategoryAsync(id)).Name }, "Rename forum category", Back);

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(Guid id, NameForm model) => SubmitAsync(async () =>
    {
        await forum.RenameCategoryAsync(Actor, id, model.Name);
        return Redirect(Back);
    }, () => NameForm(model, "Rename forum category", Back));

    public async Task<IActionResult> Delete(Guid id) => ConfirmDelete($"the category \"{(await forum.GetCategoryAsync(id)).Name}\"", Back);

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteConfirmed(Guid id) => SubmitAsync(async () =>
    {
        await forum.DeleteCategoryAsync(Actor, id);
        return Redirect(Back);
    }, () => ConfirmDelete("this category", Back));
}

[Authorize(Roles = Backend6.Infrastructure.Roles.Administrators)]
public class ForumsController(ForumService forum) : AppController
{
    private string Back => Url.Action("Index", "Forum")!;

    public Task<IActionResult> Create(Guid? categoryId) => ShowForm(new ForumForm { CategoryId = categoryId ?? Guid.Empty }, "New forum");

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ForumForm model)
    {
        var form = await ShowForm(model, "New forum");
        return await SubmitAsync(async () =>
        {
            await forum.CreateForumAsync(Actor, model.CategoryId, model.Name, model.Description);
            return Redirect(Back);
        }, () => form);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var f = await forum.GetForumAsync(id);
        return await ShowForm(new ForumForm { CategoryId = f.CategoryId, Name = f.Name, Description = f.Description }, "Edit forum");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ForumForm model)
    {
        var form = await ShowForm(model, "Edit forum");
        return await SubmitAsync(async () =>
        {
            await forum.UpdateForumAsync(Actor, id, model.CategoryId, model.Name, model.Description);
            return Redirect(Back);
        }, () => form);
    }

    public async Task<IActionResult> Delete(Guid id) => ConfirmDelete($"the forum \"{(await forum.GetForumAsync(id)).Name}\"", Back);

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteConfirmed(Guid id) => SubmitAsync(async () =>
    {
        await forum.DeleteForumAsync(Actor, id);
        return Redirect(Back);
    }, () => ConfirmDelete("this forum", Back));

    private async Task<IActionResult> ShowForm(ForumForm model, string heading)
    {
        ViewData["Categories"] = new SelectList(await forum.ListCategoriesAsync(), "Id", "Name", model.CategoryId);
        ViewData["Title"] = heading;
        ViewData["Back"] = Back;
        return View("Form", model);
    }
}

/// <summary>A forum's topics; starting, renaming and deleting topics.</summary>
public class ForumTopicsController(ForumService forum) : AppController
{
    public async Task<IActionResult> Index(Guid forumId)
    {
        ViewData["Forum"] = await forum.GetForumAsync(forumId);
        return View(await forum.ListTopicsAsync(forumId));
    }

    [Authorize]
    public async Task<IActionResult> Create(Guid forumId)
    {
        ViewData["Forum"] = await forum.GetForumAsync(forumId);
        return View(new TopicForm());
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> Create(Guid forumId, TopicForm model)
    {
        ViewData["Forum"] = await forum.GetForumAsync(forumId);
        return await SubmitAsync(async () =>
        {
            var topic = await forum.StartTopicAsync(Actor, forumId, model.Name, model.Text);
            return RedirectToAction("Index", "ForumMessages", new { topicId = topic.Id });
        }, () => View(model));
    }

    [Authorize]
    public async Task<IActionResult> Edit(Guid id)
    {
        var topic = await forum.GetTopicAsync(id);
        return topic.CanEdit(Actor) ? NameForm(new NameForm { Name = topic.Name }, "Rename topic", TopicUrl(id)) : Forbid();
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize]
    public Task<IActionResult> Edit(Guid id, NameForm model) => SubmitAsync(async () =>
    {
        await forum.RenameTopicAsync(Actor, id, model.Name);
        return Redirect(TopicUrl(id));
    }, () => NameForm(model, "Rename topic", TopicUrl(id)));

    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var topic = await forum.GetTopicAsync(id);
        return topic.CanEdit(Actor) ? ConfirmDelete($"the topic \"{topic.Name}\" with all its messages", TopicUrl(id)) : Forbid();
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> DeleteConfirmed(Guid id) =>
        RedirectToAction(nameof(Index), new { forumId = await forum.DeleteTopicAsync(Actor, id) });

    private string TopicUrl(Guid id) => Url.Action("Index", "ForumMessages", new { topicId = id })!;
}

/// <summary>A topic's messages; replying, editing, deleting and attaching files.</summary>
public class ForumMessagesController(ForumService forum) : AppController
{
    public async Task<IActionResult> Index(Guid topicId) => View(await forum.GetTopicAsync(topicId));

    [Authorize]
    public IActionResult Create(Guid topicId) => TextForm(new TextForm(), "Reply", TopicUrl(topicId));

    [HttpPost, ValidateAntiForgeryToken, Authorize]
    public Task<IActionResult> Create(Guid topicId, TextForm model) => SubmitAsync(async () =>
    {
        await forum.ReplyAsync(Actor, topicId, model.Text);
        return Redirect(TopicUrl(topicId));
    }, () => TextForm(model, "Reply", TopicUrl(topicId)));

    [Authorize]
    public async Task<IActionResult> Edit(Guid id)
    {
        var message = await forum.GetMessageAsync(id);
        return message.CanEdit(Actor) ? TextForm(new TextForm { Text = message.Text }, "Edit message", TopicUrl(message.TopicId)) : Forbid();
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> Edit(Guid id, TextForm model)
    {
        var back = TopicUrl((await forum.GetMessageAsync(id)).TopicId);
        return await SubmitAsync(async () => Redirect(TopicUrl(await forum.EditMessageAsync(Actor, id, model.Text))),
            () => TextForm(model, "Edit message", back));
    }

    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var message = await forum.GetMessageAsync(id);
        return message.CanEdit(Actor) ? ConfirmDelete("this message and its attachments", TopicUrl(message.TopicId)) : Forbid();
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> DeleteConfirmed(Guid id) => Redirect(TopicUrl(await forum.DeleteMessageAsync(Actor, id)));

    [Authorize]
    public async Task<IActionResult> Attach(Guid id)
    {
        var message = await forum.GetMessageAsync(id);
        return message.CanEdit(Actor) ? UploadForm("Attach a file", TopicUrl(message.TopicId)) : Forbid();
    }

    [HttpPost, ValidateAntiForgeryToken, Authorize, RequestSizeLimit(Backend6.Core.Rules.MaxUploadBytes + 1024 * 1024)]
    public async Task<IActionResult> Attach(Guid id, UploadForm model)
    {
        var back = TopicUrl((await forum.GetMessageAsync(id)).TopicId);
        return await SubmitAsync(async () => Redirect(TopicUrl(await forum.AttachAsync(Actor, id, ToUpload(model.File)!))),
            () => UploadForm("Attach a file", back));
    }

    public async Task<IActionResult> Attachment(Guid id) => Send(await forum.DownloadAsync(id));

    [Authorize]
    public async Task<IActionResult> DeleteAttachment(Guid id)
    {
        var attachment = await forum.GetAttachmentAsync(id);
        return attachment.Message.CanEdit(Actor)
            ? ConfirmDelete($"the attachment \"{attachment.FileName}\"", TopicUrl(attachment.Message.TopicId))
            : Forbid();
    }

    [HttpPost, ActionName("DeleteAttachment"), ValidateAntiForgeryToken, Authorize]
    public async Task<IActionResult> DeleteAttachmentConfirmed(Guid id) => Redirect(TopicUrl(await forum.DeleteAttachmentAsync(Actor, id)));

    private string TopicUrl(Guid topicId) => Url.Action(nameof(Index), new { topicId })!;
}
