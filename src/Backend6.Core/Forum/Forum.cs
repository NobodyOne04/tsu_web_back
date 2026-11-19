namespace Backend6.Core.Forum;

/// <summary>A group of forums. Managed by admins.</summary>
public class ForumCategory
{
    private readonly List<Forum> _forums = [];

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = "";
    public IReadOnlyCollection<Forum> Forums => _forums;

    private ForumCategory() { }

    public ForumCategory(string name) => Rename(name);

    public void Rename(string name) => Name = Rules.Required(name, "Name");
}

public class Forum
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid CategoryId { get; private set; }
    public ForumCategory Category { get; private set; } = null!;
    public string Name { get; private set; } = "";
    public string? Description { get; private set; }

    private Forum() { }

    public Forum(ForumCategory category, string name, string? description) => Update(category, name, description);

    public void Update(ForumCategory category, string name, string? description)
    {
        (Category, CategoryId) = (category, category.Id);
        Name = Rules.Required(name, "Name");
        Description = Rules.Optional(description);
    }
}

/// <summary>A discussion in a forum, with its messages.</summary>
public class ForumTopic
{
    private readonly List<ForumMessage> _messages = [];

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ForumId { get; private set; }
    public Forum Forum { get; private set; } = null!;
    public string AuthorId { get; private set; } = "";
    public string AuthorName { get; private set; } = "";
    public string Name { get; private set; } = "";
    public DateTime Created { get; private set; }
    public IReadOnlyCollection<ForumMessage> Messages => _messages;

    private ForumTopic() { }

    public ForumTopic(Actor author, Forum forum, string name, DateTime now)
    {
        (Forum, ForumId, AuthorId, AuthorName, Created) = (forum, forum.Id, author.UserId, author.UserName, now);
        Name = Rules.Required(name, "Name");
    }

    public bool CanEdit(Actor? actor) => actor?.Owns(AuthorId) == true;

    public void Rename(Actor actor, string name)
    {
        Rules.Allow(CanEdit(actor), "rename this topic");
        Name = Rules.Required(name, "Name");
    }

    /// <summary>Stored names of every attachment in the topic, for deleting the files with it.</summary>
    public IEnumerable<string> StoredFiles => _messages.SelectMany(m => m.Attachments).Select(a => a.StoredName);

    public ForumMessage Reply(Actor author, string text, DateTime now)
    {
        var message = new ForumMessage(this, author, text, now);
        _messages.Add(message);
        return message;
    }

    public ForumMessage Message(Guid id) =>
        _messages.SingleOrDefault(m => m.Id == id) ?? throw new NotFoundException("Message", id);

    public void EditMessage(Actor actor, Guid messageId, string text, DateTime now)
    {
        var message = Message(messageId);
        Rules.Allow(message.CanEdit(actor), "edit this message");
        message.SetText(text, now);
    }

    public IReadOnlyList<string> RemoveMessage(Actor actor, Guid messageId)
    {
        var message = Message(messageId);
        Rules.Allow(message.CanEdit(actor), "delete this message");
        _messages.Remove(message);
        return message.Attachments.Select(a => a.StoredName).ToList();
    }

    public ForumMessageAttachment Attach(Actor actor, Guid messageId, string fileName, string storedName, DateTime now)
    {
        var message = Message(messageId);
        Rules.Allow(message.CanEdit(actor), "attach files to this message");
        return message.Attach(fileName, storedName, now);
    }

    public ForumMessageAttachment Attachment(Guid id) =>
        _messages.SelectMany(m => m.Attachments).SingleOrDefault(a => a.Id == id) ?? throw new NotFoundException("Attachment", id);

    public string RemoveAttachment(Actor actor, Guid attachmentId)
    {
        var attachment = Attachment(attachmentId);
        Rules.Allow(attachment.Message.CanEdit(actor), "delete this attachment");
        attachment.Message.Detach(attachment);
        return attachment.StoredName;
    }
}

public class ForumMessage
{
    private readonly List<ForumMessageAttachment> _attachments = [];

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid TopicId { get; private set; }
    public ForumTopic Topic { get; private set; } = null!;
    public string AuthorId { get; private set; } = "";
    public string AuthorName { get; private set; } = "";
    public string Text { get; private set; } = "";
    public DateTime Created { get; private set; }
    public DateTime Modified { get; private set; }
    public IReadOnlyCollection<ForumMessageAttachment> Attachments => _attachments;

    private ForumMessage() { }

    internal ForumMessage(ForumTopic topic, Actor author, string text, DateTime now)
    {
        (Topic, TopicId, AuthorId, AuthorName, Created) = (topic, topic.Id, author.UserId, author.UserName, now);
        SetText(text, now);
    }

    public bool CanEdit(Actor? actor) => actor?.Owns(AuthorId) == true;

    internal void SetText(string text, DateTime now) => (Text, Modified) = (Rules.Required(text, "Message", 10_000), now);

    internal ForumMessageAttachment Attach(string fileName, string storedName, DateTime now)
    {
        var attachment = new ForumMessageAttachment(this, fileName, storedName, now);
        _attachments.Add(attachment);
        return attachment;
    }

    internal void Detach(ForumMessageAttachment attachment) => _attachments.Remove(attachment);
}

public class ForumMessageAttachment : Attachment
{
    public Guid MessageId { get; private set; }
    public ForumMessage Message { get; private set; } = null!;

    private ForumMessageAttachment() { }

    internal ForumMessageAttachment(ForumMessage message, string fileName, string storedName, DateTime now)
        : base(fileName, storedName, now) => (Message, MessageId) = (message, message.Id);
}
