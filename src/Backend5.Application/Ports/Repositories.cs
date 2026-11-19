using Backend5.Core.Entities;

namespace Backend5.Application.Ports;

/// <summary>Hospitals, loaded with phones, labs (+ lab) and doctors (+ doctor).</summary>
public interface IHospitalRepository
{
    Task<IReadOnlyList<Hospital>> ListAsync(CancellationToken ct = default);
    Task<Hospital?> FindAsync(int id, CancellationToken ct = default);
    void Add(Hospital hospital);
    void Remove(Hospital hospital);
}

/// <summary>Labs, loaded with phones.</summary>
public interface ILabRepository
{
    Task<IReadOnlyList<Lab>> ListAsync(CancellationToken ct = default);
    Task<Lab?> FindAsync(int id, CancellationToken ct = default);
    void Add(Lab lab);
    void Remove(Lab lab);
}

public interface IDoctorRepository
{
    Task<IReadOnlyList<Doctor>> ListAsync(CancellationToken ct = default);
    Task<Doctor?> FindAsync(int id, CancellationToken ct = default);
    void Add(Doctor doctor);
    void Remove(Doctor doctor);
}

/// <summary>Wards, loaded with their hospital, placements and staff.</summary>
public interface IWardRepository
{
    Task<IReadOnlyList<Ward>> ListAsync(int? hospitalId, CancellationToken ct = default);
    Task<Ward?> FindAsync(int id, CancellationToken ct = default);
    Task<Ward?> FindByPlacementAsync(int placementId, CancellationToken ct = default);
    Task<Ward?> FindByStaffAsync(int staffId, CancellationToken ct = default);
    void Add(Ward ward);
    void Remove(Ward ward);
}

/// <summary>Commits everything the repositories changed, at once.</summary>
public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct = default);
}
