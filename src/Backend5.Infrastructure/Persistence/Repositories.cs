using Backend5.Application.Ports;
using Backend5.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend5.Infrastructure.Persistence;

internal class HospitalRepository(Backend5DbContext db) : IHospitalRepository
{
    private IQueryable<Hospital> Full => db.Hospitals
        .Include(h => h.Phones)
        .Include(h => h.Labs).ThenInclude(l => l.Lab)
        .Include(h => h.Doctors).ThenInclude(d => d.Doctor)
        .AsSplitQuery();

    public async Task<IReadOnlyList<Hospital>> ListAsync(CancellationToken ct) =>
        await db.Hospitals.OrderBy(h => h.Name).ToListAsync(ct);

    public Task<Hospital?> FindAsync(int id, CancellationToken ct) => Full.SingleOrDefaultAsync(h => h.Id == id, ct);

    public void Add(Hospital hospital) => db.Hospitals.Add(hospital);

    public void Remove(Hospital hospital) => db.Hospitals.Remove(hospital);
}

internal class LabRepository(Backend5DbContext db) : ILabRepository
{
    public async Task<IReadOnlyList<Lab>> ListAsync(CancellationToken ct) =>
        await db.Labs.OrderBy(l => l.Name).ToListAsync(ct);

    public Task<Lab?> FindAsync(int id, CancellationToken ct) =>
        db.Labs.Include(l => l.Phones).SingleOrDefaultAsync(l => l.Id == id, ct);

    public void Add(Lab lab) => db.Labs.Add(lab);

    public void Remove(Lab lab) => db.Labs.Remove(lab);
}

internal class DoctorRepository(Backend5DbContext db) : IDoctorRepository
{
    public async Task<IReadOnlyList<Doctor>> ListAsync(CancellationToken ct) =>
        await db.Doctors.OrderBy(d => d.Name).ToListAsync(ct);

    public Task<Doctor?> FindAsync(int id, CancellationToken ct) => db.Doctors.SingleOrDefaultAsync(d => d.Id == id, ct);

    public void Add(Doctor doctor) => db.Doctors.Add(doctor);

    public void Remove(Doctor doctor) => db.Doctors.Remove(doctor);
}

internal class WardRepository(Backend5DbContext db) : IWardRepository
{
    private IQueryable<Ward> Full => db.Wards
        .Include(w => w.Hospital)
        .Include(w => w.Placements)
        .Include(w => w.WardStaffs)
        .AsSplitQuery();

    public async Task<IReadOnlyList<Ward>> ListAsync(int? hospitalId, CancellationToken ct) =>
        await db.Wards.Include(w => w.Hospital)
            .Where(w => hospitalId == null || w.HospitalId == hospitalId)
            .OrderBy(w => w.Name)
            .ToListAsync(ct);

    public Task<Ward?> FindAsync(int id, CancellationToken ct) => Full.SingleOrDefaultAsync(w => w.Id == id, ct);

    public Task<Ward?> FindByPlacementAsync(int placementId, CancellationToken ct) =>
        Full.SingleOrDefaultAsync(w => w.Placements.Any(p => p.Id == placementId), ct);

    public Task<Ward?> FindByStaffAsync(int staffId, CancellationToken ct) =>
        Full.SingleOrDefaultAsync(w => w.WardStaffs.Any(s => s.Id == staffId), ct);

    public void Add(Ward ward) => db.Wards.Add(ward);

    public void Remove(Ward ward) => db.Wards.Remove(ward);
}
