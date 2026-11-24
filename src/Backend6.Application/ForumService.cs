using Backend6.Core;
using Backend6.Core.Forum;

namespace Backend6.Application;

public class ForumService(IForumRepository repo, IFileStore files, IClock clock, IUnitOfWork uow)
{
    // categories and forums: everyone reads, admins manage

    public Task<IReadOnlyList<ForumCategory>> ListCategoriesAsync() => repo.ListCategoriesAsync();

    public async Task<ForumCategory> GetCategoryAsync(Guid id) =>
        await repo.FindCategoryAsync(id) ?? throw new NotFoundException("Forum category", id);

    public async Task CreateCategoryAsync(Actor? actor, string name)
    {
        Rules.Admin(actor, "manage the forum");
        repo.Add(new ForumCategory(name));
        await uow.SaveChangesAsync();
    }

    public async Task RenameCategoryAsync(Actor? actor, Guid id, string name)
    {
        Rules.Admin(actor, "manage the forum");
        (await GetCategoryAsync(id)).Rename(name);
        await uow.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(Actor? actor, Guid id)
    {
        Rules.Admin(actor, "manage the forum");
        var category = await GetCategoryAsync(id);
        if (category.Forums.Count > 0) throw new DomainException("Only categories without forums can be deleted.");
        repo.Remove(category);
        await uow.SaveChangesAsync();
    }

    public async Task<Forum> GetForumAsync(Guid id) => await repo.FindForumAsync(id) ?? throw new NotFoundException("Forum", id);

    public async Task CreateForumAsync(Actor? actor, Guid categoryId, string name, string? description)
    {
        Rules.Admin(actor, "manage the forum");
        repo.Add(new Forum(await CategoryOrFail(categoryId), name, description));
        await uow.SaveChangesAsync();
    }

    public async Task UpdateForumAsync(Actor? actor, Guid id, Guid categoryId, string name, string? description)
    {
        Rules.Admin(actor, "manage the forum");
        (await GetForumAsync(id)).Update(await CategoryOrFail(categoryId), name, description);
        await uow.SaveChangesAsync();
    }

    public async Task DeleteForumAsync(Actor? actor, Guid id)
    {
        Rules.Admin(actor, "manage the forum");
        var forum = await GetForumAsync(id);
        if (await repo.ForumHasTopicsAsync(id)) throw new DomainException("Only forums without topics can be deleted.");
        repo.Remove(forum);
        await uow.SaveChangesAsync();
    }

    // topics and messages: everyone reads, signed-in users write, authors and admins edit

    public Task<IReadOnlyList<ForumTopic>> ListTopicsAsync(Guid forumId) => repo.ListTopicsAsync(forumId);

    public async Task<ForumTopic> GetTopicAsync(Guid id) => await repo.FindTopicAsync(id) ?? throw new NotFoundException("Topic", id);

    /// <summary>A topic starts with its first message.</summary>
    public async Task<ForumTopic> StartTopicAsync(Actor? actor, Guid forumId, string name, string text)
    {
        var author = Rules.SignedIn(actor, "start topics");
        var topic = new ForumTopic(author, await GetForumAsync(forumId), name, clock.UtcNow);
        topic.Reply(author, text, clock.UtcNow);
        repo.Add(topic);
        await uow.SaveChangesAsync();
        return topic;
    }

    public async Task RenameTopicAsync(Actor? actor, Guid id, string name)
    {
        (await GetTopicAsync(id)).Rename(Rules.SignedIn(actor, "rename topics"), name);
        await uow.SaveChangesAsync();
    }

    public async Task<Guid> DeleteTopicAsync(Actor? actor, Guid id)
    {
        var topic = await GetTopicAsync(id);
        Rules.Allow(topic.CanEdit(actor), "delete this topic");
        var stored = topic.StoredFiles.ToList();
        repo.Remove(topic);
        await uow.SaveChangesAsync();
        Uploads.DeleteAll(files, stored);
        return topic.ForumId;
    }

    public async Task ReplyAsync(Actor? actor, Guid topicId, string text)
    {
        (await GetTopicAsync(topicId)).Reply(Rules.SignedIn(actor, "reply"), text, clock.UtcNow);
        await uow.SaveChangesAsync();
    }

    public async Task<ForumMessage> GetMessageAsync(Guid id) => (await TopicOfMessage(id)).Message(id);

    public async Task<Guid> EditMessageAsync(Actor? actor, Guid id, string text)
    {
        var topic = await TopicOfMessage(id);
        topic.EditMessage(Rules.SignedIn(actor, "edit messages"), id, text, clock.UtcNow);
        await uow.SaveChangesAsync();
        return topic.Id;
    }

    public async Task<Guid> DeleteMessageAsync(Actor? actor, Guid id)
    {
        var topic = await TopicOfMessage(id);
        var stored = topic.RemoveMessage(Rules.SignedIn(actor, "delete messages"), id);
        await uow.SaveChangesAsync();
        Uploads.DeleteAll(files, stored);
        return topic.Id;
    }

    public async Task<Guid> AttachAsync(Actor? actor, Guid messageId, Upload upload)
    {
        var topic = await TopicOfMessage(messageId);
        var author = Rules.SignedIn(actor, "attach files");
        Rules.Allow(topic.Message(messageId).CanEdit(author), "attach files to this message");
        var stored = await Uploads.StoreAsync(files, upload);
        await Uploads.CommitOrUndoAsync(uow, files, stored, () => topic.Attach(author, messageId, upload.FileName, stored, clock.UtcNow));
        return topic.Id;
    }

    public async Task<ForumMessageAttachment> GetAttachmentAsync(Guid id) =>
        (await repo.FindTopicByAttachmentAsync(id) ?? throw new NotFoundException("Attachment", id)).Attachment(id);

    public async Task<Guid> DeleteAttachmentAsync(Actor? actor, Guid id)
    {
        var topic = await repo.FindTopicByAttachmentAsync(id) ?? throw new NotFoundException("Attachment", id);
        var stored = topic.RemoveAttachment(Rules.SignedIn(actor, "delete attachments"), id);
        await uow.SaveChangesAsync();
        files.Delete(stored);
        return topic.Id;
    }

    public async Task<Download> DownloadAsync(Guid id)
    {
        var attachment = await GetAttachmentAsync(id);
        return new Download(attachment.FileName, files.OpenRead(attachment.StoredName), attachment.IsImage);
    }

    private async Task<ForumTopic> TopicOfMessage(Guid messageId) =>
        await repo.FindTopicByMessageAsync(messageId) ?? throw new NotFoundException("Message", messageId);

    private async Task<ForumCategory> CategoryOrFail(Guid id) =>
        await repo.FindCategoryAsync(id) ?? throw new DomainException("Choose a category.");
}
