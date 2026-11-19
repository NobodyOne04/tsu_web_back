using System.ComponentModel.DataAnnotations;

namespace Web.Forms;

// What the views post. Field checks here give quick feedback; the domain enforces the real rules.

public class HospitalCreateForm
{
    [Required, MaxLength(200)] public string Name { get; set; } = "";
    public string? Address { get; set; }
    [Display(Name = "Phones (comma-separated)")] public string? Phones { get; set; }
}

public class HospitalEditForm : HospitalCreateForm;

public class LabCreateForm
{
    [Required, MaxLength(200)] public string Name { get; set; } = "";
    public string? Address { get; set; }
    [Display(Name = "Phones (comma-separated)")] public string? Phones { get; set; }
}

public class LabEditForm : LabCreateForm;

public class DoctorForm
{
    [Required, MaxLength(200)] public string Name { get; set; } = "";
    public string? Speciality { get; set; }
}

public class HospitalLabCreateForm
{
    [Display(Name = "Lab")] public int LabId { get; set; }
}

public class HospitalDoctorForm
{
    [Display(Name = "Doctor")] public int DoctorId { get; set; }
}

public class WardCreateForm
{
    [Required, MaxLength(200)] public string Name { get; set; } = "";
}

public class WardEditForm : WardCreateForm;

public class PlacementForm
{
    [Range(1, int.MaxValue, ErrorMessage = "Bed must be a positive number.")] public int Bed { get; set; }
}

public class WardStaffForm
{
    [Required, MaxLength(200)] public string Name { get; set; } = "";
    public string? Position { get; set; }
}
