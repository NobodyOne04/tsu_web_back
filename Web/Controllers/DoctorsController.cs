using Backend5.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Forms;

namespace Web.Controllers;

public class DoctorsController(DoctorService doctors) : FormController
{
    public async Task<IActionResult> Index() => View(await doctors.ListAsync());

    public async Task<IActionResult> Details(int id) => View(await doctors.GetAsync(id));

    public IActionResult Create() => View(new DoctorForm());

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Create(DoctorForm model) => SubmitAsync(
        () => doctors.CreateAsync(model.Name, model.Speciality),
        () => RedirectToAction(nameof(Index)),
        () => View(model));

    public async Task<IActionResult> Edit(int id)
    {
        var doctor = await doctors.GetAsync(id);
        return View(new DoctorForm { Name = doctor.Name, Speciality = doctor.Speciality });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(int id, DoctorForm model) => SubmitAsync(
        () => doctors.UpdateAsync(id, model.Name, model.Speciality),
        () => RedirectToAction(nameof(Index)),
        () => View(model));

    public async Task<IActionResult> Delete(int id) => View(await doctors.GetAsync(id));

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await doctors.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
