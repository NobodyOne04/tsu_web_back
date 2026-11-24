using Backend6.Core;
using Backend6.Core.Blog;
using Backend6.Core.Drive;
using Backend6.Core.Forum;

namespace Backend6.Tests;

public class DomainTests
{
    private static readonly DateTime Now = new(2026, 11, 20, 12, 0, 0, DateTimeKind.Utc);
    private static readonly Actor Ann = new("ann", "ann@x", false);
    private static readonly Actor Bob = new("bob", "bob@x", false);
    private static readonly Actor Admin = new("root", "root@x", true);

    [Fact]
    public void Authors_and_admins_edit_posts_others_cannot()
    {
        var post = new Post(Ann, new PostCategory("News"), "Hi", "Text", Now);
        Assert.True(post.CanEdit(Ann));
        Assert.True(post.CanEdit(Admin));
        Assert.False(post.CanEdit(Bob));
        Assert.False(post.CanEdit(null));
        Assert.Throws<ForbiddenException>(() => post.Edit(Bob, post.Category, "Hacked", "x", Now));
        Assert.Throws<ForbiddenException>(() => post.AddAttachment(Bob, "a.png", "f", Now));
    }

    [Fact]
    public void Anyone_signed_in_comments_only_the_commenter_edits()
    {
        var post = new Post(Ann, new PostCategory("News"), "Hi", "Text", Now);
        var comment = post.AddComment(Bob, " nice ", Now);
        Assert.Equal("nice", comment.Text);
        Assert.Throws<ForbiddenException>(() => post.EditComment(Ann, comment.Id, "changed", Now));
        post.RemoveComment(Admin, comment.Id);
        Assert.Empty(post.Comments);
    }

    [Fact]
    public void A_topic_collects_its_attachment_files()
    {
        var topic = new ForumTopic(Ann, new Forum(new ForumCategory("General"), "Chat", null), "Hello", Now);
        var message = topic.Reply(Ann, "first", Now);
        topic.Attach(Ann, message.Id, "../../etc/passwd", "stored1", Now);
        Assert.Equal("passwd", message.Attachments.Single().FileName);  // path parts dropped
        Assert.Equal(new[] { "stored1" }, topic.StoredFiles);
        Assert.Throws<ForbiddenException>(() => topic.EditMessage(Bob, message.Id, "x", Now));
        Assert.Equal(new[] { "stored1" }, topic.RemoveMessage(Admin, message.Id));
    }

    [Fact]
    public void Folders_are_private_and_only_empty_ones_are_deleted()
    {
        var root = new Folder(Ann, "Docs");
        Assert.False(root.CanAccess(Bob));
        Assert.True(root.CanAccess(Admin));
        Assert.Throws<ForbiddenException>(() => new Folder(Bob, "Sneaky", root));
        var child = new Folder(Admin, "Shared", root);
        Assert.Equal("ann", child.OwnerId);  // a subfolder belongs to the parent's owner

        var file = root.AddFile(Ann, "report.pdf", 10, "stored", Now);
        Assert.Equal(("report", ".pdf"), (file.Name, file.Extension));
        Assert.Throws<DomainException>(() => root.CheckCanDelete(Ann));
    }

    [Theory]
    [InlineData("photo.PNG", true)]
    [InlineData("page.html", false)]
    [InlineData("vector.svg", false)]
    public void Only_raster_images_are_shown_inline(string name, bool image) =>
        Assert.Equal(image, new Folder(Ann, "D").AddFile(Ann, name, 1, "s", Now).IsImage);

    [Fact]
    public void Names_are_required() =>
        Assert.Throws<DomainException>(() => new PostCategory("  "));
}
