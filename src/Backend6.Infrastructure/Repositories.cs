using Backend6.Application;
using Backend6.Core.Blog;
using Backend6.Core.Drive;
using Backend6.Core.Forum;
using Microsoft.EntityFrameworkCore;

namespace Backend6.Infrastructure;

internal class BlogRepository(AppDbContext db) : IBlogRepository
{
    private IQueryable<Post> Full => db.Posts
        .Include(p => p.Category).Include(p => p.Comments).Include(p => p.Attachments).AsSplitQuery();

    public async Task<IReadOnlyList<PostCategory>> ListCategoriesAsync() => await db.PostCategories.OrderBy(c => c.Name).ToListAsync();
    public Task<PostCategory?> FindCategoryAsync(Guid id) => db.PostCategories.SingleOrDefaultAsync(c => c.Id == id);
    public Task<bool> CategoryHasPostsAsync(Guid id) => db.Posts.AnyAsync(p => p.CategoryId == id);
    public void Add(PostCategory category) => db.Add(category);
    public void Remove(PostCategory category) => db.Remove(category);

    public async Task<IReadOnlyList<Post>> ListPostsAsync() =>
        await db.Posts.Include(p => p.Category).OrderByDescending(p => p.Created).ToListAsync();
    public Task<Post?> FindPostAsync(Guid id) => Full.SingleOrDefaultAsync(p => p.Id == id);
    public Task<Post?> FindPostByCommentAsync(Guid commentId) => Full.SingleOrDefaultAsync(p => p.Comments.Any(c => c.Id == commentId));
    public Task<Post?> FindPostByAttachmentAsync(Guid attachmentId) => Full.SingleOrDefaultAsync(p => p.Attachments.Any(a => a.Id == attachmentId));
    public void Add(Post post) => db.Add(post);
    public void Remove(Post post) => db.Remove(post);
}

internal class ForumRepository(AppDbContext db) : IForumRepository
{
    private IQueryable<ForumTopic> Full => db.ForumTopics
        .Include(t => t.Forum).Include(t => t.Messages).ThenInclude(m => m.Attachments).AsSplitQuery();

    public async Task<IReadOnlyList<ForumCategory>> ListCategoriesAsync() =>
        await db.ForumCategories.Include(c => c.Forums).OrderBy(c => c.Name).ToListAsync();
    public Task<ForumCategory?> FindCategoryAsync(Guid id) => db.ForumCategories.Include(c => c.Forums).SingleOrDefaultAsync(c => c.Id == id);
    public void Add(ForumCategory category) => db.Add(category);
    public void Remove(ForumCategory category) => db.Remove(category);

    public Task<Forum?> FindForumAsync(Guid id) => db.Forums.Include(f => f.Category).SingleOrDefaultAsync(f => f.Id == id);
    public Task<bool> ForumHasTopicsAsync(Guid id) => db.ForumTopics.AnyAsync(t => t.ForumId == id);
    public void Add(Forum forum) => db.Add(forum);
    public void Remove(Forum forum) => db.Remove(forum);

    public async Task<IReadOnlyList<ForumTopic>> ListTopicsAsync(Guid forumId) =>
        await db.ForumTopics.Include(t => t.Messages).Where(t => t.ForumId == forumId).OrderByDescending(t => t.Created).AsSplitQuery().ToListAsync();
    public Task<ForumTopic?> FindTopicAsync(Guid id) => Full.SingleOrDefaultAsync(t => t.Id == id);
    public Task<ForumTopic?> FindTopicByMessageAsync(Guid messageId) => Full.SingleOrDefaultAsync(t => t.Messages.Any(m => m.Id == messageId));
    public Task<ForumTopic?> FindTopicByAttachmentAsync(Guid attachmentId) =>
        Full.SingleOrDefaultAsync(t => t.Messages.Any(m => m.Attachments.Any(a => a.Id == attachmentId)));
    public void Add(ForumTopic topic) => db.Add(topic);
    public void Remove(ForumTopic topic) => db.Remove(topic);
}

internal class DriveRepository(AppDbContext db) : IDriveRepository
{
    private IQueryable<Folder> Full => db.Folders
        .Include(f => f.Parent).Include(f => f.Folders).Include(f => f.Files).AsSplitQuery();

    public async Task<IReadOnlyList<Folder>> ListRootFoldersAsync(string? ownerId) =>
        await db.Folders.Where(f => f.ParentId == null && (ownerId == null || f.OwnerId == ownerId)).OrderBy(f => f.Name).ToListAsync();
    public Task<Folder?> FindFolderAsync(Guid id) => Full.SingleOrDefaultAsync(f => f.Id == id);
    public Task<Folder?> FindFolderByFileAsync(Guid fileId) => Full.SingleOrDefaultAsync(f => f.Files.Any(x => x.Id == fileId));
    public void Add(Folder folder) => db.Add(folder);
    public void Remove(Folder folder) => db.Remove(folder);
}
