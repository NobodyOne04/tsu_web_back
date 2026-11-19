namespace Backend5.Core.Entities;

public class Doctor
{
    private readonly List<HospitalDoctor> _hospitals = [];

    public int Id { get; private set; }
    public string Name { get; private set; } = "";
    public string? Speciality { get; private set; }
    public IReadOnlyCollection<HospitalDoctor> Hospitals => _hospitals;

    private Doctor() { }

    public Doctor(string name, string? speciality) => Update(name, speciality);

    public void Update(string name, string? speciality)
    {
        Name = Text.Required(name, "Name");
        Speciality = Text.Optional(speciality);
    }
}
