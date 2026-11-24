namespace Backend5.Core.Entities;

public class HospitalPhone
{
    public int HospitalId { get; private set; }
    public Hospital Hospital { get; private set; } = null!;
    public int PhoneId { get; private set; }
    public string Number { get; private set; } = "";

    private HospitalPhone() { }

    internal HospitalPhone(int phoneId, string number) => (PhoneId, Number) = (phoneId, number);
}

public class LabPhone
{
    public int LabId { get; private set; }
    public Lab Lab { get; private set; } = null!;
    public int PhoneId { get; private set; }
    public string Number { get; private set; } = "";

    private LabPhone() { }

    internal LabPhone(int phoneId, string number) => (PhoneId, Number) = (phoneId, number);
}
