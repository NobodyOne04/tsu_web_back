using Backend5.Application.Ports;
using Backend5.Core;
using Backend5.Core.Entities;

namespace Backend5.Application.Services;

public class DoctorService(IDoctorRepository doctors, IUnitOfWork uow)
{
    public Task<IReadOnlyList<Doctor>> ListAsync() => doctors.ListAsync();

    public async Task<Doctor> GetAsync(int id) =>
        await doctors.FindAsync(id) ?? throw new NotFoundException("Doctor", id);

    public async Task<Doctor> CreateAsync(string name, string? speciality)
    {
        var doctor = new Doctor(name, speciality);
        doctors.Add(doctor);
        await uow.SaveChangesAsync();
        return doctor;
    }

    public async Task UpdateAsync(int id, string name, string? speciality)
    {
        (await GetAsync(id)).Update(name, speciality);
        await uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        doctors.Remove(await GetAsync(id));
        await uow.SaveChangesAsync();
    }
}
