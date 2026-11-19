using Backend5.Core;
using Backend5.Core.Entities;

namespace Backend5.Tests;

public class DomainTests
{
    [Fact]
    public void Phones_are_parsed_trimmed_and_numbered_in_order()
    {
        var hospital = new Hospital(" City ", "", " 111 ,, 222 ,");
        Assert.Equal("City", hospital.Name);
        Assert.Null(hospital.Address);
        Assert.Equal(new[] { (1, "111"), (2, "222") }, hospital.Phones.Select(p => (p.PhoneId, p.Number)));
        Assert.Equal("111, 222", hospital.PhonesText);
    }

    [Fact]
    public void Updating_phones_keeps_numbering_unique()
    {
        var lab = new Lab("Lab", null, "1, 2");
        lab.Update("Lab", null, "3");
        Assert.Equal(3, lab.Phones.Single().PhoneId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void A_name_is_required(string name) =>
        Assert.Throws<DomainException>(() => new Doctor(name, null));

    [Fact]
    public void Too_long_phone_is_rejected() =>
        Assert.Throws<DomainException>(() => new Hospital("H", null, new string('9', 21)));

    [Fact]
    public void A_lab_or_doctor_is_linked_once()
    {
        var hospital = new Hospital("H", null, null);
        var lab = new Lab("L", null, null);
        var doctor = new Doctor("D", null);
        hospital.AttachLab(lab);
        hospital.AssignDoctor(doctor);
        Assert.Throws<DomainException>(() => hospital.AttachLab(lab));
        Assert.Throws<DomainException>(() => hospital.AssignDoctor(doctor));
    }

    [Fact]
    public void Detaching_an_unknown_link_is_not_found() =>
        Assert.Throws<NotFoundException>(() => new Hospital("H", null, null).DetachLab(42));

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void Beds_are_positive(int bed) =>
        Assert.Throws<DomainException>(() => new Ward(new Hospital("H", null, null), "W").AddPlacement(bed));

    [Fact]
    public void A_bed_is_used_once_per_ward()
    {
        var ward = new Ward(new Hospital("H", null, null), "W");
        ward.AddPlacement(1);
        Assert.Throws<DomainException>(() => ward.AddPlacement(1));
    }

    [Fact]
    public void Staff_needs_a_name()
    {
        var ward = new Ward(new Hospital("H", null, null), "W");
        Assert.Throws<DomainException>(() => ward.AddStaff(" ", "Nurse"));
        Assert.Equal("Nurse", ward.AddStaff("Ann", " Nurse ").Position);
    }
}
