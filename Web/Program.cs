using Backend6.Infrastructure;
using Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBackend6(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ActorAccessor>();
builder.Services.AddControllersWithViews(o => o.Filters.Add<DomainErrorsFilter>());
builder.Services.ConfigureApplicationCookie(o =>
{
    o.LoginPath = "/Account/Login";
    // signed in but not allowed: a plain 403, not a redirect to a page that doesn't exist
    o.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

var app = builder.Build();

await app.Services.InitializeBackend6Async(app.Configuration);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Posts}/{action=Index}/{id?}");

await app.RunAsync();

public partial class Program;  // visible to WebApplicationFactory in the tests
