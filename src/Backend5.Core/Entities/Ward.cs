namespace Backend5.Core.Entities;

/// <summary>A ward of a hospital, with its beds (placements) and staff.</summary>
public class Ward
{
    private readonly List<Placement> _placements = [];
    private readonly List<WardStaff> _wardStaffs = [];

    public int Id { get; private set; }
    public int HospitalId { get; private set; }
    public Hospital Hospital { get; private set; } = null!;
    public string Name { get; private set; } = "";
    public IReadOnlyCollection<Placement> Placements => _placements;
    public IReadOnlyCollection<WardStaff> WardStaffs => _wardStaffs;

    private Ward() { }

    public Ward(Hospital hospital, string name)
    {
        (Hospital, HospitalId) = (hospital, hospital.Id);
        Rename(name);
    }

    public void Rename(string name) => Name = Text.Required(name, "Name");

    public Placement AddPlacement(int bed)
    {
        CheckBed(bed, except: null);
        var placement = new Placement(this, bed);
        _placements.Add(placement);
        return placement;
    }

    public void MovePlacement(int placementId, int bed)
    {
        var placement = Placement(placementId);
        CheckBed(bed, except: placement);
        placement.Bed = bed;
    }

    public void RemovePlacement(int placementId) => _placements.Remove(Placement(placementId));

    public WardStaff AddStaff(string name, string? position)
    {
        var staff = new WardStaff(this, name, position);
        _wardStaffs.Add(staff);
        return staff;
    }

    public void UpdateStaff(int staffId, string name, string? position) => Staff(staffId).Update(name, position);

    public void RemoveStaff(int staffId) => _wardStaffs.Remove(Staff(staffId));

    public Placement Placement(int id) =>
        _placements.SingleOrDefault(p => p.Id == id) ?? throw new NotFoundException("Placement", id);

    public WardStaff Staff(int id) =>
        _wardStaffs.SingleOrDefault(s => s.Id == id) ?? throw new NotFoundException("Ward staff", id);

    private void CheckBed(int bed, Placement? except)
    {
        if (bed <= 0) throw new DomainException("Bed must be a positive number.");
        if (_placements.Any(p => p.Bed == bed && p != except))
            throw new DomainException($"Bed {bed} is already in this ward.");
    }
}

/// <summary>A bed in a ward.</summary>
public class Placement
{
    public int Id { get; private set; }
    public int Bed { get; internal set; }
    public int WardId { get; private set; }
    public Ward Ward { get; private set; } = null!;

    private Placement() { }

    internal Placement(Ward ward, int bed) => (Ward, WardId, Bed) = (ward, ward.Id, bed);
}

/// <summary>A member of a ward's staff.</summary>
public class WardStaff
{
    public int Id { get; private set; }
    public string Name { get; private set; } = "";
    public string? Position { get; private set; }
    public int WardId { get; private set; }
    public Ward Ward { get; private set; } = null!;

    private WardStaff() { }

    internal WardStaff(Ward ward, string name, string? position)
    {
        (Ward, WardId) = (ward, ward.Id);
        Update(name, position);
    }

    internal void Update(string name, string? position)
    {
        Name = Text.Required(name, "Name");
        Position = Text.Optional(position);
    }
}
