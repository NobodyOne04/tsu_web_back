using Backend5.Application.Ports;
using Backend5.Application.Services;
using Backend5.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Backend5.Infrastructure;

public static class DependencyInjection
{
    /// <summary>The composition root for everything below the web layer.</summary>
    public static IServiceCollection AddBackend5(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Backend5DbContext>(o => o.UseSqlite(connectionString));
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<Backend5DbContext>());
        services.AddScoped<IHospitalRepository, HospitalRepository>();
        services.AddScoped<ILabRepository, LabRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IWardRepository, WardRepository>();
        services.AddScoped<HospitalService>();
        services.AddScoped<LabService>();
        services.AddScoped<DoctorService>();
        services.AddScoped<WardService>();
        return services;
    }
}
