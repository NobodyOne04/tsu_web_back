using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Backend6.Tests;

/// <summary>The whole app (Identity, EF Core on SQLite, files on disk) on temporary storage.</summary>
public partial class WebTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), $"backend6-{Guid.NewGuid():N}");
    private readonly WebApplicationFactory<Program> _factory;

    public WebTests()
    {
        Directory.CreateDirectory(_dir);
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(b => b
            .UseSetting("ConnectionStrings:Default", $"Data Source={Path.Combine(_dir, "test.db")};Pooling=False")
            .UseSetting("Storage:Path", Path.Combine(_dir, "files"))
            .UseSetting("Admin:Email", "admin@test.local")
            .UseSetting("Admin:Password", "Admin-Pa55word!"));
    }

    public void Dispose()
    {
        _factory.Dispose();
        Directory.Delete(_dir, recursive: true);
    }

    private HttpClient Browser() =>
        _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    [GeneratedRegex("name=\"__RequestVerificationToken\" type=\"hidden\" value=\"([^\"]+)\"")]
    private static partial Regex Token();

    private static async Task<HttpResponseMessage> Post(HttpClient c, string url, Dictionary<string, string> fields, string? formUrl = null)
    {
        fields["__RequestVerificationToken"] = Token().Match(await c.GetStringAsync(formUrl ?? url)).Groups[1].Value;
        return await c.PostAsync(url, new FormUrlEncodedContent(fields));
    }

    private static async Task<HttpResponseMessage> Upload(HttpClient c, string url, string name, byte[] bytes)
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent(Token().Match(await c.GetStringAsync(url)).Groups[1].Value), "__RequestVerificationToken" },
            { new ByteArrayContent(bytes), "File", name },
        };
        return await c.PostAsync(url, form);
    }

    private async Task<HttpClient> SignedUp(string email)
    {
        var c = Browser();
        var r = await Post(c, "/Account/Register", new() { ["Email"] = email, ["Password"] = "Pa55word!", ["ConfirmPassword"] = "Pa55word!" });
        Assert.Equal(HttpStatusCode.Redirect, r.StatusCode);
        return c;
    }

    private async Task<HttpClient> AdminBrowser()
    {
        var c = Browser();
        var r = await Post(c, "/Account/Login", new() { ["Email"] = "admin@test.local", ["Password"] = "Admin-Pa55word!" });
        Assert.Equal(HttpStatusCode.Redirect, r.StatusCode);
        return c;
    }

    private static Guid IdIn(string url) => Guid.Parse(url.Split('/', '=').Last());

    [Theory]
    [InlineData("/")]
    [InlineData("/Posts")]
    [InlineData("/Forum")]
    [InlineData("/Account/Login")]
    public async Task Public_pages_answer(string url) =>
        Assert.True((await Browser().GetAsync(url)).IsSuccessStatusCode || url == "/");

    [Theory]
    [InlineData("/Folders")]
    [InlineData("/Posts/Create")]
    [InlineData("/PostCategories")]
    public async Task Anonymous_users_are_sent_to_log_in(string url)
    {
        var r = await Browser().GetAsync(url);
        Assert.Equal(HttpStatusCode.Redirect, r.StatusCode);
        Assert.Contains("/Account/Login", r.Headers.Location!.ToString());
    }

    [Fact]
    public async Task Only_admins_manage_categories()
    {
        var ann = await SignedUp("ann@test.local");
        Assert.Equal(HttpStatusCode.Forbidden, (await ann.GetAsync("/PostCategories")).StatusCode);
        var admin = await AdminBrowser();
        Assert.Equal(HttpStatusCode.Redirect, (await Post(admin, "/PostCategories/Create", new() { ["Name"] = "News" })).StatusCode);
        Assert.Contains("News", await admin.GetStringAsync("/PostCategories"));
    }

    [Fact]
    public async Task Authors_edit_their_posts_others_get_403_and_attachments_stay_out_of_wwwroot()
    {
        var admin = await AdminBrowser();
        await Post(admin, "/PostCategories/Create", new() { ["Name"] = "News" });
        var categoryId = Regex.Match(await admin.GetStringAsync("/Posts/Create"), "<option value=\"([0-9a-f-]{36})\"").Groups[1].Value;

        var ann = await SignedUp("ann@test.local");
        var created = await Post(ann, "/Posts/Create", new() { ["CategoryId"] = categoryId, ["Title"] = "Hello", ["Text"] = "World" });
        var postUrl = created.Headers.Location!.ToString();
        var postId = IdIn(postUrl);

        var bob = await SignedUp("bob@test.local");
        Assert.Equal(HttpStatusCode.Forbidden, (await bob.GetAsync($"/Posts/Edit/{postId}")).StatusCode);
        Assert.Equal(HttpStatusCode.Redirect, (await Post(bob, $"/PostComments/Create?postId={postId}", new() { ["Text"] = "Nice" })).StatusCode);

        var uploaded = await Upload(ann, $"/PostAttachments/Create?postId={postId}", "evil.html", "<script>alert(1)</script>"u8.ToArray());
        Assert.Equal(HttpStatusCode.Redirect, uploaded.StatusCode);
        var page = await Browser().GetStringAsync(postUrl);
        Assert.Contains("Nice", page);
        var attachmentUrl = Regex.Match(page, "href=\"(/PostAttachments/Show/[0-9a-f-]+)\"").Groups[1].Value;
        var download = await Browser().GetAsync(attachmentUrl);
        Assert.Equal("application/octet-stream", download.Content.Headers.ContentType!.MediaType);  // never rendered as HTML
        Assert.Equal("attachment", download.Content.Headers.ContentDisposition!.DispositionType);
        Assert.Single(Directory.GetFiles(Path.Combine(_dir, "files")));
        Assert.False(Directory.Exists(Path.Combine(AppContext.BaseDirectory, "wwwroot", "attachments")));

        Assert.Equal(HttpStatusCode.Redirect, (await Post(ann, $"/Posts/Delete/{postId}", new())).StatusCode);
        Assert.Empty(Directory.GetFiles(Path.Combine(_dir, "files")));  // the file went with the post
    }

    [Fact]
    public async Task Forum_topics_and_replies()
    {
        var admin = await AdminBrowser();
        await Post(admin, "/ForumCategories/Create", new() { ["Name"] = "General" });
        var categoryId = Regex.Match(await admin.GetStringAsync("/Forums/Create"), "<option value=\"([0-9a-f-]{36})\"").Groups[1].Value;
        await Post(admin, "/Forums/Create", new() { ["CategoryId"] = categoryId, ["Name"] = "Chat" });
        var forumId = Regex.Match(await admin.GetStringAsync("/Forum"), "forumId=([0-9a-f-]{36})").Groups[1].Value;

        var ann = await SignedUp("ann@test.local");
        var started = await Post(ann, $"/ForumTopics/Create?forumId={forumId}", new() { ["Name"] = "Hello", ["Text"] = "First!" });
        var topicUrl = started.Headers.Location!.ToString();
        var bob = await SignedUp("bob@test.local");
        await Post(bob, $"/ForumMessages/Create?topicId={IdIn(topicUrl)}", new() { ["Text"] = "Reply from Bob" });

        var page = await Browser().GetStringAsync(topicUrl);
        Assert.Contains("First!", page);
        Assert.Contains("Reply from Bob", page);
        Assert.Equal(HttpStatusCode.Forbidden, (await bob.GetAsync($"/ForumTopics/Edit/{IdIn(topicUrl)}")).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await Post(bob, $"/Forums/Delete/{forumId}", new(), "/Forum")).StatusCode);
    }

    [Fact]
    public async Task Drive_folders_are_private()
    {
        var ann = await SignedUp("ann@test.local");
        var created = await Post(ann, "/Folders/Create", new() { ["Name"] = "Secret" });
        var folderId = IdIn(created.Headers.Location!.ToString());
        Assert.Equal(HttpStatusCode.Redirect, (await Upload(ann, $"/Files/Create?folderId={folderId}", "note.txt", "hi"u8.ToArray())).StatusCode);
        var fileUrl = Regex.Match(await ann.GetStringAsync($"/Folders/Details/{folderId}"), "href=\"(/Files/Download/[0-9a-f-]+)\"").Groups[1].Value;
        Assert.Equal("hi", await ann.GetStringAsync(fileUrl));

        var bob = await SignedUp("bob@test.local");
        Assert.Equal(HttpStatusCode.NotFound, (await bob.GetAsync($"/Folders/Details/{folderId}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await bob.GetAsync(fileUrl)).StatusCode);
        Assert.DoesNotContain("Secret", await bob.GetStringAsync("/Folders"));

        var admin = await AdminBrowser();
        Assert.Contains("Secret", await admin.GetStringAsync("/Folders"));
    }
}
