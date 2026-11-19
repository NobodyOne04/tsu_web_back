using Backend6.Core.Blog;
using Backend6.Core.Drive;
using Backend6.Core.Forum;

namespace Backend6.Application;

public interface IBlogRepository
{
    Task<IReadOnlyList<PostCategory>> ListCategoriesAsync();
    Task<PostCategory?> FindCategoryAsync(Guid id);
    Task<bool> CategoryHasPostsAsync(Guid id);
    void Add(PostCategory category);
    void Remove(PostCategory category);

    /// <summary>Newest first, with category.</summary>
    Task<IReadOnlyList<Post>> ListPostsAsync();
    /// <summary>With category, comments and attachments.</summary>
    Task<Post?> FindPostAsync(Guid id);
    Task<Post?> FindPostByCommentAsync(Guid commentId);
    Task<Post?> FindPostByAttachmentAsync(Guid attachmentId);
    void Add(Post post);
    void Remove(Post post);
}

public interface IForumRepository
{
    /// <summary>With their forums.</summary>
    Task<IReadOnlyList<ForumCategory>> ListCategoriesAsync();
    Task<ForumCategory?> FindCategoryAsync(Guid id);
    void Add(ForumCategory category);
    void Remove(ForumCategory category);

    Task<Forum?> FindForumAsync(Guid id);
    Task<bool> ForumHasTopicsAsync(Guid id);
    void Add(Forum forum);
    void Remove(Forum forum);

    /// <summary>Newest first, with message counts loaded.</summary>
    Task<IReadOnlyList<ForumTopic>> ListTopicsAsync(Guid forumId);
    /// <summary>With forum, messages and attachments.</summary>
    Task<ForumTopic?> FindTopicAsync(Guid id);
    Task<ForumTopic?> FindTopicByMessageAsync(Guid messageId);
    Task<ForumTopic?> FindTopicByAttachmentAsync(Guid attachmentId);
    void Add(ForumTopic topic);
    void Remove(ForumTopic topic);
}

public interface IDriveRepository
{
    /// <summary>Top-level folders of one owner, or of everyone when ownerId is null.</summary>
    Task<IReadOnlyList<Folder>> ListRootFoldersAsync(string? ownerId);
    /// <summary>With parent, subfolders and files.</summary>
    Task<Folder?> FindFolderAsync(Guid id);
    Task<Folder?> FindFolderByFileAsync(Guid fileId);
    void Add(Folder folder);
    void Remove(Folder folder);
}

/// <summary>Uploaded bytes, kept outside the web root and addressed by an opaque stored name.</summary>
public interface IFileStore
{
    Task<string> SaveAsync(Stream content);
    Stream OpenRead(string storedName);
    void Delete(string storedName);
}

public interface IClock
{
    DateTime UtcNow { get; }
}

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}

/// <summary>An upload as the web layer receives it.</summary>
public sealed record Upload(string FileName, long Length, Func<Stream> Open);

/// <summary>A file to send back: its name and an open stream.</summary>
public sealed record Download(string FileName, Stream Content, bool IsImage);
