using Backend5.Application.Ports;
using Backend5.Core;
using Backend5.Core.Entities;

namespace Backend5.Application.Services;

/// <summary>Wards and what belongs to them: placements (beds) and staff.</summary>
public class WardService(IWardRepository wards, IHospitalRepository hospitals, IUnitOfWork uow)
{
    public Task<IReadOnlyList<Ward>> ListAsync(int? hospitalId = null) => wards.ListAsync(hospitalId);

    public async Task<Ward> GetAsync(int id) => await wards.FindAsync(id) ?? throw new NotFoundException("Ward", id);

    public async Task<Ward> CreateAsync(int hospitalId, string name)
    {
        var hospital = await hospitals.FindAsync(hospitalId) ?? throw new NotFoundException("Hospital", hospitalId);
        var ward = new Ward(hospital, name);
        wards.Add(ward);
        await uow.SaveChangesAsync();
        return ward;
    }

    public async Task RenameAsync(int id, string name)
    {
        (await GetAsync(id)).Rename(name);
        await uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        wards.Remove(await GetAsync(id));
        await uow.SaveChangesAsync();
    }

    public async Task<Placement> AddPlacementAsync(int wardId, int bed)
    {
        var placement = (await GetAsync(wardId)).AddPlacement(bed);
        await uow.SaveChangesAsync();
        return placement;
    }

    public async Task<Placement> GetPlacementAsync(int placementId) =>
        (await WardOfPlacement(placementId)).Placement(placementId);

    public async Task MovePlacementAsync(int placementId, int bed)
    {
        (await WardOfPlacement(placementId)).MovePlacement(placementId, bed);
        await uow.SaveChangesAsync();
    }

    public async Task<int> RemovePlacementAsync(int placementId)
    {
        var ward = await WardOfPlacement(placementId);
        ward.RemovePlacement(placementId);
        await uow.SaveChangesAsync();
        return ward.Id;
    }

    public async Task<WardStaff> AddStaffAsync(int wardId, string name, string? position)
    {
        var staff = (await GetAsync(wardId)).AddStaff(name, position);
        await uow.SaveChangesAsync();
        return staff;
    }

    public async Task<WardStaff> GetStaffAsync(int staffId) => (await WardOfStaff(staffId)).Staff(staffId);

    public async Task UpdateStaffAsync(int staffId, string name, string? position)
    {
        (await WardOfStaff(staffId)).UpdateStaff(staffId, name, position);
        await uow.SaveChangesAsync();
    }

    public async Task<int> RemoveStaffAsync(int staffId)
    {
        var ward = await WardOfStaff(staffId);
        ward.RemoveStaff(staffId);
        await uow.SaveChangesAsync();
        return ward.Id;
    }

    private async Task<Ward> WardOfPlacement(int placementId) =>
        await wards.FindByPlacementAsync(placementId) ?? throw new NotFoundException("Placement", placementId);

    private async Task<Ward> WardOfStaff(int staffId) =>
        await wards.FindByStaffAsync(staffId) ?? throw new NotFoundException("Ward staff", staffId);
}
