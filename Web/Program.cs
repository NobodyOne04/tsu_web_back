using Backend5.Infrastructure;
using Backend5.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBackend5(builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("ConnectionStrings:Default is missing"));
builder.Services.AddControllersWithViews(o => o.Filters.Add<NotFoundFilter>());

var app = builder.Build();

// bring the database schema up to date (SQLite file by default)
using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<Backend5DbContext>().Database.MigrateAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();

public partial class Program;  // visible to WebApplicationFactory in the tests
