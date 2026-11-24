namespace Backend6.Core.Blog;

public class PostCategory
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = "";

    private PostCategory() { }

    public PostCategory(string name) => Rename(name);

    public void Rename(string name) => Name = Rules.Required(name, "Name");
}

/// <summary>A blog post with its comments and attachments.</summary>
public class Post
{
    private readonly List<PostComment> _comments = [];
    private readonly List<PostAttachment> _attachments = [];

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid CategoryId { get; private set; }
    public PostCategory Category { get; private set; } = null!;
    public string AuthorId { get; private set; } = "";
    public string AuthorName { get; private set; } = "";
    public string Title { get; private set; } = "";
    public string Text { get; private set; } = "";
    public DateTime Created { get; private set; }
    public DateTime Modified { get; private set; }
    public IReadOnlyCollection<PostComment> Comments => _comments;
    public IReadOnlyCollection<PostAttachment> Attachments => _attachments;

    private Post() { }

    public Post(Actor author, PostCategory category, string title, string text, DateTime now)
    {
        (AuthorId, AuthorName, Created) = (author.UserId, author.UserName, now);
        Set(category, title, text, now);
    }

    public bool CanEdit(Actor? actor) => actor?.Owns(AuthorId) == true;

    public void Edit(Actor actor, PostCategory category, string title, string text, DateTime now)
    {
        Rules.Allow(CanEdit(actor), "edit this post");
        Set(category, title, text, now);
    }

    private void Set(PostCategory category, string title, string text, DateTime now)
    {
        (Category, CategoryId) = (category, category.Id);
        Title = Rules.Required(title, "Title");
        Text = Rules.Required(text, "Text", int.MaxValue);
        Modified = now;
    }

    public PostComment AddComment(Actor author, string text, DateTime now)
    {
        var comment = new PostComment(this, author, text, now);
        _comments.Add(comment);
        return comment;
    }

    public PostComment Comment(Guid id) =>
        _comments.SingleOrDefault(c => c.Id == id) ?? throw new NotFoundException("Comment", id);

    public void EditComment(Actor actor, Guid commentId, string text, DateTime now)
    {
        var comment = Comment(commentId);
        Rules.Allow(comment.CanEdit(actor), "edit this comment");
        comment.SetText(text, now);
    }

    public void RemoveComment(Actor actor, Guid commentId)
    {
        var comment = Comment(commentId);
        Rules.Allow(comment.CanEdit(actor), "delete this comment");
        _comments.Remove(comment);
    }

    public PostAttachment AddAttachment(Actor actor, string fileName, string storedName, DateTime now)
    {
        Rules.Allow(CanEdit(actor), "attach files to this post");
        var attachment = new PostAttachment(this, fileName, storedName, now);
        _attachments.Add(attachment);
        return attachment;
    }

    public PostAttachment Attachment(Guid id) =>
        _attachments.SingleOrDefault(a => a.Id == id) ?? throw new NotFoundException("Attachment", id);

    public string RemoveAttachment(Actor actor, Guid attachmentId)
    {
        Rules.Allow(CanEdit(actor), "delete attachments of this post");
        var attachment = Attachment(attachmentId);
        _attachments.Remove(attachment);
        return attachment.StoredName;
    }
}

public class PostComment
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid PostId { get; private set; }
    public Post Post { get; private set; } = null!;
    public string AuthorId { get; private set; } = "";
    public string AuthorName { get; private set; } = "";
    public string Text { get; private set; } = "";
    public DateTime Created { get; private set; }
    public DateTime Modified { get; private set; }

    private PostComment() { }

    internal PostComment(Post post, Actor author, string text, DateTime now)
    {
        (Post, PostId, AuthorId, AuthorName, Created) = (post, post.Id, author.UserId, author.UserName, now);
        SetText(text, now);
    }

    public bool CanEdit(Actor? actor) => actor?.Owns(AuthorId) == true;

    internal void SetText(string text, DateTime now) => (Text, Modified) = (Rules.Required(text, "Comment", 10_000), now);
}

public class PostAttachment : Attachment
{
    public Guid PostId { get; private set; }
    public Post Post { get; private set; } = null!;

    private PostAttachment() { }

    internal PostAttachment(Post post, string fileName, string storedName, DateTime now)
        : base(fileName, storedName, now) => (Post, PostId) = (post, post.Id);
}
