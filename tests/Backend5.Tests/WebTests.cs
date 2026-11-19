using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Backend5.Tests;

/// <summary>The whole app (controllers, services, EF Core, SQLite) on a temporary database file.</summary>
public partial class WebTests : IDisposable
{
    private readonly string _db = Path.Combine(Path.GetTempPath(), $"backend5-{Guid.NewGuid():N}.db");
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public WebTests()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(b =>
            b.UseSetting("ConnectionStrings:Default", $"Data Source={_db};Pooling=False"));
        _client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
        File.Delete(_db);
    }

    [GeneratedRegex("name=\"__RequestVerificationToken\" type=\"hidden\" value=\"([^\"]+)\"")]
    private static partial Regex Token();

    private async Task<HttpResponseMessage> Post(string formUrl, Dictionary<string, string> fields, string? action = null)
    {
        var page = await _client.GetStringAsync(formUrl);
        fields["__RequestVerificationToken"] = Token().Match(page).Groups[1].Value;
        return await _client.PostAsync(action ?? formUrl, new FormUrlEncodedContent(fields));
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Hospitals")]
    [InlineData("/Labs")]
    [InlineData("/Doctors")]
    [InlineData("/Hospitals/Create")]
    public async Task Pages_answer(string url) =>
        Assert.Equal(HttpStatusCode.OK, (await _client.GetAsync(url)).StatusCode);

    [Fact]
    public async Task Missing_things_are_404() =>
        Assert.Equal(HttpStatusCode.NotFound, (await _client.GetAsync("/Hospitals/Details/12345")).StatusCode);

    [Fact]
    public async Task Hospital_with_lab_ward_and_beds()
    {
        await Post("/Hospitals/Create", new() { ["Name"] = "City Hospital", ["Address"] = "Main 1", ["Phones"] = "111, 222" });
        await Post("/Labs/Create", new() { ["Name"] = "Blood Lab" });
        Assert.Contains("111", await _client.GetStringAsync("/Hospitals/Details/1"));

        await Post("/HospitalLabs/Create?hospitalId=1", new() { ["LabId"] = "1" });
        var again = await (await Post("/HospitalLabs/Create?hospitalId=1", new() { ["LabId"] = "1" })).Content.ReadAsStringAsync();
        Assert.Contains("already works with this hospital", again);
        Assert.Contains("Blood Lab", await _client.GetStringAsync("/HospitalLabs?hospitalId=1"));

        await Post("/Wards/Create?hospitalId=1", new() { ["Name"] = "Surgery" });
        await Post("/Placements/Create?wardId=1", new() { ["Bed"] = "7" });
        var duplicate = await (await Post("/Placements/Create?wardId=1", new() { ["Bed"] = "7" })).Content.ReadAsStringAsync();
        Assert.Contains("Bed 7 is already in this ward", duplicate);

        await Post("/Hospitals/Delete/1", new());
        Assert.Equal(HttpStatusCode.NotFound, (await _client.GetAsync("/Wards/Details/1")).StatusCode);
    }
}
