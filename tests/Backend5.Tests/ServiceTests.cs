using Backend5.Application.Ports;
using Backend5.Application.Services;
using Backend5.Core;
using Backend5.Core.Entities;

namespace Backend5.Tests;

/// <summary>The application layer over in-memory fakes: no EF Core, no database.</summary>
public class ServiceTests
{
    private sealed class Store<T>(Func<T, int> id) where T : class
    {
        public readonly List<T> Items = [];
        public Task<IReadOnlyList<T>> List() => Task.FromResult<IReadOnlyList<T>>(Items.ToList());
        public Task<T?> Find(int key) => Task.FromResult(Items.SingleOrDefault(x => id(x) == key));
    }

    private sealed class Fakes : IHospitalRepository, ILabRepository, IDoctorRepository, IUnitOfWork
    {
        public readonly Store<Hospital> Hospitals = new(h => h.Id);
        public readonly Store<Lab> Labs = new(l => l.Id);
        public readonly Store<Doctor> Doctors = new(d => d.Id);
        public int Saves;

        Task<IReadOnlyList<Hospital>> IHospitalRepository.ListAsync(CancellationToken ct) => Hospitals.List();
        Task<Hospital?> IHospitalRepository.FindAsync(int id, CancellationToken ct) => Hospitals.Find(id);
        void IHospitalRepository.Add(Hospital h) => Hospitals.Items.Add(h);
        void IHospitalRepository.Remove(Hospital h) => Hospitals.Items.Remove(h);
        Task<IReadOnlyList<Lab>> ILabRepository.ListAsync(CancellationToken ct) => Labs.List();
        Task<Lab?> ILabRepository.FindAsync(int id, CancellationToken ct) => Labs.Find(id);
        void ILabRepository.Add(Lab l) => Labs.Items.Add(l);
        void ILabRepository.Remove(Lab l) => Labs.Items.Remove(l);
        Task<IReadOnlyList<Doctor>> IDoctorRepository.ListAsync(CancellationToken ct) => Doctors.List();
        Task<Doctor?> IDoctorRepository.FindAsync(int id, CancellationToken ct) => Doctors.Find(id);
        void IDoctorRepository.Add(Doctor d) => Doctors.Items.Add(d);
        void IDoctorRepository.Remove(Doctor d) => Doctors.Items.Remove(d);
        Task IUnitOfWork.SaveChangesAsync(CancellationToken ct) { Saves++; return Task.CompletedTask; }
    }

    private static (HospitalService, Fakes) Service()
    {
        var f = new Fakes();
        return (new HospitalService(f, f, f, f), f);
    }

    [Fact]
    public async Task Create_adds_and_saves()
    {
        var (service, f) = Service();
        var hospital = await service.CreateAsync("H", "Street 1", "123");
        Assert.Same(hospital, f.Hospitals.Items.Single());
        Assert.Equal(1, f.Saves);
    }

    [Fact]
    public async Task Missing_hospital_is_not_found()
    {
        var (service, _) = Service();
        await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateAsync(7, "X", null, null));
    }

    [Fact]
    public async Task Attaching_an_unknown_lab_asks_to_choose_one()
    {
        var (service, f) = Service();
        f.Hospitals.Items.Add(new Hospital("H", null, null));
        var e = await Assert.ThrowsAsync<DomainException>(() => service.AttachLabAsync(0, 99));
        Assert.Equal("Choose a lab.", e.Message);
        Assert.Equal(0, f.Saves);
    }

    [Fact]
    public async Task Invalid_input_saves_nothing()
    {
        var (service, f) = Service();
        await Assert.ThrowsAsync<DomainException>(() => service.CreateAsync("", null, null));
        Assert.Empty(f.Hospitals.Items);
        Assert.Equal(0, f.Saves);
    }
}
