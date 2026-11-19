namespace Backend5.Core.Entities;

/// <summary>A hospital with its phones and the labs and doctors it works with.</summary>
public class Hospital
{
    private readonly List<HospitalPhone> _phones = [];
    private readonly List<HospitalLab> _labs = [];
    private readonly List<HospitalDoctor> _doctors = [];
    private readonly List<Ward> _wards = [];

    public int Id { get; private set; }
    public string Name { get; private set; } = "";
    public string? Address { get; private set; }
    public IReadOnlyCollection<HospitalPhone> Phones => _phones;
    public IReadOnlyCollection<HospitalLab> Labs => _labs;
    public IReadOnlyCollection<HospitalDoctor> Doctors => _doctors;
    public IReadOnlyCollection<Ward> Wards => _wards;

    private Hospital() { }

    public Hospital(string name, string? address, string? phones)
    {
        Update(name, address, phones);
    }

    /// <summary>Phones as typed in the form: comma-separated, numbered in order.</summary>
    public void Update(string name, string? address, string? phones)
    {
        Name = Text.Required(name, "Name");
        Address = Text.Optional(address);
        var next = _phones.Count == 0 ? 1 : _phones.Max(p => p.PhoneId) + 1;
        _phones.Clear();
        foreach (var number in Text.PhoneList(phones))
            _phones.Add(new HospitalPhone(next++, number));
    }

    public string PhonesText => string.Join(", ", _phones.OrderBy(p => p.PhoneId).Select(p => p.Number));

    public void AttachLab(Lab lab)
    {
        if (_labs.Any(l => l.LabId == lab.Id))
            throw new DomainException($"{lab.Name} already works with this hospital.");
        _labs.Add(new HospitalLab(this, lab));
    }

    public void DetachLab(int labId) =>
        _labs.Remove(_labs.SingleOrDefault(l => l.LabId == labId) ?? throw new NotFoundException("Hospital lab", labId));

    public void AssignDoctor(Doctor doctor)
    {
        if (_doctors.Any(d => d.DoctorId == doctor.Id))
            throw new DomainException($"{doctor.Name} already works at this hospital.");
        _doctors.Add(new HospitalDoctor(this, doctor));
    }

    public void UnassignDoctor(int doctorId) =>
        _doctors.Remove(_doctors.SingleOrDefault(d => d.DoctorId == doctorId) ?? throw new NotFoundException("Hospital doctor", doctorId));
}
