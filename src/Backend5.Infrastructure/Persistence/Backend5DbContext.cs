using Backend5.Application.Ports;
using Backend5.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend5.Infrastructure.Persistence;

public class Backend5DbContext(DbContextOptions<Backend5DbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Hospital> Hospitals => Set<Hospital>();
    public DbSet<Lab> Labs => Set<Lab>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Ward> Wards => Set<Ward>();

    async Task IUnitOfWork.SaveChangesAsync(CancellationToken ct) => await SaveChangesAsync(ct);

    protected override void OnModelCreating(ModelBuilder b)
    {
        // entities keep their collections in private fields behind read-only properties
        b.UsePropertyAccessMode(PropertyAccessMode.Field);

        b.Entity<Hospital>(e =>
        {
            e.Property(h => h.Name).HasMaxLength(200).IsRequired();
            e.Ignore(h => h.PhonesText);
            e.HasMany(h => h.Phones).WithOne(p => p.Hospital).HasForeignKey(p => p.HospitalId);
            e.HasMany(h => h.Wards).WithOne(w => w.Hospital).HasForeignKey(w => w.HospitalId);
        });
        b.Entity<HospitalPhone>(e =>
        {
            e.ToTable("HospitalPhones");
            e.HasKey(p => new { p.HospitalId, p.PhoneId });
            e.Property(p => p.Number).HasMaxLength(20).IsRequired();
        });

        b.Entity<Lab>(e =>
        {
            e.Property(l => l.Name).HasMaxLength(200).IsRequired();
            e.Ignore(l => l.PhonesText);
            e.HasMany(l => l.Phones).WithOne(p => p.Lab).HasForeignKey(p => p.LabId);
        });
        b.Entity<LabPhone>(e =>
        {
            e.ToTable("LabPhones");
            e.HasKey(p => new { p.LabId, p.PhoneId });
            e.Property(p => p.Number).HasMaxLength(20).IsRequired();
        });

        b.Entity<Doctor>(e => e.Property(d => d.Name).HasMaxLength(200).IsRequired());

        b.Entity<HospitalLab>(e =>
        {
            e.ToTable("HospitalLabs");
            e.HasKey(x => new { x.HospitalId, x.LabId });
            e.HasOne(x => x.Hospital).WithMany(h => h.Labs).HasForeignKey(x => x.HospitalId);
            e.HasOne(x => x.Lab).WithMany(l => l.Hospitals).HasForeignKey(x => x.LabId);
        });
        b.Entity<HospitalDoctor>(e =>
        {
            e.ToTable("HospitalDoctors");
            e.HasKey(x => new { x.HospitalId, x.DoctorId });
            e.HasOne(x => x.Hospital).WithMany(h => h.Doctors).HasForeignKey(x => x.HospitalId);
            e.HasOne(x => x.Doctor).WithMany(d => d.Hospitals).HasForeignKey(x => x.DoctorId);
        });

        b.Entity<Ward>(e =>
        {
            e.Property(w => w.Name).HasMaxLength(200).IsRequired();
            e.HasMany(w => w.Placements).WithOne(p => p.Ward).HasForeignKey(p => p.WardId);
            e.HasMany(w => w.WardStaffs).WithOne(s => s.Ward).HasForeignKey(s => s.WardId);
        });
        b.Entity<Placement>(e =>
        {
            e.ToTable("Placements");
            e.HasIndex(p => new { p.WardId, p.Bed }).IsUnique();  // one placement per bed in a ward
        });
        b.Entity<WardStaff>(e =>
        {
            e.ToTable("WardStaff");
            e.Property(s => s.Name).HasMaxLength(200).IsRequired();
        });
    }
}
