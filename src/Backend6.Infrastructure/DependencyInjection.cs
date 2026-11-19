using Backend6.Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend6.Infrastructure;

public static class DependencyInjection
{
    /// <summary>The composition root for everything below the web layer.</summary>
    public static IServiceCollection AddBackend6(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(o => o.UseSqlite(config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("ConnectionStrings:Default is missing")));
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IForumRepository, ForumRepository>();
        services.AddScoped<IDriveRepository, DriveRepository>();
        services.AddSingleton<IFileStore>(new LocalFileStore(config["Storage:Path"] ?? "App_Data/files"));
        services.AddSingleton<IClock, SystemClock>();

        services.AddScoped<BlogService>();
        services.AddScoped<ForumService>();
        services.AddScoped<DriveService>();
        return services;
    }

    /// <summary>Migrate the database, make sure the admin role exists, and create the admin from
    /// Admin:Email / Admin:Password when both are configured (never from code).</summary>
    public static async Task InitializeBackend6Async(this IServiceProvider services, IConfiguration config)
    {
        using var scope = services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

        var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        if (!await roles.RoleExistsAsync(Roles.Administrators))
            Check(await roles.CreateAsync(new IdentityRole(Roles.Administrators)), "create the admin role");

        if (config["Admin:Email"] is not { Length: > 0 } email || config["Admin:Password"] is not { Length: > 0 } password) return;
        var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var admin = await users.FindByEmailAsync(email);
        if (admin is null)
        {
            admin = new ApplicationUser { UserName = email, Email = email };
            Check(await users.CreateAsync(admin, password), $"create the admin {email}");
        }
        if (!await users.IsInRoleAsync(admin, Roles.Administrators))
            Check(await users.AddToRoleAsync(admin, Roles.Administrators), $"make {email} an admin");
    }

    private static void Check(IdentityResult result, string what)
    {
        if (!result.Succeeded)
            throw new InvalidOperationException($"Unable to {what}: {string.Join("; ", result.Errors.Select(e => e.Description))}");
    }
}
