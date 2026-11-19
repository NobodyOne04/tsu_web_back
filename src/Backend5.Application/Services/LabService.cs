using Backend5.Application.Ports;
using Backend5.Core;
using Backend5.Core.Entities;

namespace Backend5.Application.Services;

public class LabService(ILabRepository labs, IUnitOfWork uow)
{
    public Task<IReadOnlyList<Lab>> ListAsync() => labs.ListAsync();

    public async Task<Lab> GetAsync(int id) => await labs.FindAsync(id) ?? throw new NotFoundException("Lab", id);

    public async Task<Lab> CreateAsync(string name, string? address, string? phones)
    {
        var lab = new Lab(name, address, phones);
        labs.Add(lab);
        await uow.SaveChangesAsync();
        return lab;
    }

    public async Task UpdateAsync(int id, string name, string? address, string? phones)
    {
        (await GetAsync(id)).Update(name, address, phones);
        await uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        labs.Remove(await GetAsync(id));
        await uow.SaveChangesAsync();
    }
}
