namespace Backend5.Core.Entities;

/// <summary>A lab a hospital works with.</summary>
public class HospitalLab
{
    public int HospitalId { get; private set; }
    public Hospital Hospital { get; private set; } = null!;
    public int LabId { get; private set; }
    public Lab Lab { get; private set; } = null!;

    private HospitalLab() { }

    internal HospitalLab(Hospital hospital, Lab lab) =>
        (Hospital, HospitalId, Lab, LabId) = (hospital, hospital.Id, lab, lab.Id);
}

/// <summary>A doctor working at a hospital.</summary>
public class HospitalDoctor
{
    public int HospitalId { get; private set; }
    public Hospital Hospital { get; private set; } = null!;
    public int DoctorId { get; private set; }
    public Doctor Doctor { get; private set; } = null!;

    private HospitalDoctor() { }

    internal HospitalDoctor(Hospital hospital, Doctor doctor) =>
        (Hospital, HospitalId, Doctor, DoctorId) = (hospital, hospital.Id, doctor, doctor.Id);
}
