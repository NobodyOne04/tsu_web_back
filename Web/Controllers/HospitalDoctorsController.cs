using Backend5.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Forms;

namespace Web.Controllers;

/// <summary>The doctors working at a hospital.</summary>
public class HospitalDoctorsController(HospitalService hospitals, DoctorService doctors) : FormController
{
    public async Task<IActionResult> Index(int hospitalId)
    {
        var hospital = await hospitals.GetAsync(hospitalId);
        ViewBag.Hospital = hospital;
        return View(hospital.Doctors.OrderBy(d => d.Doctor.Name));
    }

    public async Task<IActionResult> Create(int hospitalId) => await ShowForm(hospitalId, new HospitalDoctorForm());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int hospitalId, HospitalDoctorForm model)
    {
        IActionResult showForm = await ShowForm(hospitalId, model);
        return await SubmitAsync(
            () => hospitals.AssignDoctorAsync(hospitalId, model.DoctorId),
            () => RedirectToAction(nameof(Index), new { hospitalId }),
            () => showForm);
    }

    public async Task<IActionResult> Delete(int hospitalId, int doctorId)
    {
        var hospital = await hospitals.GetAsync(hospitalId);
        var link = hospital.Doctors.SingleOrDefault(d => d.DoctorId == doctorId);
        return link is null ? NotFound() : View(link);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int hospitalId, int doctorId)
    {
        await hospitals.UnassignDoctorAsync(hospitalId, doctorId);
        return RedirectToAction(nameof(Index), new { hospitalId });
    }

    private async Task<IActionResult> ShowForm(int hospitalId, HospitalDoctorForm model)
    {
        ViewBag.Hospital = await hospitals.GetAsync(hospitalId);
        // was ViewData["LabId"] on a failed POST, so the doctor list vanished
        ViewBag.DoctorId = new SelectList(await doctors.ListAsync(), "Id", "Name", model.DoctorId);
        return View(nameof(Create), model);
    }
}
