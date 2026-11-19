namespace Backend5.Core.Entities;

/// <summary>A laboratory with its phones.</summary>
public class Lab
{
    private readonly List<LabPhone> _phones = [];
    private readonly List<HospitalLab> _hospitals = [];

    public int Id { get; private set; }
    public string Name { get; private set; } = "";
    public string? Address { get; private set; }
    public IReadOnlyCollection<LabPhone> Phones => _phones;
    public IReadOnlyCollection<HospitalLab> Hospitals => _hospitals;

    private Lab() { }

    public Lab(string name, string? address, string? phones)
    {
        Update(name, address, phones);
    }

    public void Update(string name, string? address, string? phones)
    {
        Name = Text.Required(name, "Name");
        Address = Text.Optional(address);
        var next = _phones.Count == 0 ? 1 : _phones.Max(p => p.PhoneId) + 1;
        _phones.Clear();
        foreach (var number in Text.PhoneList(phones))
            _phones.Add(new LabPhone(next++, number));
    }

    public string PhonesText => string.Join(", ", _phones.OrderBy(p => p.PhoneId).Select(p => p.Number));
}
