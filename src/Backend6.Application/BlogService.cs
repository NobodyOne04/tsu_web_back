using Backend6.Core;
using Backend6.Core.Blog;

namespace Backend6.Application;

public class BlogService(IBlogRepository repo, IFileStore files, IClock clock, IUnitOfWork uow)
{
    // categories: admins only

    public Task<IReadOnlyList<PostCategory>> ListCategoriesAsync() => repo.ListCategoriesAsync();

    public async Task<PostCategory> GetCategoryAsync(Guid id) =>
        await repo.FindCategoryAsync(id) ?? throw new NotFoundException("Category", id);

    public async Task CreateCategoryAsync(Actor? actor, string name)
    {
        Rules.Admin(actor, "manage blog categories");
        repo.Add(new PostCategory(name));
        await uow.SaveChangesAsync();
    }

    public async Task RenameCategoryAsync(Actor? actor, Guid id, string name)
    {
        Rules.Admin(actor, "manage blog categories");
        (await GetCategoryAsync(id)).Rename(name);
        await uow.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(Actor? actor, Guid id)
    {
        Rules.Admin(actor, "manage blog categories");
        var category = await GetCategoryAsync(id);
        if (await repo.CategoryHasPostsAsync(id)) throw new DomainException("Only categories without posts can be deleted.");
        repo.Remove(category);
        await uow.SaveChangesAsync();
    }

    // posts: anyone reads; signed-in users write; authors and admins edit

    public Task<IReadOnlyList<Post>> ListPostsAsync() => repo.ListPostsAsync();

    public async Task<Post> GetPostAsync(Guid id) => await repo.FindPostAsync(id) ?? throw new NotFoundException("Post", id);

    public async Task<Post> CreatePostAsync(Actor? actor, Guid categoryId, string title, string text)
    {
        var author = Rules.SignedIn(actor, "write posts");
        var post = new Post(author, await CategoryOrFail(categoryId), title, text, clock.UtcNow);
        repo.Add(post);
        await uow.SaveChangesAsync();
        return post;
    }

    public async Task EditPostAsync(Actor? actor, Guid id, Guid categoryId, string title, string text)
    {
        var post = await GetPostAsync(id);
        post.Edit(Rules.SignedIn(actor, "edit posts"), await CategoryOrFail(categoryId), title, text, clock.UtcNow);
        await uow.SaveChangesAsync();
    }

    public async Task DeletePostAsync(Actor? actor, Guid id)
    {
        var post = await GetPostAsync(id);
        Rules.Allow(post.CanEdit(actor), "delete this post");
        var stored = post.Attachments.Select(a => a.StoredName).ToList();
        repo.Remove(post);
        await uow.SaveChangesAsync();
        Uploads.DeleteAll(files, stored);
    }

    public async Task<PostComment> GetCommentAsync(Guid id) =>
        (await repo.FindPostByCommentAsync(id) ?? throw new NotFoundException("Comment", id)).Comment(id);

    public async Task AddCommentAsync(Actor? actor, Guid postId, string text)
    {
        (await GetPostAsync(postId)).AddComment(Rules.SignedIn(actor, "comment"), text, clock.UtcNow);
        await uow.SaveChangesAsync();
    }

    public async Task<Guid> EditCommentAsync(Actor? actor, Guid id, string text)
    {
        var post = await repo.FindPostByCommentAsync(id) ?? throw new NotFoundException("Comment", id);
        post.EditComment(Rules.SignedIn(actor, "edit comments"), id, text, clock.UtcNow);
        await uow.SaveChangesAsync();
        return post.Id;
    }

    public async Task<Guid> DeleteCommentAsync(Actor? actor, Guid id)
    {
        var post = await repo.FindPostByCommentAsync(id) ?? throw new NotFoundException("Comment", id);
        post.RemoveComment(Rules.SignedIn(actor, "delete comments"), id);
        await uow.SaveChangesAsync();
        return post.Id;
    }

    public async Task AttachAsync(Actor? actor, Guid postId, Upload upload)
    {
        var post = await GetPostAsync(postId);
        var editor = Rules.SignedIn(actor, "attach files");
        Rules.Allow(post.CanEdit(editor), "attach files to this post");
        var stored = await Uploads.StoreAsync(files, upload);
        await Uploads.CommitOrUndoAsync(uow, files, stored, () => post.AddAttachment(editor, upload.FileName, stored, clock.UtcNow));
    }

    public async Task<PostAttachment> GetAttachmentAsync(Guid id) =>
        (await repo.FindPostByAttachmentAsync(id) ?? throw new NotFoundException("Attachment", id)).Attachment(id);

    public async Task<Guid> DeleteAttachmentAsync(Actor? actor, Guid id)
    {
        var post = await repo.FindPostByAttachmentAsync(id) ?? throw new NotFoundException("Attachment", id);
        var stored = post.RemoveAttachment(Rules.SignedIn(actor, "delete attachments"), id);
        await uow.SaveChangesAsync();
        files.Delete(stored);
        return post.Id;
    }

    /// <summary>Post attachments are public, like the posts.</summary>
    public async Task<Download> DownloadAsync(Guid id)
    {
        var attachment = await GetAttachmentAsync(id);
        return new Download(attachment.FileName, files.OpenRead(attachment.StoredName), attachment.IsImage);
    }

    private async Task<PostCategory> CategoryOrFail(Guid id) =>
        await repo.FindCategoryAsync(id) ?? throw new DomainException("Choose a category.");
}
