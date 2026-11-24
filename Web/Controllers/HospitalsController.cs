using Backend5.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Forms;

namespace Web.Controllers;

public class HospitalsController(HospitalService hospitals) : FormController
{
    public async Task<IActionResult> Index() => View(await hospitals.ListAsync());

    public async Task<IActionResult> Details(int id) => View(await hospitals.GetAsync(id));

    public IActionResult Create() => View(new HospitalCreateForm());

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Create(HospitalCreateForm model) => SubmitAsync(
        () => hospitals.CreateAsync(model.Name, model.Address, model.Phones),
        () => RedirectToAction(nameof(Index)),
        () => View(model));

    public async Task<IActionResult> Edit(int id)
    {
        var hospital = await hospitals.GetAsync(id);
        return View(new HospitalEditForm { Name = hospital.Name, Address = hospital.Address, Phones = hospital.PhonesText });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public Task<IActionResult> Edit(int id, HospitalEditForm model) => SubmitAsync(
        () => hospitals.UpdateAsync(id, model.Name, model.Address, model.Phones),
        () => RedirectToAction(nameof(Index)),
        () => View(model));

    public async Task<IActionResult> Delete(int id) => View(await hospitals.GetAsync(id));

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await hospitals.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
