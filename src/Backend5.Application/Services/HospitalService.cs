using Backend5.Application.Ports;
using Backend5.Core;
using Backend5.Core.Entities;

namespace Backend5.Application.Services;

public class HospitalService(IHospitalRepository hospitals, ILabRepository labs, IDoctorRepository doctors, IUnitOfWork uow)
{
    public Task<IReadOnlyList<Hospital>> ListAsync() => hospitals.ListAsync();

    public async Task<Hospital> GetAsync(int id) =>
        await hospitals.FindAsync(id) ?? throw new NotFoundException("Hospital", id);

    public async Task<Hospital> CreateAsync(string name, string? address, string? phones)
    {
        var hospital = new Hospital(name, address, phones);
        hospitals.Add(hospital);
        await uow.SaveChangesAsync();
        return hospital;
    }

    public async Task UpdateAsync(int id, string name, string? address, string? phones)
    {
        (await GetAsync(id)).Update(name, address, phones);
        await uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        hospitals.Remove(await GetAsync(id));
        await uow.SaveChangesAsync();
    }

    public async Task AttachLabAsync(int hospitalId, int labId)
    {
        var hospital = await GetAsync(hospitalId);
        var lab = await labs.FindAsync(labId) ?? throw new DomainException("Choose a lab.");
        hospital.AttachLab(lab);
        await uow.SaveChangesAsync();
    }

    public async Task DetachLabAsync(int hospitalId, int labId)
    {
        (await GetAsync(hospitalId)).DetachLab(labId);
        await uow.SaveChangesAsync();
    }

    public async Task AssignDoctorAsync(int hospitalId, int doctorId)
    {
        var hospital = await GetAsync(hospitalId);
        var doctor = await doctors.FindAsync(doctorId) ?? throw new DomainException("Choose a doctor.");
        hospital.AssignDoctor(doctor);
        await uow.SaveChangesAsync();
    }

    public async Task UnassignDoctorAsync(int hospitalId, int doctorId)
    {
        (await GetAsync(hospitalId)).UnassignDoctor(doctorId);
        await uow.SaveChangesAsync();
    }
}
